using DirectX.Capture;
using MahApps.Metro.Controls.Dialogs;
using MTG_Scanner.Utils.Impl;
using MTG_Scanner.VMs;
using Ninject;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace MTG_Scanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly MainWindowViewModel _vm;
        private Capture _capturer;
        private readonly Filters _cameraFilters;
        static readonly object Locker = new object();
        private readonly PictureBox _cam = new PictureBox();

        public MainWindow()
        {
            KernelUtil.CreateKernel();
            _cameraFilters = new Filters();
            InitializeComponent();
            _vm = KernelUtil.Kernel.Get<MainWindowViewModel>();
            DataContext = _vm;

            LoadCamera();
            KeyDown += MainWindow_KeyDown;
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                AddCardToImportCardList();
        }



        private void LoadCamera()
        {
            _capturer = new Capture(_cameraFilters.VideoInputDevices[0], _cameraFilters.AudioInputDevices[0])
            {
                FrameSize = new System.Drawing.Size(640, 480),
                PreviewWindow = _cam
            };
            _capturer.FrameEvent2 += CaptureDone;
            _capturer.GrapImg();

        }

        private void CaptureDone(Bitmap e)
        {
            lock (Locker)
            {
                _vm.WebcamController.CameraBitmap = e;
                try
                {
                    _vm.WebcamController.DetectQuads();
                }
                catch (Exception)
                {
                    // ignored
                }


                var tmpImge = _vm.ConvertBitMap(_vm.WebcamController.FilteredBitmap);
                tmpImge.Freeze();
                Dispatcher.BeginInvoke(new ThreadStart(() => CamImageFiltered.Source = tmpImge));

                var tmpImge2 = _vm.ConvertBitMap(_vm.WebcamController.CameraBitmap);
                tmpImge2.Freeze();
                Dispatcher.BeginInvoke(new ThreadStart(() => CamImage.Source = tmpImge2));

                var tmpImge4 = _vm.ConvertBitMap(_vm.WebcamController.CardBitmap);
                tmpImge4.Freeze();
                Dispatcher.BeginInvoke(new ThreadStart(() => CamImageFullCard.Source = tmpImge4));

                Task.Run(() =>
                {
                    if (_vm.WebcamController.TmpCard == null)
                        return;

                    _vm.ComparePHash(_vm.WebcamController.TmpCard);
                });
            }
        }

        private async void ComputePHashes_OnClick(object sender, RoutedEventArgs e)
        {
            var fileDialog = new FolderBrowserDialog();
            //fileDialog.ShowDialog();
            //if (fileDialog.SelectedPath != null)
            //_vm.ComputePHashes(fileDialog.SelectedPath);
            var dialogController = await this.ShowProgressAsync("pHasing images... ", "Sit back a little while as I do my magic");
            await Task.Run(() => _vm.ComputePHashes(@"..\..\\Resources\Card Images", dialogController));
            var pathSaved = await Task.Run(() => _vm.SavePHashes(@"..\..\\Resources\Card Images", dialogController));

            await dialogController.CloseAsync();

            if (dialogController.IsCanceled)
            {
                await this.ShowMessageAsync("pHashes canceled", "pHash not guarenteed to be complete");
            }
            else
            {
                await this.ShowMessageAsync("pHashing is complete", "XML file has been saved to " + pathSaved + "  folder");
            }
        }

        private void StartCardImportFile(object sender, RoutedEventArgs e)
        {
            _vm.CardImportFileCreator.CardImportStarted = true;
            FlipVisibilities();
        }

        private void FlipVisibilities()
        {
            if (NewCardImportButton.Visibility == Visibility.Collapsed)
            {
                NewCardImportButton.Visibility = Visibility.Visible;
                EndCardImportButton.Visibility = Visibility.Collapsed;
            }
            else if (EndCardImportButton.Visibility == Visibility.Collapsed)
            {
                NewCardImportButton.Visibility = Visibility.Collapsed;
                EndCardImportButton.Visibility = Visibility.Visible;
            }
        }

        private void AddCardToCardImportFile(object sender, RoutedEventArgs e)
        {
            AddCardToImportCardList();
        }

        private void AddCardToImportCardList()
        {
            if (_vm.CardImportFileCreator.CardImportStarted)
                _vm.AddFileToImportList();


        }

        private void EndCardImportFile(object sender, RoutedEventArgs e)
        {
            var fileDialog = new SaveFileDialog
            {
                Filter = @"Text files (*.txt)|*.txt",
                DefaultExt = "*.txt",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };
            ;
            if (fileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK || string.IsNullOrEmpty(fileDialog.FileName))
                return;

            _vm.CardImportFileCreator.FilePath = fileDialog.FileName;
            _vm.CardImportFileCreator.CreateCardListFile();
            FlipVisibilities();

        }
    }
}
