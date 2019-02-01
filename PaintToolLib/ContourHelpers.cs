using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Xml.Linq;

namespace PaintToolLib
{
    public static class ContourHelpers
    {
        static Random _rand = new Random();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static 
            IEnumerable<IEnumerable<Point>> 
                GenerateContours() =>

            // generate a stack of contours
            Enumerable.Range(1, _rand.Next(1, 100))
                .Select(contourNumber =>
                    Enumerable.Range(1, _rand.Next(1, 1000))
                        .Select(i =>
                            new Point(_rand.NextDouble() * 100.0 - 50.0,
                                        _rand.NextDouble() * 100.0 - 50.0))
                        .ToList()       // make sure we don't regenerate 
                        .AsEnumerable())
                .ToList()               // after initialization
                .AsEnumerable();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static 
            bool 
                ContoursMatch(IEnumerable<Point> left, IEnumerable<Point> right) =>

            // compare the collections to see that the point distances are below threshold
            left.Count() == right.Count()
            && left.Zip(right, 
                        (leftPt, rightPt) => 
                        (leftPt - rightPt).Length)
                    .All(err => err < 1e-3);


        /// <summary> </summary>
        /// <param name="points3d"></param>
        /// <returns></returns>
        public static
            XElement
                ContoursToSvg(IEnumerable<IEnumerable<Point>> contours) =>

            // form the svg element with nested polygons--one for each contour
            new XElement(XName.Get("svg"),
                contours.Select(contour =>
                    new XElement(XName.Get("polygon"),
                        new XAttribute(XName.Get("points"),
                            String.Join(" ",
                                contour.Select(pt => $"{pt.X},{pt.Y}"))))));


        /// <summary> </summary>
        /// <param name="svgXmlResult"></param>
        /// <returns></returns>
        public static
            IEnumerable<IEnumerable<Point>>
                SvgToContours(string svgXmlResult) =>

            // parse the XML string to get the polygons
            XElement.Parse(svgXmlResult)
                .Descendants(XName.Get("polygon"))
                .Select(el =>
                        PointAttributeToContour(
                            el.Attribute(XName.Get("points"))));

        /// <summary> </summary>
        /// <param name="pointsAttribute"></param>
        /// <returns></returns>
        private static
            IEnumerable<Point>
                PointAttributeToContour(XAttribute pointsAttribute) =>

            // turn a point attribute to a collection of points = contour
            regexCoordinates
                .Matches(pointsAttribute.Value)
                .Cast<Match>().Select(
                    mtch => new Point(
                        Double.Parse(mtch.Groups[1].Value),
                        Double.Parse(mtch.Groups[2].Value)));

        // the regex for parsing X,Y coordinates
        static Regex regexCoordinates =
            new Regex(@"([-+]?[0-9]*\.?[0-9]+),([-+]?[0-9]*\.?[0-9]+)\s*");


    }
}
