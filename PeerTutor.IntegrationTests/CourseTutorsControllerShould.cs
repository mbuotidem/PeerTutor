using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PeerTutor.Controllers;
using PeerTutor.Extensions;
using PeerTutor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Xunit;

namespace PeerTutor.IntegrationTests
{
    public class CourseTutorsControllerShould
    {
        //Expertise is the user-facing name for CourseTutors
        [Fact]
        public async void ShowCurrentUsersExpertiseOnIndex()
        {

            // In-memory database only exists while the connection is open
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "ShowCurrentUsersExpertise")
                .Options;


            // Run the test against one instance of the context
            using (var context = new AppDbContext(options))
            {
                var user = new ApplicationUser
                {
                    UserName = "jdoe@person.com",
                    Email = "jdoe@person.com",
                    FirstName = "John",
                    LastName = "Doe",
                    Major = "Information Technology",
                    ClassYear = 2019,
                    PasswordHash = "ILoveYou!"
                };

                var course = new Course
                {
                    Major = "Computer Information Science",
                    CourseTitle = "Big Ideas of Computer Science",
                    CourseDescription = "This course introduces and explores seven “big ideas” of computer science. Students will develop computational thinking skills vital for success across all disciplines, " +
                    "including algorithmic creativity, data abstraction, and modeling and simulation. ",
                    CourseImageThumbnailUrl = "",
                    CourseNumber = "115",
                    MajorShortCode = "CIS"
                };

                context.Users.Add(user);
                context.Courses.Add(course);
                context.SaveChanges();

                //Get Id of saved user and saved course for use in saving expertise
                var userId = context.Users.FirstOrDefault().Id.ToString();
                var courseId = context.Courses.FirstOrDefault().Id;

                var expertise = new CourseTutor
                {
                    Grade = 0,
                    UserId = userId,
                    CourseId = courseId,
                };

                context.CourseTutors.Add(expertise);
                context.SaveChanges();

            }

            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new AppDbContext(options))
            {
                var userId = context.Users.FirstOrDefault().Id.ToString();
                var courseId = context.Courses.FirstOrDefault().Id;

                var sut = new CourseTutorsController(context);
                sut.ControllerContext.HttpContext = new DefaultHttpContext()
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                            new Claim(ClaimTypes.NameIdentifier, userId)
                    }))

                };

                IActionResult result = await sut.Index();
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = ((ViewResult)result).Model as IEnumerable<CourseTutor>;
                Assert.Single(model);
                Assert.Equal(CourseTutor.Grades.A, model.FirstOrDefault().Grade);
                Assert.Equal(userId, model.FirstOrDefault().UserId);
                Assert.Equal(courseId, model.FirstOrDefault().CourseId);
                sut.Dispose();
            }


        }

        [Fact]
        public async void NotShowOtherUsersExpertiseOnIndex()
        {

            // In-memory database only exists while the connection is open
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "NotShowOtherUsersExpertise")
                .Options;

            // Run the test against one instance of the context
            using (var context = new AppDbContext(options))
            {
                //Create two users and two courses. Save them to the database
                var user = new ApplicationUser
                {
                    UserName = "jdoe@person.com",
                    Email = "jdoe@person.com",
                    FirstName = "John",
                    LastName = "Doe",
                    Major = "Information Technology",
                    ClassYear = 2019,
                    PasswordHash = "ILoveYou!"
                };

                var user2 = new ApplicationUser
                {
                    UserName = "janedoe@person.com",
                    Email = "janedoe@person.com",
                    FirstName = "Jane",
                    LastName = "Doe",
                    Major = "Information Technology",
                    ClassYear = 2019,
                    PasswordHash = "ILikeYou!"
                };
                
                var course = new Course
                {
                    Major = "Computer Information Science",
                    CourseTitle = "Big Ideas of Computer Science",
                    CourseDescription = "This course introduces and explores seven “big ideas” of computer science. Students will develop computational thinking skills vital for success across all disciplines, " +
                    "including algorithmic creativity, data abstraction, and modeling and simulation. ",
                    CourseImageThumbnailUrl = "",
                    CourseNumber = "115",
                    MajorShortCode = "CIS"
                };

                var course2 = new Course
                {
                    Major = "Computer Information Science",
                    CourseTitle = "Introduction to Programming",
                    CourseDescription = "This course provides conceptual and logical tools for students planning to major in a computing-based major. Programming in a high-level language such as C++, Python, or Java, and the " +
                    "development of skills in abstraction, problem-solving, and algorithmic thinking are emphasized. ",
                    CourseImageThumbnailUrl = "",
                    CourseNumber = "121",
                    MajorShortCode = "CIS"
                };

                context.Users.Add(user);
                context.Users.Add(user2);
                context.Courses.Add(course);
                context.Courses.Add(course2);
                context.SaveChanges();

                //Obtain the user id's and course id's of the two saved users and saved courses
                //Save both of the created courses as the user userId's expertise. Save only one of the created courses as the user userId2's only expertise. 
                var userId = context.Users.FirstOrDefault().Id.ToString();
                var userId2 = context.Users.LastOrDefault().Id.ToString();
                var courseId = context.Courses.FirstOrDefault().Id;
                var courseId2 = context.Courses.LastOrDefault().Id;

                var expertise1ForUser1 = new CourseTutor
                {
                    Grade = 0,
                    UserId = userId,                    
                    CourseId = courseId,
                };

                var expertise2ForUser1 = new CourseTutor
                {
                    Grade = 0,
                    UserId = userId,
                    CourseId = courseId2,
                };

                var onlyExpertiseForUser2 = new CourseTutor
                {
                    Grade = 0,
                    UserId = userId2,
                    CourseId = courseId,
                };

                context.CourseTutors.Add(expertise1ForUser1);
                context.CourseTutors.Add(expertise2ForUser1);
                context.CourseTutors.Add(onlyExpertiseForUser2);
                context.SaveChanges();

            }

            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new AppDbContext(options))
            {
                var userId = context.Users.FirstOrDefault().Id.ToString();
                var userId2 = context.Users.LastOrDefault().Id.ToString();
                var sut = new CourseTutorsController(context);
                sut.ControllerContext.HttpContext = new DefaultHttpContext()
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                            new Claim(ClaimTypes.NameIdentifier, userId)
                    }))

                };

                IActionResult result = await sut.Index();
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = ((ViewResult)result).Model as IEnumerable<CourseTutor>;
                Assert.Equal(2, model.Count());
                Assert.NotEqual(userId2, model.First().UserId);
                Assert.NotEqual(userId2, model.Last().UserId);
                sut.Dispose();
            }

        }

        [Fact]
        public async void ShowExpertiseDetailsOnDetail()
        {
            // In-memory database only exists while the connection is open
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "ShowExpertiseDetails")
                .Options;

            // Run the test against one instance of the context
            using (var context = new AppDbContext(options))
            {
                //Add a user and a course to the database
                var user = new ApplicationUser
                {
                    UserName = "jdoe@person.com",
                    Email = "jdoe@person.com",
                    FirstName = "John",
                    LastName = "Doe",
                    Major = "Information Technology",
                    ClassYear = 2019,
                    PasswordHash = "ILoveYou!"
                };

                var course = new Course
                {
                    Major = "Computer Information Science",
                    CourseTitle = "Big Ideas of Computer Science",
                    CourseDescription = "This course introduces and explores seven “big ideas” of computer science. Students will develop computational thinking skills vital for success across all disciplines, " +
                    "including algorithmic creativity, data abstraction, and modeling and simulation. ",
                    CourseImageThumbnailUrl = "",
                    CourseNumber = "115",
                    MajorShortCode = "CIS"
                };

                context.Users.Add(user);
                context.Courses.Add(course);
                context.SaveChanges();

                //Obtain the id's for the created user and course then add the created course as the created user's expertise
                var userId = context.Users.FirstOrDefault().Id.ToString();
                var courseId = context.Courses.FirstOrDefault().Id;

                var expertise = new CourseTutor
                {
                    Grade = 0,
                    UserId = userId,
                    CourseId = courseId,
                };

                context.CourseTutors.Add(expertise);
                context.SaveChanges();

            }

            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new AppDbContext(options))
            {
                var userId = context.Users.FirstOrDefault().Id.ToString();
                var courseId = context.Courses.FirstOrDefault().Id;

                var sut = new CourseTutorsController(context);
                sut.ControllerContext.HttpContext = new DefaultHttpContext()
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                            new Claim(ClaimTypes.NameIdentifier, userId)
                    }))

                };

                IActionResult result = await sut.Details(userId, courseId);
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = ((ViewResult)result).Model as CourseTutor;
                Assert.Equal(CourseTutor.Grades.A, model.Grade);
                Assert.Equal(userId, model.UserId);
                Assert.Equal(courseId, model.CourseId);
                sut.Dispose();
            }

        }

        [Fact]
        public async void NotShowExpertiseDetailsOnDetailIfNotCurrentUser()
        {
            // In-memory database only exists while the connection is open
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "NotShowExpertiseDetails")
                .Options;

            // Run the test against one instance of the context
            using (var context = new AppDbContext(options))
            {
                //Add a user and a course to the database
                var user = new ApplicationUser
                {
                    UserName = "jdoe@person.com",
                    Email = "jdoe@person.com",
                    FirstName = "John",
                    LastName = "Doe",
                    Major = "Information Technology",
                    ClassYear = 2019,
                    PasswordHash = "ILoveYou!"
                };

                var course = new Course
                {
                    Major = "Computer Information Science",
                    CourseTitle = "Big Ideas of Computer Science",
                    CourseDescription = "This course introduces and explores seven “big ideas” of computer science. Students will develop computational thinking skills vital for success across all disciplines, " +
                    "including algorithmic creativity, data abstraction, and modeling and simulation. ",
                    CourseImageThumbnailUrl = "",
                    CourseNumber = "115",
                    MajorShortCode = "CIS"
                };

                context.Users.Add(user);
                context.Courses.Add(course);
                context.SaveChanges();

                //Obtain the id's for the created user and course then add the created course as the created user's expertise
                var userId = context.Users.FirstOrDefault().Id.ToString();
                var courseId = context.Courses.FirstOrDefault().Id;

                var expertise = new CourseTutor
                {
                    Grade = 0,
                    UserId = userId,
                    CourseId = courseId,
                };

                context.CourseTutors.Add(expertise);
                context.SaveChanges();
            }

            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new AppDbContext(options))
            {
                var userId = Guid.NewGuid().ToString();
                var courseId = context.Courses.FirstOrDefault().Id;

                var sut = new CourseTutorsController(context);
                sut.ControllerContext.HttpContext = new DefaultHttpContext()
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                            new Claim(ClaimTypes.NameIdentifier, userId)
                    }))

                };

                var result = await sut.Details(userId, courseId);
                var viewResult = Assert.IsType<AlertDecoratorResult>(result);
                Assert.Equal("The action you requested was not allowed and thus could not be completed.", viewResult.Body);
                sut.Dispose();
            }
        }

        [Fact]
        public void ShowCourseForCreateExpertiseOnCreate()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
               .UseInMemoryDatabase(databaseName: "ShowCreateExpertise")
               .Options;

            using (var context = new AppDbContext(options))
            {
                //Add user and course to database
                var user = new ApplicationUser
                {
                    UserName = "jdoe@person.com",
                    Email = "jdoe@person.com",
                    FirstName = "John",
                    LastName = "Doe",
                    Major = "Information Technology",
                    ClassYear = 2019,
                    PasswordHash = "ILoveYou!"
                };

                var course = new Course
                {
                    Major = "Computer Information Science",
                    CourseTitle = "Big Ideas of Computer Science",
                    CourseDescription = "This course introduces and explores seven “big ideas” of computer science. Students will develop computational thinking skills vital for success across all disciplines, " +
                   "including algorithmic creativity, data abstraction, and modeling and simulation. ",
                    CourseImageThumbnailUrl = "",
                    CourseNumber = "115",
                    MajorShortCode = "CIS"
                };

                context.Users.Add(user);
                context.Courses.Add(course);
                context.SaveChanges();
            }

            using (var context = new AppDbContext(options))
            {
                //Obtain the id's for the created user and course then add the created course as the created user's expertise
                var userId = context.Users.FirstOrDefault().Id.ToString();
                var courseId = context.Courses.FirstOrDefault().Id;

                var sut = new CourseTutorsController(context);
                sut.ControllerContext.HttpContext = new DefaultHttpContext()
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, userId)
                    }))

                };

                IActionResult result = sut.Create();
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Equal(userId, viewResult.ViewData["UserId"]);
                Assert.IsAssignableFrom<SelectList>(viewResult.ViewData["CourseId"]);
                sut.Dispose();
            }

        }

        [Fact]
        public async void CreateExpertiseOnCreate()
        {

            // In-memory database only exists while the connection is open
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "CreateExpertise")
                .Options;

            // Run the test against one instance of the context
            using (var context = new AppDbContext(options))
            {
                //Add user and course to database
                var user = new ApplicationUser
                {
                    UserName = "jdoe@person.com",
                    Email = "jdoe@person.com",
                    FirstName = "John",
                    LastName = "Doe",
                    Major = "Information Technology",
                    ClassYear = 2019,
                    PasswordHash = "ILoveYou!"
                };

                var course = new Course
                {
                    Major = "Computer Information Science",
                    CourseTitle = "Big Ideas of Computer Science",
                    CourseDescription = "This course introduces and explores seven “big ideas” of computer science. Students will develop computational thinking skills vital for success across all disciplines, " +
                    "including algorithmic creativity, data abstraction, and modeling and simulation. ",
                    CourseImageThumbnailUrl = "",
                    CourseNumber = "115",
                    MajorShortCode = "CIS"
                };

                context.Users.Add(user);
                context.Courses.Add(course);
                context.SaveChanges();

                //Obtain the id's for the created user and course then add the created course as the created user's expertise
                var userId = context.Users.FirstOrDefault().Id.ToString();
                var courseId = context.Courses.FirstOrDefault().Id;

                var expertise = new CourseTutor
                {
                    Grade = 0,
                    UserId = userId,
                    CourseId = courseId,
                };

                var sut = new CourseTutorsController(context);
                sut.ControllerContext.HttpContext = new DefaultHttpContext()
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                            new Claim(ClaimTypes.NameIdentifier, userId)
                    }))

                };

                await sut.Create(expertise);
                sut.Dispose();
            }

            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new AppDbContext(options))
            {
                var userId = context.Users.FirstOrDefault().Id.ToString();
                var courseId = context.Courses.FirstOrDefault().Id;
                Assert.Equal(1, context.CourseTutors.Count());
                Assert.Equal(CourseTutor.Grades.A, context.CourseTutors.FirstOrDefault().Grade);
                Assert.Equal(userId, context.CourseTutors.FirstOrDefault().UserId);
                Assert.Equal(courseId, context.CourseTutors.FirstOrDefault().CourseId);
            }

        }

        [Fact]
        public async void ShowExpertiseForEditOnEdit()
        {
            // In-memory database only exists while the connection is open
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "ShowExpertiseForEdit")
                .Options;

            // Run the test against one instance of the context
            using (var context = new AppDbContext(options))
            {
                //Add user and course to database
                var user = new ApplicationUser
                {
                    UserName = "jdoe@person.com",
                    Email = "jdoe@person.com",
                    FirstName = "John",
                    LastName = "Doe",
                    Major = "Information Technology",
                    ClassYear = 2019,
                    PasswordHash = "ILoveYou!"
                };

                var course = new Course
                {
                    Major = "Computer Information Science",
                    CourseTitle = "Big Ideas of Computer Science",
                    CourseDescription = "This course introduces and explores seven “big ideas” of computer science. Students will develop computational thinking skills vital for success across all disciplines, " +
                    "including algorithmic creativity, data abstraction, and modeling and simulation. ",
                    CourseImageThumbnailUrl = "",
                    CourseNumber = "115",
                    MajorShortCode = "CIS"
                };

                context.Users.Add(user);
                context.Courses.Add(course);
                context.SaveChanges();

                //Obtain the id's for the created user and course then add the created course as the created user's expertise
                var userId = context.Users.FirstOrDefault().Id.ToString();
                var courseId = context.Courses.FirstOrDefault().Id;

                var expertise = new CourseTutor
                {
                    Grade = 0,
                    UserId = userId,
                    CourseId = courseId,
                };

                context.CourseTutors.Add(expertise);
                context.SaveChanges();

            }

            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new AppDbContext(options))
            {
                var userId = context.Users.FirstOrDefault().Id.ToString();
                var courseId = context.Courses.FirstOrDefault().Id;

                var sut = new CourseTutorsController(context);
                sut.ControllerContext.HttpContext = new DefaultHttpContext()
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                            new Claim(ClaimTypes.NameIdentifier, userId)
                    }))

                };

                IActionResult result = await sut.Edit(userId, courseId);
                var viewResult = Assert.IsType<ViewResult>(result);
                var model = ((ViewResult)result).Model as CourseTutor;
                Assert.Equal(CourseTutor.Grades.A, model.Grade);
                Assert.IsAssignableFrom<SelectList>(viewResult.ViewData["UserId"]);
                Assert.IsAssignableFrom<SelectList>(viewResult.ViewData["CourseId"]);
                sut.Dispose();
            }

        }

        [Fact]

        public async void EditExpertiseOnEdit()
        {
            // In-memory database only exists while the connection is open
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "EditExpertise")
                .Options;

            // Run the test against one instance of the context
            using (var context = new AppDbContext(options))
            {
                //Add user and course to database
                var user = new ApplicationUser
                {
                    UserName = "jdoe@person.com",
                    Email = "jdoe@person.com",
                    FirstName = "John",
                    LastName = "Doe",
                    Major = "Information Technology",
                    ClassYear = 2019,
                    PasswordHash = "ILoveYou!"
                };

                var course = new Course
                {
                    Major = "Computer Information Science",
                    CourseTitle = "Big Ideas of Computer Science",
                    CourseDescription = "This course introduces and explores seven “big ideas” of computer science. Students will develop computational thinking skills vital for success across all disciplines, " +
                    "including algorithmic creativity, data abstraction, and modeling and simulation. ",
                    CourseImageThumbnailUrl = "",
                    CourseNumber = "115",
                    MajorShortCode = "CIS"
                };

                context.Users.Add(user);
                context.Courses.Add(course);
                context.SaveChanges();

                //Obtain the id's for the created user and course then add the created course as the created user's expertise
                var userId = context.Users.FirstOrDefault().Id.ToString();
                var courseId = context.Courses.FirstOrDefault().Id;

                var expertise = new CourseTutor
                {
                    Grade = 0,
                    UserId = userId,
                    CourseId = courseId,
                };

                context.CourseTutors.Add(expertise);
                context.SaveChanges();

            }

            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new AppDbContext(options))
            {
                var userId = context.Users.FirstOrDefault().Id.ToString();
                var courseId = context.Courses.FirstOrDefault().Id;

                var sut = new CourseTutorsController(context);
                sut.ControllerContext.HttpContext = new DefaultHttpContext()
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                            new Claim(ClaimTypes.NameIdentifier, userId)
                    }))

                };

                var expertiseToEdit = context.CourseTutors.FirstOrDefault();

                //Change grade from A to B
                expertiseToEdit.Grade = CourseTutor.Grades.B;

                IActionResult result = await sut.Edit(expertiseToEdit.UserId, expertiseToEdit);
                var viewResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal(CourseTutor.Grades.B, context.CourseTutors.FirstOrDefault().Grade);
                sut.Dispose();
            }

        }

        [Fact]
        public async void DeleteExpertiseOnDelete()
        {
            // In-memory database only exists while the connection is open
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "DeleteExpertise")
                .Options;

            // Run the test against one instance of the context
            using (var context = new AppDbContext(options))
            {
                //Add user and course to the database
                var user = new ApplicationUser
                {
                    UserName = "jdoe@person.com",
                    Email = "jdoe@person.com",
                    FirstName = "John",
                    LastName = "Doe",
                    Major = "Information Technology",
                    ClassYear = 2019,
                    PasswordHash = "ILoveYou!"
                };

                var course = new Course
                {
                    Major = "Computer Information Science",
                    CourseTitle = "Big Ideas of Computer Science",
                    CourseDescription = "This course introduces and explores seven “big ideas” of computer science. Students will develop computational thinking skills vital for success across all disciplines, " +
                    "including algorithmic creativity, data abstraction, and modeling and simulation. ",
                    CourseImageThumbnailUrl = "",
                    CourseNumber = "115",
                    MajorShortCode = "CIS"
                };

                context.Users.Add(user);
                context.Courses.Add(course);
                context.SaveChanges();

                //Obtain the id's for the created user and course then add the created course as the created user's expertise
                var userId = context.Users.FirstOrDefault().Id.ToString();
                var courseId = context.Courses.FirstOrDefault().Id;

                var expertise = new CourseTutor
                {
                    Grade = 0,
                    UserId = userId,
                    CourseId = courseId,
                };

                context.CourseTutors.Add(expertise);
                context.SaveChanges();

            }

            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new AppDbContext(options))
            {
                var userId = context.Users.FirstOrDefault().Id.ToString();
                var courseId = context.Courses.FirstOrDefault().Id;

                var sut = new CourseTutorsController(context);
                sut.ControllerContext.HttpContext = new DefaultHttpContext()
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                            new Claim(ClaimTypes.NameIdentifier, userId)
                    }))

                };

                IActionResult result = await sut.DeleteConfirmed(userId, courseId);
                var viewResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal(0, context.CourseTutors.Count());
                sut.Dispose();
            }

        }

    }

}

