using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PeerTutor.Models;
using PeerTutor.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PeerTutor.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICourseRepository _courseRepository;


        public HomeController(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {

            var courses = _courseRepository.GetCourses().OrderBy(c => c.CourseNumber);

            var homeViewModel = new HomeViewModel()
            {
                Title = "Courses",
                Courses = courses.ToList()
            };

            return View(homeViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
