using MahApps.Metro;
using MTG_Scanner.Theme;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace MTG_Scanner.VMs
{
    public class MainWindowViewModel
    {
        public List<AccentColorMenuData> AccentColors { get; set; }
        public List<AppThemeMenuData> AppThemes { get; set; }

        public MainWindowViewModel()
        {
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
    }
}
