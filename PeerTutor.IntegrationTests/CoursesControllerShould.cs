//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Data.Sqlite;
//using Microsoft.EntityFrameworkCore;
//using PeerTutor.Controllers;
//using PeerTutor.Models;
//using System.Linq;
//using Xunit;


//namespace PeerTutor.Tests.Controller
//{
//    public class CoursesControllerShould
//    {
//       [Fact]

//       public async void ShowAllCourses()
//        {
//            // In-memory database only exists while the connection is open
//            var connection = new SqliteConnection("DataSource=:memory:");
//            connection.Open();

//            try
//            {
//                var options = new DbContextOptionsBuilder<AppDbContext>()
//                    .UseSqlite(connection)
//                    .Options;

//                // Create the schema in the database
//                using (var context = new AppDbContext(options))
//                {
//                    context.Database.EnsureCreated();
//                }

//                // Run the test against one instance of the context
//                using (var context = new AppDbContext(options))
//                {
//                    var controller = new CoursesController(context);

//                    var course = new Course
//                    {
//                        Major = "Computer Information Science",
//                        CourseTitle = "Big Ideas of Computer Science",
//                        CourseDescription = "This course introduces and explores seven “big ideas” of computer science. Students will develop computational thinking skills vital for success across all disciplines, " +
//                        "including algorithmic creativity, data abstraction, and modeling and simulation. ",
//                        CourseImageThumbnailUrl = "",
//                        CourseNumber = "115",
//                        MajorShortCode = "CIS"
//                    };

//                    var course2 = new Course
//                    {
//                        Major = "Computer Information Science",
//                        CourseTitle = "Big Ideas of Computer Science",
//                        CourseDescription = "This course provides conceptual and logical tools for students planning to major in a computing-based major. Programming in a high-level language such as C++, Python, or Java, and the development of skills in abstraction, problem-solving, and algorithmic thinking are emphasized. " +
//                        "including algorithmic creativity, data abstraction, and modeling and simulation. ",
//                        CourseImageThumbnailUrl = "",
//                        CourseNumber = "121",
//                        MajorShortCode = "CIS"
//                    };

//                    await controller.Create(course);
//                    await controller.Create(course2);

//                    context.SaveChanges();
//                }

//                // Use a separate instance of the context to verify correct data was saved to database
//                using (var context = new AppDbContext(options))
//                {
//                    var sut = new CoursesController(context);

//                    IActionResult result = await sut.Index();

//                    var viewResult = Assert.IsType<ViewResult>(result);
//                    Assert.Equal(2, context.Courses.Count());
//                    Assert.True(string.IsNullOrEmpty(viewResult.ViewName) || viewResult.ViewName == "Index");
//                    //Assert.Equal("Computer Information Science", context.Courses.Single().Major);
//                }
//            }
//            finally
//            {
//                connection.Close();
//            }
//        }



//       [Fact]
//        public async void AddCourseToDatabase()
//        {
//            // In-memory database only exists while the connection is open
//            var connection = new SqliteConnection("DataSource=:memory:");
//            connection.Open();

//            try
//            {
//                var options = new DbContextOptionsBuilder<AppDbContext>()
//                    .UseSqlite(connection)
//                    .Options;

//                // Create the schema in the database
//                using (var context = new AppDbContext(options))
//                {
//                    context.Database.EnsureCreated();
//                }

//                // Run the test against one instance of the context
//                using (var context = new AppDbContext(options))
//                {
//                    var sut = new CoursesController(context);

//                    var course = new Course
//                    {
//                        Major = "Computer Information Science",
//                        CourseTitle = "Big Ideas of Computer Science",
//                        CourseDescription = "This course introduces and explores seven “big ideas” of computer science. Students will develop computational thinking skills vital for success across all disciplines, " +
//                        "including algorithmic creativity, data abstraction, and modeling and simulation. ",
//                        CourseImageThumbnailUrl = "",
//                        CourseNumber = "115",
//                        MajorShortCode = "CIS"
//                    };

//                    await sut.Create(course);

//                    context.SaveChanges();
//                }

//                // Use a separate instance of the context to verify correct data was saved to database
//                using (var context = new AppDbContext(options))
//                {
//                    Assert.Equal(1, context.Courses.Count());
//                    Assert.Equal("Computer Information Science", context.Courses.Single().Major);
//                    Assert.Equal("Big Ideas of Computer Science", context.Courses.Single().CourseTitle);
//                    Assert.Equal("This course introduces and explores seven “big ideas” of computer science. Students will develop computational thinking skills vital for success across all disciplines, " +
//                        "including algorithmic creativity, data abstraction, and modeling and simulation. ", context.Courses.Single().CourseDescription);
//                    Assert.Equal("", context.Courses.Single().CourseImageThumbnailUrl);
//                    Assert.Equal("115", context.Courses.Single().CourseNumber);
//                    Assert.Equal("CIS", context.Courses.Single().MajorShortCode);
//                }
//            }
//            finally
//            {
//                connection.Close();
//            }
//        }

//        [Fact]
//        public async void DeleteCourseFromDatabase()
//        {
//            // In-memory database only exists while the connection is open
//            var connection = new SqliteConnection("DataSource=:memory:");
//            connection.Open();

//            try
//            {
//                var options = new DbContextOptionsBuilder<AppDbContext>()
//                    .UseSqlite(connection)
//                    .Options;

//                // Create the schema in the database
//                using (var context = new AppDbContext(options))
//                {
//                    context.Database.EnsureCreated();
//                }

//                // Run the test against one instance of the context
//                using (var context = new AppDbContext(options))
//                {
//                    var controller = new CoursesController(context);

//                    var course = new Course
//                    {
//                        Major = "Computer Information Science",
//                        CourseTitle = "Big Ideas of Computer Science",
//                        CourseDescription = "This course introduces and explores seven “big ideas” of computer science. Students will develop computational thinking skills vital for success across all disciplines, " +
//                        "including algorithmic creativity, data abstraction, and modeling and simulation. ",
//                        CourseImageThumbnailUrl = "",
//                        CourseNumber = "115",
//                        MajorShortCode = "CIS"
//                    };

//                    await controller.Create(course);

//                    context.SaveChanges();
//                }

//                // Use a separate instance of the context to verify correct data was saved to database
//                using (var context = new AppDbContext(options))
//                {
//                    var sut = new CoursesController(context);

//                    await sut.DeleteConfirmed(1);
//                    await context.SaveChangesAsync();

//                    Assert.Equal(0, context.Courses.Count());
                    
//                }
//            }
//            finally
//            {
//                connection.Close();
//            }
//        }

//        [Fact]
//        public async void NotAddIncompleteCourseToDatabase()
//        {
//            // In-memory database only exists while the connection is open
//            var connection = new SqliteConnection("DataSource=:memory:");
//            connection.Open();

//            try
//            {
//                var options = new DbContextOptionsBuilder<AppDbContext>()
//                    .UseSqlite(connection)
//                    .Options;

//                // Create the schema in the database
//                using (var context = new AppDbContext(options))
//                {
//                    context.Database.EnsureCreated();
//                }

//                // Run the test against one instance of the context
//                using (var context = new AppDbContext(options))
//                {
//                    var sut = new CoursesController(context);

//                    var course = new Course
//                    {
//                        //Major = "Computer Information Science",
//                        CourseTitle = "Big Ideas of Computer Science",
//                        CourseDescription = "This course introduces and explores seven “big ideas” of computer science. Students will develop computational thinking skills vital for success across all disciplines, " +
//                        "including algorithmic creativity, data abstraction, and modeling and simulation. ",
//                        CourseImageThumbnailUrl = "",
//                        CourseNumber = "115",
//                        MajorShortCode = "CIS"
//                    };

//                    await Assert.ThrowsAsync<DbUpdateException>(() => sut.Create(course));

//                }

//            }
//            finally
//            {
//                connection.Close();
//            }
//        }


//        [Fact]
//        public async void ReturnViewWhenInvalidModelState()
//        {
//            // In-memory database only exists while the connection is open
//            var connection = new SqliteConnection("DataSource=:memory:");
//            connection.Open();

//            try
//            {
//                var options = new DbContextOptionsBuilder<AppDbContext>()
//                    .UseSqlite(connection)
//                    .Options;

//                // Create the schema in the database
//                using (var context = new AppDbContext(options))
//                {
//                    context.Database.EnsureCreated();
//                }

//                // Run the test against one instance of the context
//                using (var context = new AppDbContext(options))
//                {
//                    var sut = new CoursesController(context);

//                    sut.ModelState.AddModelError("x", "Test Error");

//                    var course = new Course
//                    {
//                        //Major = "Computer Information Science",
//                        CourseTitle = "Big Ideas of Computer Science",
//                        CourseDescription = "This course introduces and explores seven “big ideas” of computer science. Students will develop computational thinking skills vital for success across all disciplines, " +
//                        "including algorithmic creativity, data abstraction, and modeling and simulation. ",
//                        CourseImageThumbnailUrl = "",
//                        CourseNumber = "115",
//                        MajorShortCode = "CIS"
//                    };

//                    IActionResult result = await sut.Create(course);

//                    ViewResult viewResult = Assert.IsType<ViewResult>(result);

//                    var model = Assert.IsType<Course>(viewResult.Model);

//                    Assert.Equal(course.CourseTitle, model.CourseTitle);


//                }

//            }
//            finally
//            {
//                connection.Close();
//            }
//        }

//    }
//}
