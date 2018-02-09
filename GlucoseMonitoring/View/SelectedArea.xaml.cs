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
    /// Interaction logic for SelectedArea.xaml
    /// </summary>
    public partial class SelectedArea : UserControl
    {// The part of the rectangle the mouse is over.
        private enum HitType
        {
            None, Body, UL, UR, LR, LL,T,B,R,L
        };

        // The part of the rectangle under the mouse.
        HitType MouseHitType = HitType.None;
        Rectangle WorkingRect = new Rectangle();


        // True if a drag is in progress.
        private bool DragInProgress = false;

        // The drag's last point.
        private Point LastPoint;


        public SelectedArea()
        {
            InitializeComponent();
            SetMouseCursor();
           // rect = new Re
        }

        // Return a HitType value to indicate what is at the point.
        private HitType SetHitType(Rectangle rect, Point point)
        {
            double left = Canvas.GetLeft(rect);
            double top = Canvas.GetTop(rect);
            double right = left + rect.Width;
            double bottom = top + rect.Height;
            if (point.X < left) return HitType.None;
            if (point.X > right) return HitType.None;
            if (point.Y < top) return HitType.None;
            if (point.Y > bottom) return HitType.None;

            const double GAP = 10;
            if (point.X - left < GAP)
            {
                // Left edge.
                if (point.Y - top < GAP) return HitType.UL;
                if (bottom - point.Y < GAP) return HitType.LL;
                return HitType.L;
            }
            else if (right - point.X < GAP)
            {
                // Right edge.
                if (point.Y - top < GAP) return HitType.UR;
                if (bottom - point.Y < GAP) return HitType.LR;
                return HitType.R;
            }
            if (point.Y - top < GAP) return HitType.T;
            if (bottom - point.Y < GAP) return HitType.B;
            return HitType.Body;
        }

        // Set a mouse cursor appropriate for the current hit type.
        private void SetMouseCursor()
        {
            // See what cursor we should display.
            Cursor desired_cursor = Cursors.Arrow;
            switch (MouseHitType)
            {
                case HitType.None:
                    desired_cursor = Cursors.Arrow;
                    break;
                case HitType.Body:
                    desired_cursor = Cursors.ScrollAll;
                    break;
                case HitType.UL:
                case HitType.LR:
                    desired_cursor = Cursors.SizeNWSE;
                    break;
                case HitType.LL:
                case HitType.UR:
                    desired_cursor = Cursors.SizeNESW;
                    break;
                case HitType.T:
                case HitType.B:
                    desired_cursor = Cursors.SizeNS;
                    break;
                case HitType.L:
                case HitType.R:
                    desired_cursor = Cursors.SizeWE;
                    break;
            }

            // Display the desired cursor.
            if (Cursor != desired_cursor) Cursor = desired_cursor;
        }

        private void SACanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MouseHitType = SetHitType(SARec, Mouse.GetPosition(SACanvas));
            if (MouseHitType != HitType.None)
                WorkingRect = SARec;
            else
            {
                MouseHitType = SetHitType(SACer, Mouse.GetPosition(SACanvas));
                if (MouseHitType != HitType.None)
                    WorkingRect = SACer;
            }
            SetMouseCursor();
            if (MouseHitType == HitType.None) return;

            LastPoint = Mouse.GetPosition(SACanvas);
            DragInProgress = true;
        }

        private void SACanvas_MouseMove(object sender, MouseEventArgs e)
        {
            //pakaz.Text = string.Format("{0} ; {1}", e.GetPosition(this).X.ToString("#.##"), e.GetPosition(this).Y.ToString("#.##"));

            if (DragInProgress)
            {
                // See how much the mouse has moved.
                Point point = Mouse.GetPosition(SACanvas);
                //pakaz.Text = string.Format("{0} ; {1}", point.X.ToString("#"), point.Y.ToString("#"));
                double offset_x = point.X - LastPoint.X;
                double offset_y = point.Y - LastPoint.Y;

                // Get the rectangle's current position.
                double new_x = Canvas.GetLeft(WorkingRect);
                double new_y = Canvas.GetTop(WorkingRect);
                double new_width = WorkingRect.Width;
                double new_height = WorkingRect.Height;
                pakaz.Text = string.Format("{0} ; {1}", new_width.ToString("#"), new_height.ToString("#"));

                // Update the rectangle.
                switch (MouseHitType)
                {
                    case HitType.Body:
                        new_x += offset_x;
                        new_y += offset_y;
                        break;
                    case HitType.UL:
                        new_x += offset_x;
                        new_y += offset_y;
                        new_width -= offset_x;
                        new_height -= offset_y;
                        break;
                    case HitType.UR:
                        new_y += offset_y;
                        new_width += offset_x;
                        new_height -= offset_y;
                        break;
                    case HitType.LR:
                        new_width += offset_x;
                        new_height += offset_y;
                        break;
                    case HitType.LL:
                        new_x += offset_x;
                        new_width -= offset_x;
                        new_height += offset_y;
                        break;
                    case HitType.L:
                        new_x += offset_x;
                        new_width -= offset_x;
                        break;
                    case HitType.R:
                        new_width += offset_x;
                        break;
                    case HitType.B:
                        new_height += offset_y;
                        break;
                    case HitType.T:
                        new_y += offset_y;
                        new_height -= offset_y;
                        break;
                }

                // Don't use negative width or height.
                if ((new_width > 0) && (new_height > 0))
                {
                    // Update the rectangle.
                    Canvas.SetLeft(WorkingRect, new_x);
                    Canvas.SetTop(WorkingRect, new_y);
                    WorkingRect.Width = new_width;
                    WorkingRect.Height = new_height;

                    // Save the mouse's new location.
                    LastPoint = point;
                }
            }
           else
            {
                MouseHitType = SetHitType(WorkingRect, Mouse.GetPosition(SACanvas));
                SetMouseCursor();
            }

        }

        private void SACanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            DragInProgress = false;
        }
    }
}
