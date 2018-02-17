using GraphLib;
using System;
using System.ComponentModel;
using System.Threading;
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
        public PrecisionTimer.Timer mTimer = null;
        private DateTime lastTimerTick = DateTime.Now;
        // BackgroundWorker _backgroundWorker = new BackgroundWorker();

        //private TextBox _resultControls;
        private string _resultStr = String.Empty;
        public string Result { set { _resultStr = value; } }

        private int iter = 0;

        public MScreenControls()
        {
            InitializeComponent();


            //_resultControls = GlucoseMonitoring.View.MSScreenResults.MSScreenResults.Result;
            // Set up the Background Worker Events
            //_backgroundWorker.DoWork += _backgroundWorker_DoWork;
            //_backgroundWorker.RunWorkerCompleted +=
            //    _backgroundWorker_RunWorkerCompleted;

            //_PlotterImage = PlotterImage;
            //SuspendLayout();
            CreatePlotterGraph();

            //display.Refresh();

            //_backgroundWorker.RunWorkerAsync(5000);
            //----------------------------------------------
            WindowsFormsHost host1 = new WindowsFormsHost();
            host1.Child = display;
           // host1.Width = 560;
           // host1.Height = 200;
                    //Grid.SetRow(host1, 0);
                    //Grid.SetColumn(host1, 0);
            GridControls.Children.Add(host1);


            // Run the Background Worker
            


            // host1.LayoutUpdated += Host1_LayoutUpdated;

            mTimer = new PrecisionTimer.Timer();
            mTimer.Period = 50;                         // 20 fps
            mTimer.Tick += new EventHandler(OnTimerTick);
            lastTimerTick = DateTime.Now;
            //mTimer.Start();
        }

        private void _backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                //statusText.Text = "Cancelled";
            }
            else if (e.Error != null)
            {
               // statusText.Text = "Exception Thrown";
            }
            else
            {
                //statusText.Text = "Completed";
            }
        }

        private void _backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //CreatePlotterGraph();

           // display.Refresh();
        }

        private void CreatePlotterGraph()
        {
            using (display = new GraphLib.PlotterDisplayEx())
            {
                display = new GraphLib.PlotterDisplayEx();
                display.Smoothing = System.Drawing.Drawing2D.SmoothingMode.None; //System.Drawing.Drawing2D.SmoothingMode.HighQuality; System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
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
                display.Size = new System.Drawing.Size(560, 200);
                display.SolidGridColor = System.Drawing.Color.Blue;
                display.TabIndex = 1;

                //display.Refresh();

                display.DataSources.Clear();
                display.SetDisplayRangeX(0, 600);

                display.DataSources.Add(new DataSource());
                display.DataSources[0].Name = "Glucose Monitoring";
                display.DataSources[0].OnRenderXAxisLabel += RenderXLabel;

                display.DataSources[0].Length = 600;
                display.PanelLayout = PlotterGraphPaneEx.LayoutMode.NORMAL;
                display.DataSources[0].AutoScaleY = true;
                display.DataSources[0].AutoScaleX = true;
                display.DataSources[0].SetDisplayRangeY(-200, 200);
                display.DataSources[0].SetGridDistanceY(100);
                display.DataSources[0].OnRenderYAxisLabel = RenderYLabel;


                //display.DataSources[0].XAutoScaleOffset = 50;

                //display.DataSources[0].CurGraphWidth = 560;
                //display.DataSources[0].CurGraphHeight = 200;

                display.DataSources[0].GraphColor = System.Drawing.Color.Black;
                display.BackgroundColorTop = System.Drawing.Color.White;
                display.BackgroundColorBot = System.Drawing.Color.LightGray;
                display.SolidGridColor = System.Drawing.Color.LightGray;
                display.DashedGridColor = System.Drawing.Color.LightGray;

                cPoint[] src = display.DataSources[0].Samples;

                for (int i = 0; i < src.Length; i++)
                {
                    src[i].x = i;
                    src[i].y = 0; // (float)(((float)200 * Math.Sin((3 + 1) * (i + 1.0) * 48 / src.Length)));
                    /**
                                (float)( 4* Math.Sin( ((time + (i+8) * 100) / 900.0)))+
                                (float)(28 * Math.Sin(((time + (i + 8) * 100) / 290.0))); */
                }
            }
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
                //TimeSpan dt = DateTime.Now - lastTimerTick;

                //display.DataSources[0].Length = iter;




                if (iter < display.DataSources[0].Length)
                {
                    cPoint[] src = display.DataSources[0].Samples;

                    src[iter].x = iter;
                    src[iter].y = float.Parse(_resultStr);
                }
                else
                {
                    //src[400].y = float.Parse(_resultStr);
                    ShiftLeft(display.DataSources[0]);
                    cPoint[] src = display.DataSources[0].Samples;

                    src[display.DataSources[0].Length-1].x = display.DataSources[0].Length-1;
                    src[display.DataSources[0].Length-1].y = float.Parse(_resultStr);

                }


                iter++;
                // for (int i = 0; i < iter; i++)
                //  {
                //     src[i].x = i;
                //     src[i].y = float.Parse(_resultStr);// (float)(((float)200 * Math.Sin((3 + 1) * (i + 1.0) * 48 / src.Length))); 
                /**
                            (float)( 4* Math.Sin( ((time + (i+8) * 100) / 900.0)))+
                            (float)(28 * Math.Sin(((time + (i + 8) * 100) / 290.0))); */
                // }
                /*for (int j = 0; j < NumGraphs; j++)
                {

                    //CalcSinusFunction_3(display.DataSources[j], j, (float)dt.TotalMilliseconds);

                }*/
                // RefreshGraph();
                //TimerEventHandeler.Invoke(new MethodInvoker(RefreshGraph));
                //System.Threading.ThreadPool.QueueUserWorkItem((st) => RefreshGraph());
                //Thread curr = new Thread(RefreshGraph); /* In a class, myfunction is a void taking an object */
                //curr.Start(new object()); /* Or something else to be downcast */
                Dispatcher.Invoke(RefreshGraph);


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

        private void ShiftLeft(DataSource dataSource)
        {
            for (int i = 1;i < display.DataSources[0].Length; i++ )
            {
                cPoint[] src = display.DataSources[0].Samples;
                //cPoint[] tmp = new cPoint[display.DataSources[0].Length];
                //display.DataSources[0].Cle
                src[i - 1].x = i-1;
                src[i - 1].y = src[i].y;
                //display.DataSources.Clear();
                //CreatePlotterGraph();
                //display.DataSources[0].Samples = src;

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
