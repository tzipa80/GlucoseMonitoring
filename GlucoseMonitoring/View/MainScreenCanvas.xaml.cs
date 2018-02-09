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

namespace GlucoseMonitoring.View
{
    /// <summary>
    /// Interaction logic for MainScreenCanvas.xaml
    /// </summary>
    public partial class MainScreenCanvas : UserControl
    {

        public WebCam MWwebcam { get; set; }

       
        public MainScreenCanvas()
        {
            InitializeComponent();
            MWwebcam = new WebCam();
            MWwebcam.InitializeWebCam(ref MSSelectedArea.imgVideo);
        }
    }
}
