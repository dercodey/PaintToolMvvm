using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PaintToolWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public FileResult GetThumbnail(string resourceName)
        {
            return File(string.Format("~/App_Data/{0}.jpg", resourceName), "image/jpeg");
        }

        public FileResult GetContour(string resourceName)
        {
            var eventDirectory = Server.MapPath("~/App_Data");
            var eventFiles = 
                Directory
                    .EnumerateFiles(eventDirectory, "*-contour.txt")
                    .OrderBy(fn => fn);

            var contentType = "text/plain; charset=utf-8";

            if (eventFiles.Count() > 0)
                return File(eventFiles.Last(), contentType);


            // return empty text result
            return File(new byte[] { }, contentType);
        }

        [HttpPost]
        public ActionResult UpdateContour()
        {
            // check content type
            var contentType = Request.ContentType;
            System.Diagnostics.Trace.Assert(contentType.StartsWith("text"));

            var eventFileBase = string.Format("~/App_Data/{0}-contour.txt",
                DateTime.Now.ToString("yyyyMMddHHmmss"));
            var eventFileName = Server.MapPath(eventFileBase);
            using (var fileStream = System.IO.File.Create(eventFileName))
            {
                Request.InputStream.Seek(0, SeekOrigin.Begin);
                Request.InputStream.CopyTo(fileStream);
            }

            return View();
        }
    }
}