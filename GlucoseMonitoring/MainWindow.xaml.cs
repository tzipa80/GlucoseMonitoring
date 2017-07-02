﻿using System;
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

        WebCam webcam;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            webcam.Start();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // TODO: Add event handler implementation here.
            webcam = new WebCam();
            webcam.InitializeWebCam(ref imgVideo);
        }
    }
}
