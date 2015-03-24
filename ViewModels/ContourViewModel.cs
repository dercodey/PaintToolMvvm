using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace PaintToolCs
{
    /// <summary>
    /// represents a contour being edited
    /// </summary>
    public class ContourViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// form the contour with a fake base image
        /// </summary>
        public ContourViewModel(ContourPersistenceService persistService)
        {
            _persistService = persistService;
            BaseImage = ImageViewModel.CreateFromResource("CT-Lung-Metastases_1.jpg");
        }

        ContourPersistenceService _persistService;

        /// <summary>
        /// represents the base image
        /// </summary>
        public ImageViewModel BaseImage
        {
            get;
            set;
        }

        /// <summary>
        /// converts the 2D contour to a 3D spatial contour, and then persists
        /// </summary>
        public void SaveContour(double pixelSpacing, double sliceZ)
        {
            // enumerate the points at 1% fraction total length
            // TODO: how to convert this to a physical distance?
            var point2ds = GetPointEnumeration();

            // form 3D points from the 2D points, 
            var point3ds = from pt in point2ds
                           select new Point3D()
                           {
                               X = pt.X,
                               Y = pt.Y,
                               Z = 0.0,
                           };

            // scale the pixels by the pixel spacing amount
            var pixelSpacingTransform = new ScaleTransform3D(pixelSpacing, pixelSpacing, 1.0);
            var scaledPoint3ds = point3ds.ToArray();
            pixelSpacingTransform.Transform(scaledPoint3ds);

            // perform the z-shift
            var zShiftTransform = new TranslateTransform3D(new Vector3D(0.0, 0.0, sliceZ));
            var zShiftedPoint3ds = scaledPoint3ds;
            zShiftTransform.Transform(zShiftedPoint3ds);

            // and persist the result
            _persistService.SaveContour(zShiftedPoint3ds);
        }

        /// <summary>
        /// returns an enumeration of points on the contour
        /// </summary>
        /// <param name="stepSize"></param>
        /// <returns></returns>
        public IEnumerable<Point> GetPointEnumeration()
        {
#if USE_GET_POINT_AT_FRACTION_LENGTH
            double stepSize = 0.01;
            double atFraction = 0.0;
            Point currentPoint;
            Point currentTangent;
            while (atFraction <= 1.0)
            {
                _pathGeometry.GetPointAtFractionLength(atFraction, out currentPoint, out currentTangent);
                yield return currentPoint;
                atFraction += stepSize;
            }
#else
            // get the flattened geometry = geometry turned 
            //      to only PolyLineSegments and LineSegments
            PathGeometry flattened = _pathGeometry.GetFlattenedPathGeometry();

            // iterate over all the figures (in order)
            foreach (var figure in flattened.Figures)
            {
                // each figure is either...
                foreach (var segment in figure.Segments)
                {
                    // a PolyLineSegment
                    if (segment is PolyLineSegment)
                    {
                        var polyLineSegment = segment as PolyLineSegment;
                        foreach (var point in polyLineSegment.Points)
                        {
                            yield return point;
                        }
                    }
                    // or a LineSegment
                    else if (segment is LineSegment)
                    {
                        var lineSegment = segment as LineSegment;
                        yield return lineSegment.Point;
                    }
                }
            }
#endif
        }

        /// <summary>
        /// the contours current geometry
        /// </summary>
        public PathGeometry ContourGeometry
        {
            get { return _pathGeometry; }
            set
            {
                _pathGeometry = value;

                if (PropertyChanged != null)
                { 
                    // tell the world that the geometry has changed
                    PropertyChanged(this, new PropertyChangedEventArgs("ContourGeometry"));

                    // tell the world that the area is updated as well
                    PropertyChanged(this, new PropertyChangedEventArgs("ContourArea"));
                }
            }
        }

        PathGeometry _pathGeometry = new PathGeometry();

        /// <summary>
        /// 
        /// </summary>
        public double ContourArea
        {
            get { return _pathGeometry.GetArea(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
