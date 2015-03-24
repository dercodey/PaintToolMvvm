using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PaintToolCs
{
    /// <summary>
    /// represents an interaction source that manages a paint tool
    /// </summary>
    public class PaintToolInteractionSource : Canvas
    {
        public PaintToolInteractionSource()
        {
            // background is transparent to capture mouse events
            this.Background = Brushes.Transparent;

            // the current polygon represents (visually) the current state
            _currentSpot = new Path();
            _currentSpot.StrokeThickness = 2;
            _currentSpot.Stroke = Brushes.White;
            _currentSpot.Fill = Brushes.Gray;
            _currentSpot.Opacity = 0.4;

            _currentEllipse = new EllipseGeometry(new Point(), Diameter, Diameter);
            _currentSpot.Data = _currentEllipse;          

            this.Children.Add(_currentSpot);

            // and the mouse events
            MouseDown += PaintToolInteractionSource_MouseDown;
            MouseMove += PaintToolInteractionSource_MouseMove;
            MouseUp += PaintToolInteractionSource_MouseUp;
        }

        /// <summary>
        /// current geometry being painted
        /// </summary>
        public PathGeometry CurrentPathGeometry
        {
            get
            {
                return (PathGeometry)this.GetValue(CurrentPathGeometryProperty);
            }
            set
            {
                this.SetValue(CurrentPathGeometryProperty, value);
            }
        }

        /// <summary>
        /// bindable DependencyProperty for painted geometry
        /// </summary>
        public static readonly DependencyProperty CurrentPathGeometryProperty = DependencyProperty.Register(
          "CurrentPathGeometry", typeof(PathGeometry), typeof(PaintToolInteractionSource),
            new PropertyMetadata(new PathGeometry(),
                new PropertyChangedCallback(OnCurrentGeometryChanged)));

        // helper to update when geometry changes
        static void OnCurrentGeometryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // nothing to do
        }

        /// <summary>
        /// diameter of paint tool
        /// </summary>
        public double Diameter
        {
            get
            {
                return (double)this.GetValue(DiameterProperty);
            }
            set
            {
                this.SetValue(DiameterProperty, value);
            }
        }

        /// <summary>
        /// bindable DependencyProperty for painted geometry
        /// </summary>
        public static readonly DependencyProperty DiameterProperty = DependencyProperty.Register(
          "Diameter", typeof(double), typeof(PaintToolInteractionSource),
            new PropertyMetadata((double)20.0,
                new PropertyChangedCallback(OnDiameterChanged)));

        // helper to update when geometry changes
        static void OnDiameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // update the tool's ellipse
            var tool = (PaintToolInteractionSource)d;
            tool._currentEllipse.RadiusX = tool.Diameter;
            tool._currentEllipse.RadiusY = tool.Diameter;
        }

        /// <summary>
        /// routed event (it should be a routed event) to help
        /// update the "cursor" state based on the current mouse position
        /// </summary>
        /// <param name="pt"></param>
        public void TrackMousePosition(Point pt)
        {
            // update the ellipse position
            _currentEllipse.Center = pt;

            // if we are in a drag, don't change anything
            if (_mouseDrag)
                return;

            // if there is currently no geometry...
            if (CurrentPathGeometry == null
                || CurrentPathGeometry.GetArea() == 0.0)
            {
                // then set to Union mode to allow drawing
                SetMode(GeometryCombineMode.Union);
            }
            else
            {
                // otherwise default to exclude
                SetMode(GeometryCombineMode.Exclude);
            }


            // Set up a callback to receive the hit test result enumeration.
            VisualTreeHelper.HitTest(this, null, 

                htr => SetModeOnResult(htr),

                // new HitTestResultCallback(MyHitTestResultCallback),
                new GeometryHitTestParameters(CurrentPathGeometry));
        }

        /// <summary>
        /// helper to set the mode, if the hit test indicates the ellipse intersects
        /// the current path geometry
        /// </summary>
        /// <param name="htr"></param>
        /// <returns></returns>
        HitTestResultBehavior SetModeOnResult(HitTestResult htr)
        {
            // Retrieve the results of the hit test.
            IntersectionDetail intersectionDetail = ((GeometryHitTestResult)htr).IntersectionDetail;
            switch (intersectionDetail)
            {
                case IntersectionDetail.FullyContains:
                    return HitTestResultBehavior.Continue;

                case IntersectionDetail.FullyInside:
                case IntersectionDetail.Intersects:

                    // if inside or intersects, set to union mode
                    SetMode(GeometryCombineMode.Union);
                    return HitTestResultBehavior.Continue;

                default:
                    return HitTestResultBehavior.Stop;
            }
        }

        /// <summary>
        /// sets the mode, adjusting the ellipse display parameters
        /// to indicate whether we are adding or subtracting
        /// </summary>
        /// <param name="mode"></param>
        void SetMode(GeometryCombineMode mode)
        {
            _mode = mode;
            if (_mode == GeometryCombineMode.Union)
            {
                _currentSpot.Stroke = Brushes.Gray;
                _currentSpot.Fill = Brushes.White;
            }
            else
            {
                _currentSpot.Stroke = Brushes.White;
                _currentSpot.Fill = Brushes.Gray;
            }
        }

        /// <summary>
        /// create the path, with a zero-width ellipse (so it is not empty)
        /// </summary>
        Path _currentSpot;
        EllipseGeometry _currentEllipse;

        // mouse flags
        bool _mouseDrag = false;
        GeometryCombineMode _mode = GeometryCombineMode.Union;

        private void PaintToolInteractionSource_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _mouseDrag = true;
        }

        private void PaintToolInteractionSource_MouseMove(object sender, MouseEventArgs e)
        {
            Point pt = e.GetPosition(this);
            _currentEllipse.Center = pt;

            if (_mouseDrag)
            {
                // combine using the current mode
                CurrentPathGeometry = Geometry.Combine(CurrentPathGeometry,
                    _currentEllipse, _mode, null);
            }
        }

        private void PaintToolInteractionSource_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _mouseDrag = false;
        }
    }
}
