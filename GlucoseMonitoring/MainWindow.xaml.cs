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
using MahApps.Metro.Controls;
using GlucoseMonitoring.Model;
using MahApps.Metro;
using GlucoseMonitoring.View;
using System.Drawing;

namespace GlucoseMonitoring
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //Graphics.
        //WebCam MWwebcam;
       // System.Runtime.InteropServices.Marshal.Copy(

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MW_MSC.MWwebcam.Start();
           // var theme = ThemeManager.DetectAppStyle(Application.Current);
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // TODO: Add event handler implementation here.
           // MWwebcam = new WebCam();
          //  MWwebcam.InitializeWebCam(ref imgVideo);
        }

        private void Button_Click_Save(object sender, RoutedEventArgs e)
        {
            //MW_MSC.MWwebcam.ResolutionSetting();
            //MW_MSC.MWwebcam.AdvanceSetting();
            Helper.SaveImageCapture((BitmapSource)MW_MSC.MSSelectedArea.imgVideo.Source);
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            //MW_MSC.MWwebcam.ResolutionSetting();
            MW_MSC.MWwebcam.AdvanceSetting();
            //Helper.SaveImageCapture((BitmapSource)MW_MSC.MSSelectedArea.imgVideo.Source);
        }

        private void MW_MSC_Loaded(object sender, RoutedEventArgs e)
        {

        }

        /*
                private MetroWindow testWindow;

                private MetroWindow GetTestWindow()
                {
                    if (testWindow != null)
                    {
                        testWindow.Close();
                    }
                    testWindow = new MetroWindow() { Owner = this, WindowStartupLocation = WindowStartupLocation.CenterOwner, Title = "Another Test...", Width = 500, Height = 300 };
                    testWindow.Closed += (o, args) => testWindow = null;
                    return testWindow;
                }

                private void MenuWindowWithoutBorderOnClick(object sender, RoutedEventArgs e)
                {
                    var w = this.GetTestWindow();
                    w.Content = new TextBlock() { Text = "MetroWindow without Border", FontSize = 28, FontWeight = FontWeights.Light, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center };
                    //w.BorderThickness = new Thickness(1);
                    w.Show();
                }

                 private void MenuWindowWithBorderOnClick(object sender, RoutedEventArgs e)
                {
                    var w = this.GetTestWindow();
                    w.Content = new TextBlock() { Text = "MetroWindow with Border", FontSize = 28, FontWeight = FontWeights.Light, VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center };
                    w.BorderThickness = new Thickness(1);
                    w.GlowBrush = null;
                    w.SetResourceReference(MetroWindow.BorderBrushProperty, "AccentColorBrush");
                    w.Show();
                }
                */

    }
}
