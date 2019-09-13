using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PeerTutor.Controllers;
using PeerTutor.Models;
using PeerTutor.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Xunit;

namespace PeerTutor.Tests.Controller
{
    public class HomeControllerShould
    {
        private readonly Mock<ICourseRepository> mockRepo = new Mock<ICourseRepository>();
        private readonly Mock<IIdentity> identity = new Mock<IIdentity>();
        private readonly Mock<ClaimsPrincipal> mockPrincipal = new Mock<ClaimsPrincipal>();
        public HomeControllerShould()
        {
            
            mockRepo.Setup(repo => repo.GetCourses()).Returns(() =>
                new List<Course> { new Course {Id = 1, Major = "Computer Information Science", CourseTitle = "Big Ideas of Computer Science", CourseDescription = "This course introduces and explores seven “big ideas” of computer science. Students will develop computational thinking skills vital for success across all disciplines, including algorithmic creativity, data abstraction, and modeling and simulation. ", CourseImageThumbnailUrl = "" , CourseNumber="115", MajorShortCode = "CIS" },
                    new Course {Id = 2, Major = "Computer Information Science", CourseTitle = "Introduction to Programming", CourseDescription = "This course provides conceptual and logical tools for students planning to major in a computing-based major. Programming in a high-level language such as C++, Python, or Java, and the development of skills in abstraction, problem-solving, and algorithmic thinking are emphasized. ", CourseImageThumbnailUrl = "" , CourseNumber="121", MajorShortCode = "CIS" }});

            identity.SetupGet(i => i.IsAuthenticated).Returns(true);

            mockPrincipal.Setup(x => x.Identity).Returns(identity.Object);

        }

        [Fact]
        public void ReturnLandingPageIfNotUserAuthenticated()
        {
            //Arrange
                       
            identity.SetupGet(i => i.IsAuthenticated).Returns(false);
            
            var sut = new HomeController(mockRepo.Object);

            sut.ControllerContext.HttpContext = new DefaultHttpContext()
            {
                User = mockPrincipal.Object
            };
            
            //Act
            var result = sut.Index();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.True(string.IsNullOrEmpty(viewResult.ViewName) || viewResult.ViewName == "Index");
            identity.Verify(x => x.IsAuthenticated, Times.Once);


        }

        [Fact]
        public void RedirectToMenuPageIfUserAuthenticated()
        {
            //Arrange            
            var sut = new HomeController(mockRepo.Object);

            sut.ControllerContext.HttpContext = new DefaultHttpContext()
            {
                User = mockPrincipal.Object
            };

            //Act
            var result = sut.Index();

            //Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Menu", redirectToActionResult.ActionName);
            identity.Verify(x => x.IsAuthenticated, Times.Once);

        }

        [Fact]
        public void ReturnViewForMenuWithAListOfCourses()
        {
            //Arrange
            var sut = new HomeController(mockRepo.Object);

            //Act
            var result = sut.Menu();

            //Assert

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<HomeViewModel>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Courses.Count());

        }

        [Fact]
        public void ReturnViewForPrivacy()
        {
            //Arrange
            var sut = new HomeController(mockRepo.Object);

            //Act
            var result = sut.Privacy();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.True(string.IsNullOrEmpty(viewResult.ViewName) || viewResult.ViewName == "Privacy");
            identity.Verify(x => x.IsAuthenticated, Times.Never);

        }

        [Fact]
        public void ReturnViewForFaq()
        {
            //Arrange
            var sut = new HomeController(mockRepo.Object);

            //Act
            var result = sut.Faq();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.True(string.IsNullOrEmpty(viewResult.ViewName) || viewResult.ViewName == "Faq");
            identity.Verify(x => x.IsAuthenticated, Times.Never);


        }
    }
}
