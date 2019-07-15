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
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ICourseRepository _courseRepository;

        public HomeController(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
            
        }

        //#region Protected Members
        ///// <summary>
        ///// The scooped Application context
        ///// </summary>

        //protected AppDbContext mContext;

        ///// <summary>
        ///// The manager for handling user creation, deletion, searching, roles etc..
        ///// </summary>

        //protected UserManager<ApplicationUser> mUserManager;

        ///// <summary>
        ///// The manager for handling signing in and out for our users
        ///// </summary>

        //protected SignInManager<ApplicationUser> mSignInManager;
        //#endregion

        //#region Constructor

        ///// <summary>
        ///// Default constructor
        ///// </summary>
        ///// <param name="context">The injected context</param>

        //public HomeController(ICourseRepository courseRepository, 
        //    AppDbContext context, 
        //    UserManager<ApplicationUser> userManager, 
        //    SignInManager<ApplicationUser> signInManager)
        //{
        //    _courseRepository = courseRepository;
        //    mContext = context;
        //    mUserManager = userManager;
        //    mSignInManager = signInManager;

        //}

        //#endregion
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

        //[Route("create")]
        //public async Task<IActionResult> CreateUserAsync()
        //{
        //    await Task.Delay(0);
        //    return Content("User was created", "text/html");
        //}
    }
}
