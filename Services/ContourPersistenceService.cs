using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

using PaintToolLib;

namespace PaintToolMvvm
{
    /// <summary>
    /// class for persisting a contour
    /// </summary>
    public class ContourPersistenceService : IContourPersistenceService
    {
        // set up the HTTP client that is used for queries and saves
        static HttpClient client = new HttpClient();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public async 
            Task<IEnumerable<IEnumerable<Point>>> 
                LoadContour(Guid guid) =>

            // turn the XML returned by the query to a collection of contours
            ContourHelpers.SvgToContours(await client.GetStringAsync(GetContourQueryUrl()));

        /// <summary> </summary>
        /// <param name="points3d"></param>
        /// <returns></returns>
        public void 
            SaveContour(IEnumerable<IEnumerable<Point>> contours) =>

            // post the saved contour to the save URL
            client.PostAsync(GetContourSaveUrl(),
                new StringContent(ContourHelpers.ContoursToSvg(contours).ToString()));

        /// <summary> </summary>
        /// <returns></returns>
        private static 
            string 
                GetContourQueryUrl() =>

            string.Format("{0}{1}",
                ConfigurationManager.AppSettings["paintToolWebBase"],
                ConfigurationManager.AppSettings["contourLoadQuery"]);

        /// <summary> </summary>
        /// <returns></returns>
        private static 
            string 
                GetContourSaveUrl() =>

            string.Format("{0}{1}",
                ConfigurationManager.AppSettings["paintToolWebBase"],
                ConfigurationManager.AppSettings["contourSaveQuery"]);
    }
}
