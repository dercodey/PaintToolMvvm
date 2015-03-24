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
    public class ContourView : Canvas
    {
        public ContourView()
        {
            // the current polygon represents (visually) the current state
            _currentPolygon = new Path();
            _currentPolygon.Stroke = Brushes.Red;
            _currentPolygon.StrokeThickness = 2;
            _currentPolygon.Fill = new SolidColorBrush(Color.FromArgb(0x44, 0xFF, 0x00, 0x00));
            _currentPolygon.Data = DisplayPathGeometry;
            this.Children.Add(_currentPolygon);
        }

        /// <summary>
        /// current geometry being painted
        /// </summary>
        public PathGeometry DisplayPathGeometry
        {
            get
            {
                return (PathGeometry)this.GetValue(DisplayPathGeometryProperty);
            }
            set
            {
                this.SetValue(DisplayPathGeometryProperty, value);
            }
        }

        /// <summary>
        /// bindable DependencyProperty for painted geometry
        /// </summary>
        public static readonly DependencyProperty DisplayPathGeometryProperty = DependencyProperty.Register(
          "DisplayPathGeometry", typeof(PathGeometry), typeof(ContourView),
            new PropertyMetadata(new PathGeometry(),
                new PropertyChangedCallback(OnDisplayPathGeometryChanged)));

        // helper to update when geometry changes
        static void OnDisplayPathGeometryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // get the new geometry
            PathGeometry newPath = (PathGeometry)e.NewValue;

            // and set the path to show it
            var source = (ContourView)d;
            source._currentPolygon.Data = newPath;
        }

        /// <summary>
        /// create the path, with a zero-width ellipse (so it is not empty)
        /// </summary>
        Path _currentPolygon;
    }
}
