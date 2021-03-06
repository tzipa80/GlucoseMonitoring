﻿using GlucoseMonitoring.Model;
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

        private TextBlock _pakaz;
        private TextBox _result;

        private int row;
        //private int col = 0;
        private DateTime DiffDate;
        private Microsoft.Office.Interop.Excel.Application DiffoXL = null;
        //private Microsoft.Office.Interop.Excel._Workbook DiffoWB = null;
        private Microsoft.Office.Interop.Excel._Worksheet DiffoSheet = null;

        public string ResultStr { get { return _result.Text;  } }

        public MainScreenCanvas()
        {
            InitializeComponent();
            MWwebcam = new WebCam();
            MWwebcam.InitializeWebCam(ref MSSelectedArea.imgVideo);
            MWwebcam.ToCrap += MWwebcam_ToCrap; //Helper.MWwebcam_ToCrap;

            _FrameImageCropTop = MSScreenResults.imgVideoResTop;
            _FrameImageCropBottom = MSScreenResults.imgVideoBottom;
            _FrameImageCropDiff = MSScreenResults.imgVideoDiff;
            _pakaz = MSSelectedArea.pakaz;//MSScreenResults.pakaz;
            _result = MSScreenResults.Result;
           // DiffDate = DateTime.Now;

           // row = 1;
          //  DiffoXL = new Microsoft.Office.Interop.Excel.Application();
            //DiffoWB = DiffoXL.Workbooks.Open(string.Format(@"{0}\{1:s}.xlsx", Environment.CurrentDirectory, DiffDate.ToString("MM-dd-HH-mm-ss")));
          //  DiffoXL.Workbooks.Add();
          //  DiffoSheet = DiffoXL.ActiveSheet;//String.IsNullOrEmpty("GlucoseMonitoring") ? (Microsoft.Office.Interop.Excel._Worksheet)DiffoWB.ActiveSheet : (Microsoft.Office.Interop.Excel._Worksheet)DiffoWB.Worksheets["GlucoseMonitoring"];
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
                MSScreenControls.mTimer.Start();
                if (DiffoSheet == null)
                {

                    DiffDate = DateTime.Now;
                    row = 1;
                    DiffoXL = new Microsoft.Office.Interop.Excel.Application();
                    DiffoXL.Workbooks.Add();
                    DiffoSheet = DiffoXL.ActiveSheet;

                }
            }
            else
            {
                MSScreenControls.mTimer.Stop();
                if (DiffoSheet != null)
                {
                    DiffoSheet.SaveAs(string.Format(@"{0}\diff_{1:s}.xlsx", Environment.CurrentDirectory, DiffDate.ToString("MM-dd-HH-mm-ss")));
                    DiffoXL.Quit();
                    DiffoSheet = null;
                }

                
            }
        }

        private void _UpdateDiffImage()
        {
            //BitmapFrame destination = BitmapFrame.Create((BitmapSource)_FrameImageCropDiff.Source);//new BitmapImage();

           // int[] pixelDataDiff = null;
            byte[] pixelDataTop = null;
            byte[] pixelDataBottom = null;
            double[] FloatPixelData = null;
            double SumTop = 0.0;
            double Bottom = 0.0;

            // int? widthInByteDiff = new int();
            int? widthInByteTop = new int();
            int? widthInByteBottom = new int();

            
            WriteableBitmap WBTopImage = _CreateWB(ImageTypeE.Top, ref pixelDataTop, ref widthInByteTop);
            WriteableBitmap WBBottomImage = _CreateWB(ImageTypeE.Bottom, ref pixelDataBottom, ref widthInByteBottom);
            // WriteableBitmap WBDiffImage = _CreateWB(ImageTypeE.Diff, ref pixelDataDiff, ref widthInByteDiff);

            //_pakaz.Text = string.Format("{0} ; {1}", WBTopImage.Format.BitsPerPixel, pixelDataTop.Length);
            FloatPixelData = new double[pixelDataTop.Length];
            //FloatPixelDataSumTop = new double[pixelDataTop.Length];
            //FloatPixelDataSumBottom = new double[pixelDataTop.Length];
            for (int i = 0; i < pixelDataTop.Length; i++)
            {
                //pixelDataTop[i] -= pixelDataBottom[i];

                SumTop += pixelDataTop[i];
                Bottom += pixelDataBottom[i];

                //byte tmpMax = Math.Max(pixelDataTop[i], pixelDataBottom[i]);
                //byte tmpMin = Math.Min(pixelDataTop[i], pixelDataBottom[i]);
                pixelDataTop[i] = (byte)(Math.Max(pixelDataTop[i], pixelDataBottom[i]) - Math.Min(pixelDataTop[i], pixelDataBottom[i]));
                
                //pixelDataTop[i] -= pixelDataBottom[i];


                FloatPixelData[i] = pixelDataTop[i];
            }

            //_pakaz.Text = string.Format("{0} ; {1}", SumTop, Bottom);
            if (DiffoSheet != null)
            {
                DiffoSheet.Cells[row, 1] = row;
                DiffoSheet.Cells[row, 2] = SumTop;
                DiffoSheet.Cells[row, 3] = Bottom;
                row++;
            }
            _pakaz.Text = string.Format("{0}", row);

            WBTopImage.WritePixels(new Int32Rect(0, 0, (int)WBTopImage.Width, (int)WBTopImage.Height), pixelDataTop, (int)widthInByteTop, 0);

            _FrameImageCropDiff.Source = WBTopImage;

            if ((bool)MSScreenControls.TypeCalculation.IsChecked)
            {
                double tmp = (Math.Max(SumTop, Bottom) - Math.Min(SumTop, Bottom));
                MSScreenControls.Result = tmp.ToString();
                _result.Text = tmp.ToString();
            }
            else
            {
                _calculateSTD(FloatPixelData);
            }
            //MSScreenControls.Result = (Math.Max(SumTop, Bottom) - Math.Min(SumTop, Bottom)).ToString();
            //_calculateSTD(FloatPixelData);
        }

        private void _calculateSTD(double[] floatPixelData)
        {
            double average = floatPixelData.Average();
            double sumOfSquaresOfDifferences = floatPixelData.Select(val => (val - average) * (val - average)).Sum();
            double sd = Math.Sqrt(sumOfSquaresOfDifferences / floatPixelData.Length);
            
            //Bug when clear all from FactorText it was exeption due to empty TextBox: now it's checked
            if (MSScreenControls.FactorText.GetLineLength(0) > 0)
            {
                sd *= double.Parse(MSScreenControls.FactorText.Text, System.Globalization.CultureInfo.InvariantCulture);
            } 

            _result.Text = sd.ToString("#");
            MSScreenControls.Result = _result.Text;
        }

        private WriteableBitmap _CreateWB(ImageTypeE type, ref byte[] pixelData, ref int? widthInByte)
        {
            WriteableBitmap mImage = null;
            switch (type)
            {
                case ImageTypeE.Diff:
                    //mImage = new WriteableBitmap((BitmapSource)_FrameImageCropTop.Source); //Here we take top image
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
            pixelData = new byte[w * h];
            int widthInByteInternal = w;

            mImage.CopyPixels(pixelData, widthInByteInternal, 0);

            widthInByte = widthInByteInternal;
            return mImage;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            //if (DiffoWB != null)
            //    DiffoWB.Close();
            if (DiffoXL != null)
                DiffoXL.Quit();
            //if (DiffoSheet != null)
            //    DiffoSheet.Delete();
        }
    }
}
