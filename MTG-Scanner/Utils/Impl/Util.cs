using MTG_Scanner.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MTG_Scanner.Utils.Impl
{
    public class Util : IUtil
    {
        private readonly ICardDatabase _cardDatabase;

        [DllImport(@"\Extern DLLs\pHash.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int ph_dct_imagehash(string file, ref ulong hash);

        public Util(ICardDatabase cardDatabase)
        {
            _cardDatabase = cardDatabase;
        }


        private readonly Regex _matchUntilDot = new Regex(@"^([^.]*)");
        private readonly Queue<MagicCard> _compareList = new Queue<MagicCard>();

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

        public MagicCard ComparePHash(MagicCard card)
        {
            var tmpPath = Path.GetTempPath();
            card.CardBitmap.Save(tmpPath + "tmpCard.bmp", ImageFormat.Bmp);
            card.PathOfCardImage = tmpPath + "tmpCard.bmp";
            //compute Phash for card
            card.PHashes.Add(ComputePHash(card));
            //compare on each card
            var tmpList = new List<MagicCard>();
            foreach (var dbCard in _cardDatabase.ListOfAllMagicCards)
            {
                ulong delta = 0;
                foreach (var pHash in dbCard.PHashes)
                {
                    var tmpdelta = ComparePHashes(card.PHashes.FirstOrDefault(), pHash);
                    if (tmpdelta > delta)
                        delta = tmpdelta;
                }

                if (delta < 90)
                    continue;

                dbCard.DeltaMatch = delta;
                tmpList.Add(dbCard);
            }
            var winner = tmpList.OrderByDescending(o => o.DeltaMatch).FirstOrDefault();
            if (winner != null)
                _compareList.Enqueue(winner);

            var realWinner = CalculateWinner();

            if (winner != null)
                Debug.WriteLine("Best Match --> " + winner + "--> " + winner.DeltaMatch);


            return realWinner ?? winner;
        }

        private MagicCard CalculateWinner()
        {
            if (_compareList.Count <= 40)
                return null;

            var countWinners = (from cards in _compareList
                                group cards by cards.Name into c
                                select new
                                {
                                    Name = c.Key,
                                    HashTotal = c.Sum(o => (int)o.DeltaMatch),
                                    Count = c.Count(),
                                    WeightedPower = c.Sum(o => (int)o.DeltaMatch) * c.Count()
                                }).ToList();
            var realWinner = countWinners.OrderByDescending(o => o.WeightedPower).FirstOrDefault();
            _compareList.Dequeue();
            return _compareList.FirstOrDefault(o => o.Name == realWinner?.Name);
        }

        public ulong ComputePHash(MagicCard card)
        {
            ulong hash = 0;
            ph_dct_imagehash(card.PathOfCardImage, ref hash);
            return hash;
        }

        private static ulong ComparePHashes(ulong cardPHash, ulong dbCardPHash)
        {
            var x = cardPHash ^ dbCardPHash;
            const ulong m1 = 0x5555555555555555UL;
            const ulong m2 = 0x3333333333333333UL;
            const ulong h01 = 0x0101010101010101UL;
            const ulong m4 = 0x0f0f0f0f0f0f0f0fUL;

            x -= (x >> 1) & m1;
            x = (x & m2) + ((x >> 2) & m2);
            x = (x + (x >> 4)) & m4;
            var returnMe = (x * h01) >> 56;
            return 100 - returnMe;

        }

        /// <summary>
        /// Converts a Bitmap image to an image source and freezes the img
        /// </summary>
        /// <param name="cameraBitmap"></param>
        /// <returns></returns>
        public ImageSource ConvertBitmapInMemory(Image cameraBitmap)
        {
            using (var memStream = new MemoryStream())
            {
                cameraBitmap.Save(memStream, ImageFormat.Bmp);
                memStream.Position = 0;
                var bmpImage = new BitmapImage();
                bmpImage.BeginInit();
                bmpImage.StreamSource = memStream;
                bmpImage.CacheOption = BitmapCacheOption.OnLoad;
                bmpImage.EndInit();
                bmpImage.Freeze();
                return bmpImage;
            }
        }
    }

}
