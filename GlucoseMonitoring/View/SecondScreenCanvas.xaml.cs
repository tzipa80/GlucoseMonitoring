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

namespace GlucoseMonitoring.View
{
    /// <summary>
    /// Interaction logic for SecondScreenCanvas.xaml
    /// </summary>
    public partial class SecondScreenCanvas : UserControl
    {

        public static string mouse_pakaz = "11111111111111";

        public SecondScreenCanvas()
        {
            InitializeComponent();
            appe_text.Text = mouse_pakaz;
        }

    }
}
