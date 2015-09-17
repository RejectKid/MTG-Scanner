using MahApps.Metro;
using MahApps.Metro.Controls.Dialogs;
using MTG_Scanner.Models;
using MTG_Scanner.Theme;
using MTG_Scanner.Utils;
using MTG_Scanner.Utils.Impl;
using Ninject;
using System.Collections.Generic;
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
        [DllImport(@"\Extern DLLs\pHash.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ph_hamming_distance(ulong hasha, ulong hashb);

        private readonly IUtil _util;
        private XmlFileLoader _xmlFileLoader;
        private readonly IXmlFileCreator _xmlFileCreator;
        public List<AccentColorMenuData> AccentColors { get; set; }
        public List<AppThemeMenuData> AppThemes { get; set; }
        public List<MagicCard> ListOfMagicCards { get; set; } = new List<MagicCard>();
        private const string XmlDbPath = @"H:\Compy Sci\MTG-Scanner\MTG-Scanner\Resources\Card Db\StandardDB.xml";

        public MainWindowViewModel()
        {
            GenerateThemeData();
            _util = KernelUtil.Kernel.Get<IUtil>();
            _xmlFileCreator = KernelUtil.Kernel.Get<IXmlFileCreator>();
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
    }
}
