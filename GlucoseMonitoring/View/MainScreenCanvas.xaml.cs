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
//using System.Drawing;

namespace GlucoseMonitoring.View
{
    /// <summary>
    /// Interaction logic for MainScreenCanvas.xaml
    /// </summary>
    public partial class MainScreenCanvas : UserControl
    {
        private enum ImageTypeE
        {
            Top, Bottom, Diff
        };
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
            System.Windows.Shapes.Rectangle aa = MSSelectedArea.SARec;
            System.Windows.Shapes.Rectangle bb = MSSelectedArea.SACer;
            int x = (int)Canvas.GetLeft(aa);
            int y = (int)Canvas.GetTop(aa);
            Int32Rect RecTop = new Int32Rect(x, y, (int)aa.Width, (int)aa.Height);
            x = (int)Canvas.GetLeft(bb);
            y = (int)Canvas.GetTop(bb);
            Int32Rect RecBottom = new Int32Rect(x, y, (int)aa.Width, (int)aa.Height);

            _FrameImageCropTop.Source = Helper.DoGrayImage((System.Drawing.Bitmap)e.WebCamImage);
            _FrameImageCropBottom.Source = Helper.DoGrayImage((System.Drawing.Bitmap)e.WebCamImage);

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
            //BitmapFrame destination = BitmapFrame.Create((BitmapSource)_FrameImageCropDiff.Source);//new BitmapImage();

            int[] pixelDataDiff = null;
            int[] pixelDataTop = null;
            int[] pixelDataBottom = null;

            int? widthInByteDiff = null;
            int? widthInByteTop = null;
            int? widthInByteBottom = null;

            
            WriteableBitmap WBTopImage = _CreateWB(ImageTypeE.Top, ref pixelDataTop, ref widthInByteTop);
            WriteableBitmap WBBottomImage = _CreateWB(ImageTypeE.Bottom, ref pixelDataBottom, ref widthInByteBottom);
            WriteableBitmap WBDiffImage = _CreateWB(ImageTypeE.Diff, ref pixelDataDiff, ref widthInByteDiff);


            for (int i = 0; i < pixelDataDiff.Length; i++)
            {
                pixelDataDiff[i] ^= 0x00ffffff;
            }

            WBDiffImage.WritePixels(new Int32Rect(0, 0, (int)WBDiffImage.Width, (int)WBDiffImage.Height), (int[])pixelDataDiff, (int)widthInByteDiff, 0);

            _FrameImageCropDiff.Source = WBDiffImage;


     
        }

        private WriteableBitmap _CreateWB(ImageTypeE type, ref int[] pixelData, ref int? widthInByte)
        {
            WriteableBitmap mImage = null;
            switch (type)
            {
                case ImageTypeE.Diff:
                    mImage = new WriteableBitmap((BitmapSource)_FrameImageCropDiff.Source);
                    break;
                case ImageTypeE.Top:
                    mImage = new WriteableBitmap((BitmapSource)_FrameImageCropTop.Source);
                    break;
                case ImageTypeE.Bottom:
                    mImage = new WriteableBitmap((BitmapSource)_FrameImageCropBottom.Source);
                    break;
            }
           // WriteableBitmap mImage = new WriteableBitmap(source);

            int h = mImage.PixelHeight;
            int w = mImage.PixelWidth;
            pixelData = new int[w * h];
            int widthInByteInternal = 4 * w;

            mImage.CopyPixels(pixelData, widthInByteInternal, 0);

            widthInByte = widthInByteInternal;
            return mImage;
        }
    }
}
