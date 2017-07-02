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
    /// Interaction logic for SelactedArea.xaml
    /// </summary>
    public partial class SelactedArea : UserControl
    {// The part of the rectangle the mouse is over.
        private enum HitType
        {
            None, Body, UL, UR, LR, LL
        };

        // The part of the rectangle under the mouse.
        HitType MouseHitType = HitType.None;

        // True if a drag is in progress.
        private bool DragInProgress = false;

        // The drag's last point.
        private Point LastPoint;
        public SelactedArea()
        {
            InitializeComponent();
        }
    }
}
