using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace PaintToolMvvm
{
    /// <summary>
    /// represents a contour being edited
    /// </summary>
    public class ContourViewModel : INotifyPropertyChanged
    {

        IContourPersistenceService _contourSvc;

        /// <summary>
        /// form the contour with a fake base image
        /// </summary>
        public ContourViewModel(IContourPersistenceService contourSvc, 
                                    IImagePersistenceService imageSvc)
        {
            BaseImage = 
                ImageViewModel.CreateFromResource(imageSvc, 
                    "CT_Lung_Metastases_1");

            _contourSvc = contourSvc;
            _contourSvc.LoadContour(Guid.Empty)
                .ContinueWith(LoadPointsToContour, 
                    TaskScheduler.FromCurrentSynchronizationContext());
        }

        /// <summary>  </summary>
        /// <param name="contourTask"></param>
        private
            void
                LoadPointsToContour(Task<IEnumerable<IEnumerable<Point>>> contourTask)
        {
            var contours = contourTask.Result;
            ContourGeometry =
                new PathGeometry(
                    contours
                        .Select(contour =>
                            new PathFigure(contour.First(),
                                contour
                                    .Skip(1)
                                    .Select(pt => new LineSegment(pt, true))
                                    .ToList(),
                                false)) //true if closed
                        .ToList());
        }

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
        public void SaveContour()
        {
            // and persist the result
            _contourSvc.SaveContour(GetPointEnumeration());
        }

        /// <summary>
        /// returns an enumeration of points on the contour
        /// </summary>
        /// <param name="stepSize"></param>
        /// <returns></returns>
        internal
            IEnumerable<IEnumerable<Point>> 
                GetPointEnumeration()
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
            PathGeometry flattened = 
                _pathGeometry.GetFlattenedPathGeometry();

            return flattened.Figures.SelectMany(figure => 
                    figure.Segments.Select(PointsFromSegment));
#endif
        }

        /// <summary> </summary>
        /// <param name="segment"></param>
        /// <returns></returns>
        internal 
            IEnumerable<Point> 
                PointsFromSegment(PathSegment segment)
        {
            // a PolyLineSegment
            if (segment is PolyLineSegment pls)
            {
                return pls.Points.Cast<Point>();
            }
            // or a LineSegment
            else if (segment is LineSegment ls)
            {
                return Enumerable.Repeat(ls.Point, 1);
            }

            return null;
        }

        /// <summary>
        /// the contours current geometry
        /// </summary>
        public PathGeometry ContourGeometry
        {
            get => _pathGeometry;
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
            get => _pathGeometry.GetArea();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
