using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

using PaintToolLib;

namespace PaintToolMvvm.UnitTests
{
    [TestClass()]
    public class ContourViewModelTests
    {
        [TestInitialize()]
        public void InitializeSynchronizationContext()
        {
            SynchronizationContext.SetSynchronizationContext(
                new SynchronizationContext());
        }

        [TestMethod()]
        public void ContourViewModelTest()
        {
            var guid = Guid.NewGuid();

            IEnumerable<IEnumerable<Point>> inputContours = 
                ContourHelpers.GenerateContours();

            var mockContourSvc = new Mock<IContourPersistenceService>();
            mockContourSvc
                .Setup(svc => svc.LoadContour(guid))
                .Returns(Task.FromResult(inputContours));

            var mockImageSvc = new Mock<IImagePersistenceService>();
            var cvm = new ContourViewModel(mockContourSvc.Object, mockImageSvc.Object);

            // test that, 
            var bAllContoursMatch =
                cvm.ContourGeometry.Figures.All(outputContour =>     // for all contours, 
                    inputContours.Any(inputContour =>   // there is at least one matching input
                        ContourHelpers.ContoursMatch(inputContour,
                            outputContour.Segments.Cast<Point>())));

            Assert.IsTrue(bAllContoursMatch);
        }

        [TestMethod()]
        public void SaveContourTest()
        {
            // generate a stack of contours
            var inputContours = ContourHelpers.GenerateContours();
            var svgXml = ContourHelpers.ContoursToSvg(inputContours);

            var mockContourSvc = new Mock<IContourPersistenceService>();
            IEnumerable<IEnumerable<Point>> storedContours = null;
            mockContourSvc
                .Setup(svc => svc.SaveContour(inputContours))
                .Callback<IEnumerable<IEnumerable<Point>>>(
                    contoursFromVm => storedContours = contoursFromVm);

            var mockImageSvc = new Mock<IImagePersistenceService>();
            var cvm = new ContourViewModel(mockContourSvc.Object, mockImageSvc.Object);
            cvm.SaveContour();

            var bAllContoursMatch =
                storedContours.All(storedContour =>     // for all contours, 
                    inputContours.Any(inputContour =>   // there is at least one matching input
                        ContourHelpers.ContoursMatch(inputContour, storedContour)));
            Assert.IsTrue(bAllContoursMatch);
        }
    }
}