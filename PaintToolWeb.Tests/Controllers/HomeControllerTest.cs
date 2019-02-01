using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PaintToolWeb;
using PaintToolWeb.Controllers;

namespace PaintToolWeb.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.AreEqual("Your application description page.", result.ViewBag.Message);
        }

        [TestMethod]
        public void Contact()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Contact() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetThumbnail()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            var result = controller.GetThumbnail(string.Empty) as FileResult;

            // Assert
            Assert.IsNotNull(result);

        }


        [TestMethod]
        public void GetContour()
        {
            // Arrange
            HomeController controller = new HomeController();

        }

        [TestMethod]
        public void UpdateContour()
        {
            // Arrange
            var requestContext = new Mock<HttpContextBase>();

            HomeController controller = new HomeController();
            controller.ControllerContext =
                new ControllerContext(requestContext.Object, new RouteData(), controller);

            // Act
            controller.UpdateContour();

            // Assert
        }


    }
}
