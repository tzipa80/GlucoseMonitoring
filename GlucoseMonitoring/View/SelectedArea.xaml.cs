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

        // True if a drag is in progress.
        private bool DragInProgress = false;

        // The drag's last point.
        private Point LastPoint;
        public SelectedArea()
        {
            InitializeComponent();
            SetMouseCursor();
        }

        // Return a HitType value to indicate what is at the point.
        private HitType SetHitType(Rectangle rect, Point point)
        {
            double left = Canvas.GetLeft(SARec);
            double top = Canvas.GetTop(SARec);
            double right = left + SARec.Width;
            double bottom = top + SARec.Height;
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
            SetMouseCursor();
            if (MouseHitType == HitType.None) return;

            LastPoint = Mouse.GetPosition(SACanvas);
            DragInProgress = true;
        }

        private void SACanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (DragInProgress)
            {
                // See how much the mouse has moved.
                Point point = Mouse.GetPosition(SACanvas);
                double offset_x = point.X - LastPoint.X;
                double offset_y = point.Y - LastPoint.Y;

                // Get the rectangle's current position.
                double new_x = Canvas.GetLeft(SARec);
                double new_y = Canvas.GetTop(SARec);
                double new_width = SARec.Width;
                double new_height = SARec.Height;

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
                    Canvas.SetLeft(SARec, new_x);
                    Canvas.SetTop(SARec, new_y);
                    SARec.Width = new_width;
                    SARec.Height = new_height;

                    // Save the mouse's new location.
                    LastPoint = point;
                }
            }
           else
            {
                MouseHitType = SetHitType(SARec,
                    Mouse.GetPosition(SACanvas));
                SetMouseCursor();
            }

        }

        private void SACanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            DragInProgress = false;
        }
    }
}
