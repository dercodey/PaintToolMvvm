using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace PaintToolMvvm
{
    public interface IContourPersistenceService
    {
        Task<IEnumerable<IEnumerable<Point>>> LoadContour(Guid guid);
        void SaveContour(IEnumerable<IEnumerable<Point>> contours);
    }
}