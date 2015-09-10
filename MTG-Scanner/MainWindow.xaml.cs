using MTG_Scanner.Utils.Impl;
using MTG_Scanner.VMs;
using System.Windows;
using System.Windows.Forms;


namespace MTG_Scanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly MainWindowViewModel _vm;

        public MainWindow()
        {
            InitializeComponent();
            KernelUtil.CreateKernel();
            _vm = new MainWindowViewModel();
            DataContext = _vm;
        }

        private void ComputePHashes_OnClick(object sender, RoutedEventArgs e)
        {
            var fileDialog = new FolderBrowserDialog();
            //fileDialog.ShowDialog();
            //if (fileDialog.SelectedPath != null)
            //_vm.ComputePHashes(fileDialog.SelectedPath);
            _vm.ComputePHashes(@"H:\Compy Sci\MTG-Scanner\MTG-Scanner\Resources\Card Images");
        }
    }
}
