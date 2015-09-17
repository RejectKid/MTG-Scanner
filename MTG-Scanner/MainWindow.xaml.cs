﻿using DirectX.Capture;
using MahApps.Metro.Controls.Dialogs;
using MTG_Scanner.Models;
using MTG_Scanner.Utils.Impl;
using MTG_Scanner.VMs;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
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
        private Capture _capturer;
        private readonly Filters _cameraFilters;
        static readonly object Locker = new object();
        private List<MagicCard> _previousMagicCards;
        private readonly List<MagicCard> _currentListOfMagicCards = new List<MagicCard>();
        private Bitmap _cameraBitmap;
        private Bitmap _cameraBitmapLive;
        private readonly PictureBox _cam = new PictureBox();

        public MainWindow()
        {
            _cameraFilters = new Filters();
            InitializeComponent();
            KernelUtil.CreateKernel();
            _vm = new MainWindowViewModel();
            DataContext = _vm;

            LoadCamera();
        }

        private void LoadCamera()
        {
            _cameraBitmap = new Bitmap(640, 480);
            _capturer = new Capture(_cameraFilters.VideoInputDevices[0], _cameraFilters.AudioInputDevices[0]);
            var vc = _capturer.VideoCaps;
            _capturer.FrameSize = new System.Drawing.Size(640, 480);
            _capturer.PreviewWindow = _cam;
            _capturer.FrameEvent2 += CaptureDone;
            _capturer.GrapImg();
        }

        private void CaptureDone(Bitmap e)
        {
            lock (Locker)
            {
                _previousMagicCards = new List<MagicCard>(_currentListOfMagicCards);
                _currentListOfMagicCards.Clear();
                _cameraBitmap = e;
                _cameraBitmapLive = (Bitmap)_cameraBitmap.Clone();
                //detectQuads( _cameraBitmap );
                //matchCard();

                //image_output.Image = filteredBitmap;
                var tmpImge = _vm.ConvertBitMap(_cameraBitmap);
                tmpImge.Freeze();
                Dispatcher.BeginInvoke(new ThreadStart(() => _camImage.Source = tmpImge));
            }
        }



        private async void ComputePHashes_OnClick(object sender, RoutedEventArgs e)
        {
            var fileDialog = new FolderBrowserDialog();
            //fileDialog.ShowDialog();
            //if (fileDialog.SelectedPath != null)
            //_vm.ComputePHashes(fileDialog.SelectedPath);
            var dialogController = await this.ShowProgressAsync("pHasing images... ", "Sit back a little while as I do my magic");
            var pathSaved = await Task.Run(() => _vm.ComputePHashes(@"..\..\\Resources\Card Images", dialogController));

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
    }
}
