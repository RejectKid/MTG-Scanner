using MTG_Scanner.Models;
using System;
using System.Collections.Generic;

namespace MTG_Scanner.Utils.Impl
{
    public class Util : IUtil
    {
        private readonly IMagicCardFactory _magicCardFactory;

        public Util(IMagicCardFactory magicCardFactory)
        {
            _magicCardFactory = magicCardFactory;
        }

        public void TraverseTree(string root, List<MagicCard> listOfMagicCards)
        {
            var dirs = new Stack<string>(100);

            if (!System.IO.Directory.Exists(root))
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
                    subDirs = System.IO.Directory.GetDirectories(currentDir);
                }
                catch (UnauthorizedAccessException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                catch (System.IO.DirectoryNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                string[] files;
                try
                {
                    files = System.IO.Directory.GetFiles(currentDir);
                }

                catch (UnauthorizedAccessException e)
                {

                    Console.WriteLine(e.Message);
                    continue;
                }

                catch (System.IO.DirectoryNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }
                foreach (var file in files)
                {
                    try
                    {
                        // Perform whatever action is required in your scenario.
                        var fi = new System.IO.FileInfo(file);
                        var tmpCard = _magicCardFactory.CreateMagicCard(fi.FullName);
                        if (fi.Directory != null)
                        {
                            var dirName = fi.Directory.FullName.Split('\\');
                            tmpCard.SetNameShort = dirName[dirName.Length - 1];
                        }
                        listOfMagicCards.Add(tmpCard);
                    }
                    catch (System.IO.FileNotFoundException e)
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
    }

}
