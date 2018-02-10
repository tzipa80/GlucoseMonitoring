using GlucoseMonitoring.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WebCam_Capture;


namespace GlucoseMonitoring.View
{
    /// <summary>
    /// Interaction logic for MainScreenCanvas.xaml
    /// </summary>
    public partial class MainScreenCanvas : UserControl
    {

        public WebCam MWwebcam { get; set; }

        //Block Memory Leak
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr handle);
        public static BitmapSource bs_crop;
        public static IntPtr ip_crop;

        private Image _FrameImageCropTop;
        private Image _FrameImageCropBottom;
        private Image _FrameImageCropDiff;

        public MainScreenCanvas()
        {
            InitializeComponent();
            MWwebcam = new WebCam();
            MWwebcam.InitializeWebCam(ref MSSelectedArea.imgVideo);
            MWwebcam.ToCrap += MWwebcam_ToCrap; //Helper.MWwebcam_ToCrap;

            _FrameImageCropTop = MSScreenResults.imgVideoResTop;
            _FrameImageCropBottom = MSScreenResults.imgVideoBottom;
            _FrameImageCropDiff = MSScreenResults.imgVideoDiff;
        }

        private CroppedBitmap _DoCropCP(BitmapSource bitmap, Int32Rect rec)
        {
            // Create a CroppedBitmap from BitmapImage  
            CroppedBitmap cb = new CroppedBitmap(bitmap,
               rec);

            return cb;
        }
        private void MWwebcam_ToCrap(object sender, WebcamEventArgs e)
        {
            Rectangle aa = MSSelectedArea.SARec;
            Rectangle bb = MSSelectedArea.SACer;
            int x = (int)Canvas.GetLeft(aa);
            int y = (int)Canvas.GetTop(aa);
            Int32Rect RecTop = new Int32Rect(x, y, (int)aa.Width, (int)aa.Height);
            x = (int)Canvas.GetLeft(bb);
            y = (int)Canvas.GetTop(bb);
            Int32Rect RecBottom = new Int32Rect(x, y, (int)aa.Width, (int)aa.Height);

            _FrameImageCropTop.Source = Helper.DoGrayImage((System.Drawing.Bitmap)e.WebCamImage);
            _FrameImageCropBottom.Source = Helper.DoGrayImage((System.Drawing.Bitmap)e.WebCamImage);

            // Create an Image TOP  
            //Image croppedImageTop = new Image();
            //croppedImage.Width = 200;
            //croppedImageTop.Margin = new Thickness(2);

            // Create an Image Bottom  
            // Image croppedImageBottom = new Image();
            //croppedImage.Width = 200;
            //croppedImageBottom.Margin = new Thickness(2);

            // Create a CroppedBitmap from BitmapImage
            //CroppedBitmap cb = new CroppedBitmap((BitmapSource)_FrameImageCrop.Source,
            //                                      RecTop);

            // Set Image.Source to cropped image
            _FrameImageCropTop.Source = _DoCropCP((BitmapSource)_FrameImageCropTop.Source,
                                           RecTop);
            _FrameImageCropBottom.Source = _DoCropCP((BitmapSource)_FrameImageCropBottom.Source,
                                           RecBottom);
            if ((bool)MSScreenResults.Calculate.IsChecked)
            {
                _UpdateDiffImage();
                
            }
        }

        private void _UpdateDiffImage()
        {
            // Create an Image TOP  
            //Image croppedImageDiff = new Image();
            //croppedImage.Width = 200;
            //croppedImageDiff.Margin = new Thickness(2);

        }
    }
}
