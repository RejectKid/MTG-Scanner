using MahApps.Metro;
using MahApps.Metro.Controls.Dialogs;
using MTG_Scanner.Models;
using MTG_Scanner.Theme;
using MTG_Scanner.Utils;
using MTG_Scanner.Utils.Impl;
using Ninject;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Brush = System.Windows.Media.Brush;

namespace MTG_Scanner.VMs
{
    public class MainWindowViewModel
    {
        [DllImport(@"\Extern DLLs\pHash.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ph_dct_imagehash(string file, ref ulong hash);
        //[DllImport(@"\Extern DLLs\pHash.dll", CallingConvention = CallingConvention.Cdecl)]
        //public static extern int ph_hamming_distance(ulong hasha, ulong hashb);

        private readonly IUtil _util;
        private XmlFileLoader _xmlFileLoader;
        private readonly IXmlFileCreator _xmlFileCreator;
        private int intCounter;
        public IWebcamController WebcamController { get; set; }
        public List<AccentColorMenuData> AccentColors { get; set; }
        public List<AppThemeMenuData> AppThemes { get; set; }
        public List<MagicCard> ListOfMagicCards { get; set; } = new List<MagicCard>();
        private const string XmlDbPath = @"H:\Compy Sci\MTG-Scanner\MTG-Scanner\Resources\Card Db\StandardDB.xml";

        public MainWindowViewModel()
        {
            GenerateThemeData();
            _util = KernelUtil.Kernel.Get<IUtil>();
            _xmlFileCreator = KernelUtil.Kernel.Get<IXmlFileCreator>();
            WebcamController = KernelUtil.Kernel.Get<IWebcamController>();
            _xmlFileLoader = new XmlFileLoader();
        }

        private void GenerateThemeData()
        {
            // create accent color menu items for the demo
            AccentColors = ThemeManager.Accents
                                            .Select(a => new AccentColorMenuData
                                            {
                                                Name = a.Name,
                                                ColorBrush = a.Resources["AccentColorBrush"] as Brush
                                            })
                                            .ToList();

            // create metro theme color menu items for the demo
            AppThemes = ThemeManager.AppThemes
                                           .Select(a => new AppThemeMenuData
                                           {
                                               Name = a.Name,
                                               BorderColorBrush = a.Resources["BlackColorBrush"] as Brush,
                                               ColorBrush = a.Resources["WhiteColorBrush"] as Brush
                                           })
                                           .ToList();

        }

        public string ComputePHashes(string selectedPath, ProgressDialogController dialogController)
        {
            ListOfMagicCards.Clear();
            //read images in resources
            _util.TraverseTree(selectedPath, ListOfMagicCards);
            //compute hashes
            var curCard = 0;
            var totalCards = ListOfMagicCards.Count();
            var hashingLoop = Parallel.ForEach(ListOfMagicCards, card =>
            {
                ulong hash = 0;
                ph_dct_imagehash(card.PathOfCardImage, ref hash);
                card.PHash = hash;
                curCard++;
                dialogController.SetProgress((double)curCard / totalCards);
                dialogController.SetMessage("pHashing real hard! Finished: " + curCard + "/" + totalCards);
            });

            //Create Basic XML DB of pHashes
            var path = _xmlFileCreator.CreateXmlDb(ListOfMagicCards);
            return path;
        }

        /// <summary>
        /// Converts a Bitmap image to an image source and freezes the img
        /// </summary>
        /// <param name="cameraBitmap"></param>
        /// <returns></returns>
        public ImageSource ConvertBitMap(Image cameraBitmap)
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

        public void ComparePHash(MagicCard card)
        {
            intCounter++;
            var tmpPath = Path.GetTempPath();
            card.CardBitmap.Save(tmpPath + "tmpCard.bmp", ImageFormat.Bmp);
            card.PathOfCardImage = tmpPath + "tmpCard.bmp";
            //compute Phash for card
            card.PHash = ComputePHash(card);
            //compare on each card
            var delta = ComparePHashes(card);
            if (delta > 87)
                Debug.WriteLine("Success -> " + delta);
            else
            {
                Debug.WriteLine("FAIL! -> " + delta);

            }
        }

        private static ulong ComparePHashes(MagicCard card)
        {
            var x = card.PHash ^ 4571825439342088429;//Call of the full moon
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

        private ulong ComputePHash(MagicCard card)
        {
            ulong hash = 0;
            ph_dct_imagehash(card.PathOfCardImage, ref hash);
            return hash;
        }
    }
}
