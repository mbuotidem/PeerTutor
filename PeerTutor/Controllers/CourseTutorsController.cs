using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PeerTutor.Extensions;
using PeerTutor.Models;

namespace PeerTutor.Controllers
{
    public class CourseTutorsController : Controller
    {
        private readonly AppDbContext _context;
      
        public CourseTutorsController(AppDbContext context)
        {
            _context = context;
            
        }

        // GET: CourseTutors
        public async Task<IActionResult> Index()
        {
            //Only show expertise (coursetutors) for the currently logged in user
            var appDbContext = _context.CourseTutors.Include(c => c.Course).Include(c => c.User).Where(c => c.UserId == this.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString());
            return View(await appDbContext.ToListAsync());
        }

        // GET: CourseTutors/Details/5
        public async Task<IActionResult> Details(string id, int? courseId)
        {
            
            if (id == null || courseId == null)
            {
                return NotFound();
            }

            var courseTutor = await _context.CourseTutors
                .Include(c => c.Course)
                .Include(c => c.User)
                .Where(c => c.UserId == this.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString()) //only show for currently logged in user
                .Where(c => c.CourseId == courseId)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (courseTutor == null)
            {
                return RedirectToAction("Index").WithWarning("You were redirected!", "The action you requested was not allowed and thus could not be completed.");
            }
            return View(courseTutor);
        }

        // GET: CourseTutors/Create
        public IActionResult Create()
        {
            //ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id");
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "CourseTitle");
            ViewData["UserId"] = this.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();

            return View();
        }

        // POST: CourseTutors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Grade,UserId,CourseId")] CourseTutor courseTutor)
        {
            if (ModelState.IsValid)
            {

                _context.Add(courseTutor);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    if (ex != null)
                    {
                        return RedirectToAction("Create").WithWarning("You tried to add a course you've already added.",  "Please select a different course.");
                    }
                    //return View(courseTutor);
                    //This either returns a error string, or null if it can’t handle that error
                    
                    throw; //couldn’t handle that error, so rethrow
                }
                
                return RedirectToAction(nameof(Index));
            }
            //ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id", courseTutor.CourseId);
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", courseTutor.UserId);
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "CourseTitle");
            ViewData["UserId"] = this.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();

            return View(courseTutor);
        }

        // GET: CourseTutors/Edit/5
        public async Task<IActionResult> Edit(string id, int? courseId)
        {
            if (id == null || courseId == null)
            {
                return NotFound();
            }

            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();

            if (id != userId)
            {

                return RedirectToAction("Index").WithWarning("You were redirected!", "The action you requested was invalid!");
            }

            var courseTutor = await _context.CourseTutors.FindAsync(id, courseId);
            if (courseTutor == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id", courseTutor.CourseId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", courseTutor.UserId);
            return View(courseTutor);
        }

        // POST: CourseTutors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Grade,UserId,CourseId")] CourseTutor courseTutor)
        {
            if (id != courseTutor.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(courseTutor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseTutorExists(courseTutor.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id", courseTutor.CourseId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", courseTutor.UserId);
            return View(courseTutor);
        }

        // GET: CourseTutors/Delete/5
        public async Task<IActionResult> Delete(string id, int? courseId)
        {
            if (id == null || courseId == null)
            {
                return NotFound();
            }
            
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();

            if (id != userId)
            {

                return RedirectToAction("Index").WithWarning("You were redirected!", "The action you hit has bounced you back to Index!");
            }
            var courseTutor = await _context.CourseTutors
                .Include(c => c.Course)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (courseTutor == null)
            {
                return NotFound();
            }

            return View(courseTutor);
        }

        // POST: CourseTutors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id, int? courseId)
        {
            var courseTutor = await _context.CourseTutors.FindAsync(id, courseId);
            _context.CourseTutors.Remove(courseTutor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseTutorExists(string id)
        {
            return _context.CourseTutors.Any(e => e.UserId == id);
        }

        [HttpGet]
        public async Task<IActionResult> ShowExperts(int courseId)
        {
            //var appDbContext = _context.CourseTutors.Include(c => c.Course).Include(c => c.User).Where(c => c.UserId == this.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString());
            var courseTutors = await _context.CourseTutors
                .Include(c => c.User)
                //nclude(c => c.User.Ratings)
                .Include(c => c.User.ReviewsFor)
                .Include(c => c.Course)
                .Where(c => c.CourseId == courseId).ToListAsync();


            //var courseTutor = await _context.CourseTutors
            //    .Include(c => c.Course)
            //    .Include(c => c.User)
            //    .Where(c => c.UserId == this.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString()) //only show for currently logged in user
            //    .Where(c => c.CourseId == courseId)
            //    .FirstOrDefaultAsync(m => m.UserId == this.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString());

            //courseTutors.Remove(courseTutor);

            ViewData["CourseTitle"] = _context.Courses.Find(courseId).CourseTitle;
                

            return View("Experts", courseTutors);
        }
    }
}
