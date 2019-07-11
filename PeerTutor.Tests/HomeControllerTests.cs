using Microsoft.AspNetCore.Mvc;
using Moq;
using PeerTutor.Controllers;
using PeerTutor.Models;
using PeerTutor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace PeerTutor.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void HomeController_Index_ReturnsAViewResult_WithAListOfCourses()
        {

            //Arrange
            var mockRepo = new Mock<ICourseRepository>();
            mockRepo.Setup(repo => repo.GetCourses()).Returns(() =>
            new List<Course> { new Course {Id = 1, Major = "Computer Information Science", CourseTitle = "Big Ideas of Computer Science", CourseDescription = "This course introduces and explores seven “big ideas” of computer science. Students will develop computational thinking skills vital for success across all disciplines, including algorithmic creativity, data abstraction, and modeling and simulation. ", CourseImageThumbnailUrl = "" , CourseNumber="115", MajorShortCode = "CIS" },
                    new Course {Id = 2, Major = "Computer Information Science", CourseTitle = "Introduction to Programming", CourseDescription = "This course provides conceptual and logical tools for students planning to major in a computing-based major. Programming in a high-level language such as C++, Python, or Java, and the development of skills in abstraction, problem-solving, and algorithmic thinking are emphasized. ", CourseImageThumbnailUrl = "" , CourseNumber="121", MajorShortCode = "CIS" }});
            var controller = new HomeController(mockRepo.Object);

            //Act
            var result = controller.Index();
            

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<HomeViewModel>(
                viewResult.ViewData.Model);
            Assert.Equal("Courses", model.Title);
            Assert.Equal(2, model.Courses.Count());



        }
    }
}
