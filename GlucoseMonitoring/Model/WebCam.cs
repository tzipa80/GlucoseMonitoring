using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.ComponentModel;
using System.ComponentModel.Design;
using WebCam_Capture;
using System.Windows.Media.Imaging;

namespace GlucoseMonitoring.Model
{
    public class WebCam
    {
        //public event Action
        //Predicate<Image> 
        private WebCamCapture webcam;
        private Image _FrameImage;               //System.Windows.Controls.
        private int FrameNumber = 500;

        public event EventHandler<WebcamEventArgs> ToCrap;

        public void InitializeWebCam(ref Image ImageControl)     //System.Windows.Controls.
        {
            webcam = new WebCamCapture();
            webcam.FrameNumber = ((ulong)(0ul));
            webcam.CaptureHeight = 600;
            webcam.CaptureWidth = 800;
            webcam.TimeToCapture_milliseconds = FrameNumber;
            webcam.ImageCaptured += new WebCamCapture.WebCamEventHandler(webcam_ImageCaptured);
            _FrameImage = ImageControl;

        }

        void webcam_ImageCaptured(object source, WebcamEventArgs e)
        {
            //webcam.Stop();
            //_FrameImage.Source = Helper.DoGrayImage((System.Drawing.Bitmap)e.WebCamImage);
            _FrameImage.Source = Helper.LoadBitmap((System.Drawing.Bitmap)e.WebCamImage);
            //System.Threading.ThreadPool.QueueUserWorkItem((st) => ToCrap(this, e));
            ToCrap(this, e);
            //webcam.Start(this.webcam.FrameNumber);
        }

        public void Start()
        {
            webcam.TimeToCapture_milliseconds = FrameNumber;
            webcam.Start(0);
        }

        public void Stop()
        {
            webcam.Stop();
        }

        public void Continue()
        {
            // change the capture time frame
            webcam.TimeToCapture_milliseconds = FrameNumber;

            // resume the video capture from the stop
            webcam.Start(this.webcam.FrameNumber);
        }

        public void ResolutionSetting()
        {
            webcam.Config();
        }

        public void AdvanceSetting()
        {
            webcam.Config2();
        }

    }
}

