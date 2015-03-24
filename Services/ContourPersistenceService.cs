using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace PaintToolCs
{
    /// <summary>
    /// class for persisting a contour
    /// </summary>
    public class ContourPersistenceService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public IEnumerable<Point3D> LoadContour(Guid guid)
        {
            yield return new Point3D();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="points3d"></param>
        /// <returns></returns>
        public Guid SaveContour(IEnumerable<Point3D> points3d)
        {
            Guid guid = Guid.NewGuid();
            System.Console.WriteLine("Saving contour {0}", guid);

            foreach (var point3d in points3d)
            {
                System.Console.WriteLine("Saving point {0}", point3d);
            }

            return guid;
        }
    }
}
