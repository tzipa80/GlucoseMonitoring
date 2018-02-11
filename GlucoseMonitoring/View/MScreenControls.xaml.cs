using GraphLib;
using System;
using System.Windows.Controls;
using System.Windows.Forms.Integration;

namespace GlucoseMonitoring.View
{
    /// <summary>
    /// Interaction logic for MScreenControls.xaml
    /// </summary>
    public partial class MScreenControls : UserControl
    {
        //private Image _PlotterImage;
        //private delegate 
        GraphLib.PlotterDisplayEx display = null;
        private PrecisionTimer.Timer mTimer = null;
        private DateTime lastTimerTick = DateTime.Now;

        public MScreenControls()
        {
            InitializeComponent();
            //_PlotterImage = PlotterImage;

            display = new GraphLib.PlotterDisplayEx();
            display.Smoothing = System.Drawing.Drawing2D.SmoothingMode.None;
            // 
            // display
            // 
            display.BackColor = System.Drawing.Color.White;
            display.BackgroundColorBot = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            display.BackgroundColorTop = System.Drawing.Color.Navy;
            display.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            display.DashedGridColor = System.Drawing.Color.Blue;
            display.Dock = System.Windows.Forms.DockStyle.Fill;
            display.DoubleBuffering = true;
            display.Location = new System.Drawing.Point(0, 24);
            display.Name = "display";
            display.PlaySpeed = 0.5F;
            display.Size = new System.Drawing.Size(1142, 491);
            display.SolidGridColor = System.Drawing.Color.Blue;
            display.TabIndex = 1;

            //display.Refresh();

            display.DataSources.Clear();
            display.SetDisplayRangeX(0, 400);

            display.DataSources.Add(new DataSource());
            display.DataSources[0].Name = "Glucose Monitoring";
            display.DataSources[0].OnRenderXAxisLabel += RenderXLabel;

            display.DataSources[0].Length = 5800;
            display.PanelLayout = PlotterGraphPaneEx.LayoutMode.NORMAL;
            display.DataSources[0].AutoScaleY = true;
            display.DataSources[0].SetDisplayRangeY(-300, 300);
            display.DataSources[0].SetGridDistanceY(100);
            display.DataSources[0].OnRenderYAxisLabel = RenderYLabel;

            //----------------------------------------------
            WindowsFormsHost host1 = new WindowsFormsHost();
            host1.Child = display;
                    //Grid.SetRow(host1, 0);
                    //Grid.SetColumn(host1, 0);
            GridControls.Children.Add(host1);

            mTimer = new PrecisionTimer.Timer();
            mTimer.Period = 40;                         // 20 fps
            mTimer.Tick += new EventHandler(OnTimerTick);
            lastTimerTick = DateTime.Now;
            mTimer.Start();
        }

        private string RenderYLabel(DataSource src, float value)
        {
            return String.Format("{0:0.0}", value);
        }

        private string RenderXLabel(DataSource src, int idx)
        {
            if (src.AutoScaleX)
            {
                //if (idx % 2 == 0)
                {
                    int Value = (int)(src.Samples[idx].x);
                    return "" + Value;
                }
                //return "";
            }
            else
            {
                int Value = (int)(src.Samples[idx].x / 200);
                String Label = "" + Value + "\"";
                return Label;
            }
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            try
            {
                TimeSpan dt = DateTime.Now - lastTimerTick;

                cPoint[] src = display.DataSources[0].Samples;

                for (int i = 0; i < src.Length; i++)
                {
                    src[i].x = i;
                    src[i].y = 100;
                    /**
                                (float)( 4* Math.Sin( ((time + (i+8) * 100) / 900.0)))+
                                (float)(28 * Math.Sin(((time + (i + 8) * 100) / 290.0))); */
                }
                /*for (int j = 0; j < NumGraphs; j++)
                {

                    //CalcSinusFunction_3(display.DataSources[j], j, (float)dt.TotalMilliseconds);

                }*/
               // RefreshGraph();
                //TimerEventHandeler.Invoke(new MethodInvoker(RefreshGraph));

            }
            catch (ObjectDisposedException ex)
            {
                // we get this on closing of form
            }
            catch (Exception ex)
            {
                Console.Write("exception invoking refreshgraph(): " + ex.Message);
            }
        }

        private void RefreshGraph()
        {
            display.Refresh();
        }

        private void GridControls_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            mTimer.Stop();
            mTimer.Dispose();
        }
    }
}
