using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PeerTutor.Controllers;
using PeerTutor.Models;
using PeerTutor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Xunit;

namespace PeerTutor.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void HomeControllerShouldReturnLandingPageIfNotAuthenticated()
        {

            //Arrange
            var mockRepo = new Mock<ICourseRepository>();
            mockRepo.Setup(repo => repo.GetCourses()).Returns(() =>
            new List<Course> { new Course {Id = 1, Major = "Computer Information Science", CourseTitle = "Big Ideas of Computer Science", CourseDescription = "This course introduces and explores seven “big ideas” of computer science. Students will develop computational thinking skills vital for success across all disciplines, including algorithmic creativity, data abstraction, and modeling and simulation. ", CourseImageThumbnailUrl = "" , CourseNumber="115", MajorShortCode = "CIS" },
                    new Course {Id = 2, Major = "Computer Information Science", CourseTitle = "Introduction to Programming", CourseDescription = "This course provides conceptual and logical tools for students planning to major in a computing-based major. Programming in a high-level language such as C++, Python, or Java, and the development of skills in abstraction, problem-solving, and algorithmic thinking are emphasized. ", CourseImageThumbnailUrl = "" , CourseNumber="121", MajorShortCode = "CIS" }});

            var identity = new Mock<IIdentity>();
            identity.SetupGet(i => i.IsAuthenticated).Returns(false);

            var mockPrincipal = new Mock<ClaimsPrincipal>();
            mockPrincipal.Setup(x => x.Identity).Returns(identity.Object);

            var controller = new HomeController(mockRepo.Object);

            controller.ControllerContext.HttpContext = new DefaultHttpContext()
            {
                User = mockPrincipal.Object
            };




            //Act
            var result = controller.Index();


            //Assert

            var viewResult = Assert.IsType<ViewResult>(result);




        }

        [Fact]
        public void HomeControllerShouldRedirectToMenuPageIfUserAuthenticated()
        {
            //Arrange
            var mockRepo = new Mock<ICourseRepository>();
            mockRepo.Setup(repo => repo.GetCourses()).Returns(() =>
            new List<Course> { new Course {Id = 1, Major = "Computer Information Science", CourseTitle = "Big Ideas of Computer Science", CourseDescription = "This course introduces and explores seven “big ideas” of computer science. Students will develop computational thinking skills vital for success across all disciplines, including algorithmic creativity, data abstraction, and modeling and simulation. ", CourseImageThumbnailUrl = "" , CourseNumber="115", MajorShortCode = "CIS" },
                    new Course {Id = 2, Major = "Computer Information Science", CourseTitle = "Introduction to Programming", CourseDescription = "This course provides conceptual and logical tools for students planning to major in a computing-based major. Programming in a high-level language such as C++, Python, or Java, and the development of skills in abstraction, problem-solving, and algorithmic thinking are emphasized. ", CourseImageThumbnailUrl = "" , CourseNumber="121", MajorShortCode = "CIS" }});

            var identity = new Mock<IIdentity>();
            identity.SetupGet(i => i.IsAuthenticated).Returns(true);

            var mockPrincipal = new Mock<ClaimsPrincipal>();
            mockPrincipal.Setup(x => x.Identity).Returns(identity.Object);

            var controller = new HomeController(mockRepo.Object);

            controller.ControllerContext.HttpContext = new DefaultHttpContext()
            {
                User = mockPrincipal.Object
            };

            //Act
            var result = controller.Index();

            //Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        }
    }
}
