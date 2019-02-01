using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using PaintToolCs;
using PaintToolLib;

namespace PaintToolMvvm.UnitTests
{
    [TestClass()]
    public class ContourPersistenceServiceTests
    {
        /// <summary>
        /// test the SvgToContour function in ContourPersistenceService
        /// </summary>
        [TestMethod()]
        public void SvgToContourTest()
        {
            // generate a stack of contours
            var inputContours = ContourHelpers.GenerateContours();

            // turn contour coordinates in to coordinate pair strings, suitable for
            //      inclusion in a points attribute
            var inputCoordinateStrings =
                inputContours
                    .Select(contour => 
                        string.Join(" ", 
                            contour.Select(tp => 
                                $"{tp.X.ToString("F4")},{tp.Y.ToString("F4")}")));

            // construct the svg 'polygon' elements with the points
            var inputPolygonsXml =
                string.Join("\n",
                    inputCoordinateStrings
                        .Select(contourCoordinateString => 
                            $"<polygon points=\"{contourCoordinateString}\" />"));

            // create the top-level svg element
            var inputSvgXml = $"<svg>{inputPolygonsXml}</svg>";

            // process to create contour enumerables
            var outputContours = 
                ContourHelpers.SvgToContours(inputSvgXml)
                    .ToList();


            // test that, 
            var bAllContoursMatch =
                outputContours.All(outputContour =>     // for all contours, 
                    inputContours.Any(inputContour =>   // there is at least one matching input
                        ContourHelpers.ContoursMatch(inputContour, outputContour)));

            Assert.IsTrue(bAllContoursMatch);
        }

        [TestMethod()]
        public void ContoursToSvgTest()
        {
            // generate a stack of contours
            var inputContours = ContourHelpers.GenerateContours();

            var svgXml = ContourHelpers.ContoursToSvg(inputContours);

            Assert.Fail();
        }
    }
}