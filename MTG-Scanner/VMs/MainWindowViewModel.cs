using MahApps.Metro;
using MahApps.Metro.Controls.Dialogs;
using MTG_Scanner.Models;
using MTG_Scanner.Theme;
using MTG_Scanner.Utils;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using Brush = System.Windows.Media.Brush;

namespace MTG_Scanner.VMs
{
    public class MainWindowViewModel
    {
        public IUtil Util { get; set; }
        public IXmlFileCreator XmlFileCreator { get; set; }
        public ICardDatabase CardDatabase { get; set; }
        public IWebcamController WebcamController { get; set; }

        public List<AccentColorMenuData> AccentColors { get; set; }
        public List<AppThemeMenuData> AppThemes { get; set; }
        public List<MagicCard> ListOfMagicCards { get; set; } = new List<MagicCard>();

        public MainWindowViewModel(IUtil util,
            IXmlFileCreator xmlFileCreator,
            IWebcamController webcamController,
            ICardDatabase cardDatabase)
        {
            Util = util;
            XmlFileCreator = xmlFileCreator;
            WebcamController = webcamController;
            CardDatabase = cardDatabase;
            GenerateThemeData();
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
            Util.TraverseTree(selectedPath, ListOfMagicCards);
            //compute hashes
            var curCard = 0;
            var totalCards = ListOfMagicCards.Count;
            Parallel.ForEach(ListOfMagicCards, card =>
            {
                var hash = Util.ComputePHash(card);
                card.PHash = hash;
                curCard++;
                dialogController.SetProgress((double)curCard / totalCards);
                dialogController.SetMessage("pHashing real hard! Finished: " + curCard + "/" + totalCards);
            });

            //Create Basic XML DB of pHashes
            var path = XmlFileCreator.CreateXmlDb(ListOfMagicCards);
            return path;
        }

        /// <summary>
        /// Converts a Bitmap image to an image source and freezes the img
        /// </summary>
        /// <param name="cameraBitmap"></param>
        /// <returns></returns>
        public ImageSource ConvertBitMap(Image cameraBitmap)
        {
            return Util.ConvertBitmapInMemory(cameraBitmap);
        }

        public MagicCard ComparePHash(MagicCard card)
        {
            var tmpCard = Util.ComparePHash(card);
            return tmpCard;
        }
    }
}
