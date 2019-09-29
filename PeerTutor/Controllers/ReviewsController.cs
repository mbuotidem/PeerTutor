using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeerTutor.Models;
using PeerTutor.ViewModels;
using System.Linq;

namespace PeerTutor.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly AppDbContext _context;
        public ReviewsController(AppDbContext context)
        {
            _context = context;
        }
        // GET: Review
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Reviews.Include(s => s.Reviewed).Include(s => s.Reviewer)
                .Where(c => (c.ReviewerId == this.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString() || c.RevieweeId == this.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString()));
            return View(await appDbContext.ToListAsync());
        }

        // GET: Review/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .Include(s => s.Reviewer)
                .Include(s => s.Reviewed)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (review == null)
            {
                return NotFound();

            }

            return View(review);
        }

        // GET: Review/Create
        public ActionResult Create()
        {
            ViewBag.UserId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();
            return View();
        }

        // POST: Review/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Create([Bind("ReviewContent, RevieweeId, ReviewerId")] ReviewAndRatingViewModel userReview)
        {
            // TODO: Add insert logic here

            if (ModelState.IsValid)
            {

                try
                {
                    var review = new Review { ReviewContent = userReview.ReviewContent, ReviewerId = userReview.ReviewerId , RevieweeId = userReview.RevieweeId, ReviewDate = userReview.ReviewDate };

                    _context.Add(review);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    return View(ex.Message);
                }
            }

            ModelState.AddModelError("", "Error");
            return View(userReview);
        }

        // GET: Review/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Review/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Review/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Review/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public JsonResult PostRating(int rating, string userId)
        {
            //save data into the database

            StarRating rt = new StarRating { Stars = rating, UserId = userId };
            
            
            //save into the database 
            _context.Ratings.Add(rt);
            _context.SaveChanges();

            return Json("You rated this " + rating.ToString() + " star(s)");
        }
    }
}