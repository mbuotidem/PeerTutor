using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeerTutor.Models;
using PeerTutor.ViewModels;

namespace PeerTutor.Controllers
{
    public class UsersController : Controller
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reviewsFor = _context.Reviews
                .Include(s => s.Reviewed)
                .Include(c => c.Reviewer)
                .Where(c => c.RevieweeId == id);

            var reviewsBy =  _context.Reviews
                .Include(s => s.Reviewed)
                .Include(c => c.Reviewer)
                .Where(c => c.ReviewerId == id);

            var user = await _context.Users
                .Include(s => s.ReviewsBy)
                .Include(s => s.ReviewsFor)
                .FirstOrDefaultAsync(m => m.Id == id);

            var model = new UserViewModel
            {
                UserName = user.UserName,
                Photo = user.Photo,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ClassYear = user.ClassYear,
                RateCount = user.RateCount,
                RateTotal = user.RateTotal,
                ReviewsFor = reviewsFor.ToList(),
                ReviewsBy = reviewsBy.ToList()



            };
            return View(model);
        }
    }
}