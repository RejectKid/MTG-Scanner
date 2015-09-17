using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using MTG_Scanner.Models;

namespace MTG_Scanner.Utils.Impl
{
    public class Util : IUtil
    {
        //private readonly IMagicCardFactory _magicCardFactory;
        private readonly Regex _matchUntilDot = new Regex(@"^([^.]*)");

        public Util()
        //public Util( IMagicCardFactory magicCardFactory )
        {
            //_magicCardFactory = magicCardFactory;
        }

        public void TraverseTree(string root, List<MagicCard> listOfMagicCards)
        {
            var dirs = new Stack<string>(100);

            if (!Directory.Exists(root))
            {
                throw new ArgumentException();
            }
            dirs.Push(root);

            while (dirs.Count > 0)
            {
                var currentDir = dirs.Pop();
                string[] subDirs;
                try
                {
                    subDirs = Directory.GetDirectories(currentDir);
                }
                catch (UnauthorizedAccessException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                catch (DirectoryNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                string[] files;
                try
                {
                    files = Directory.GetFiles(currentDir);
                }

                catch (UnauthorizedAccessException e)
                {

                    Console.WriteLine(e.Message);
                    continue;
                }

                catch (DirectoryNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                foreach (var file in files)
                {
                    try
                    {
                        var tmpCard = CreateBaseCard(file);
                        listOfMagicCards.Add(tmpCard);
                    }
                    catch (FileNotFoundException e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                // Push the subdirectories onto the stack for traversal. 
                // This could also be done before handing the files. 
                foreach (var str in subDirs)
                    dirs.Push(str);
            }
        }

        private MagicCard CreateBaseCard(string file)
        {
            var fi = new FileInfo(file);
            //var tmpCard = _magicCardFactory.CreateMagicCard( fi.FullName );
            var tmpCard = new MagicCard();
            if (fi.Directory == null)
                return tmpCard;

            var dirName = fi.FullName.Split('\\');
            tmpCard.Set = dirName[dirName.Length - 2];
            var tmpCardName = _matchUntilDot.Match(dirName[dirName.Length - 1]);
            tmpCard.Name = tmpCardName.Value;
            tmpCard.PathOfCardImage = fi.FullName;
            return tmpCard;
        }

        public string GetVariableName<T>(Expression<Func<T>> expression)
        {
            var body = ((MemberExpression)expression.Body);
            return body.Member.Name;
        }
    }

}
