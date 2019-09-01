using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PeerTutor.Models;
using PeerTutor.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PeerTutor.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ICourseRepository _courseRepository;

        public HomeController(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
            
        }


        // GET: /<controller>/
        [AllowAnonymous]
        public IActionResult Index()
        {

           if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Menu");
            }

            return View();
        }

        
        public IActionResult Menu()
        {

            var courses = _courseRepository.GetCourses().OrderBy(c => c.CourseNumber);

            var homeViewModel = new HomeViewModel()
            {
                Title = "Course Menu",
                Courses = courses
            };

            return View(homeViewModel);
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Faq()
        {
            return View();
        }


    }
}
