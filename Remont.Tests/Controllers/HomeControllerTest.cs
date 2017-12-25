using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Remont;
using Remont.Controllers;
using Moq;
using Remont.Core;
using Remont.Business;

namespace Remont.Tests.Controllers
{
	[TestClass]
	public class HomeControllerTest
	{
		[TestMethod]
		public void Index()
		{
			var mockRepository = new Mock<IRemontRepository>();
			var mockManager = new Mock<IHomeManager>();
			// Arrange
			HomeController controller = new HomeController(mockManager.Object);

			// Act
			ViewResult result = controller.Index() as ViewResult;

			// Assert
			Assert.AreEqual("Modify this template to jump-start your ASP.NET MVC application.", result.ViewBag.Message);
		}

		[TestMethod]
		public void About()
		{
			var mockRepository = new Mock<IRemontRepository>();
			var mockManager = new Mock<IHomeManager>();

			// Arrange
			HomeController controller = new HomeController(mockManager.Object);

			// Act
			ViewResult result = controller.About() as ViewResult;

			// Assert
			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void Contact()
		{
			var mockRepository = new Mock<IRemontRepository>();
			var mockManager = new Mock<IHomeManager>();

			// Arrange
			HomeController controller = new HomeController(mockManager.Object);

			// Act
			ViewResult result = controller.Contact() as ViewResult;

			// Assert
			Assert.IsNotNull(result);
		}
	}
}
