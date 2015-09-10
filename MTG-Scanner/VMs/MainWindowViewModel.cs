using MahApps.Metro;
using MTG_Scanner.Models;
using MTG_Scanner.Theme;
using MTG_Scanner.Utils;
using MTG_Scanner.Utils.Impl;
using Ninject;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Media;

namespace MTG_Scanner.VMs
{
    public class MainWindowViewModel
    {
        [DllImport(@"pHash.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ph_dct_imagehash(string file, ref ulong hash);
        [DllImport(@"pHash.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ph_hamming_distance(ulong hasha, ulong hashb);

        private readonly IUtil _until;
        public List<AccentColorMenuData> AccentColors { get; set; }
        public List<AppThemeMenuData> AppThemes { get; set; }
        public List<MagicCard> ListOfMagicCards { get; set; } = new List<MagicCard>();

        public MainWindowViewModel()
        {
            GenerateThemeData();
            _until = KernelUtil.Kernel.Get<IUtil>();
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

        public void ComputePHashes(string selectedPath)
        {
            _until.TraverseTree(selectedPath, ListOfMagicCards);
            ulong hash1 = 0;
            foreach (var magicCard in ListOfMagicCards)
            {

                ph_dct_imagehash(magicCard.PathOfCardImage, ref hash1);
            }

        }
    }
}
