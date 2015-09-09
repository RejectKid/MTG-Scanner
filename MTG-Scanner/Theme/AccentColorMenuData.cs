using MahApps.Metro;
using MTG_Scanner.Models;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace MTG_Scanner.Theme
{
    public class AccentColorMenuData
    {
        public string Name { get; set; }
        public Brush BorderColorBrush { get; set; }
        public Brush ColorBrush { get; set; }

        private ICommand _changeAccentCommand;

        public ICommand ChangeAccentCommand
        {
            get
            {
                return _changeAccentCommand ?? (_changeAccentCommand = new SimpleCommand
                {
                    CanExecuteDelegate = x => true,
                    ExecuteDelegate = x => DoChangeTheme(x)
                });
            }
        }

        protected virtual void DoChangeTheme(object sender)
        {
            var theme = ThemeManager.DetectAppStyle(Application.Current);
            var accent = ThemeManager.GetAccent(Name);
            ThemeManager.ChangeAppStyle(Application.Current, accent, theme.Item1);
        }
    }
}
