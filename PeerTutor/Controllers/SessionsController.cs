using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PeerTutor.Extensions;
using PeerTutor.Models;
using Stripe;

namespace PeerTutor.Controllers
{
    public class SessionsController : Controller
    {
        private readonly AppDbContext _context;

        //private readonly string _stripeEmail;

        //private readonly string _stripeToken;

        public IConfiguration Configuration { get; }

        public SessionsController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
            //_stripeEmail = stripeEmail;
            //_stripeToken = stripeToken;

        }

        // GET: Sessions
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Session.Include(s => s.Booked).Include(s => s.Booker).Include(s => s.Course)
                .Where(c => (c.BookedId == this.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString() || c.BookerId == this.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString())); ;
            return View(await appDbContext.ToListAsync());
        }

        // GET: Sessions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var session = await _context.Session
                .Include(s => s.Booked)
                .Include(s => s.Booker)
                .Include(s => s.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (session == null)
            {
                return NotFound();
            }

            return View(session);
        }

        // GET: Sessions/Create
        public IActionResult Create(string bookedWith, int courseId)
        {
            if (bookedWith == this.User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString())
            {
                return RedirectToAction("ShowExperts","CourseTutors", new { courseId = courseId } ).WithWarning("You were redirected!", "You cannot book a tutoring session with yourself.");
            }
            //ViewData["BookedId"] = new SelectList(_context.Users, "Id", "FirstName");
            //ViewData["BookerId"] = new SelectList(_context.Users, "Id", "FirstName");
            ViewData["BookerId"] = User.FindFirst(ClaimTypes.NameIdentifier).Value.ToString();
            ViewData["BookedId"] = bookedWith;
            ViewData["CourseId"] = courseId;
            ViewBag.Secret = Configuration["MapsAPI"];
            ViewBag.Stripe = Configuration["StripePK"];
            return View();
        }

        // POST: Sessions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Topic,Description,StartTime,EndTime,BookerId,BookedId,CourseId,Location")] Session session, string stripeEmail, string stripeToken)
        {
            if (ModelState.IsValid)
            {
                //Charge(session,_stripeEmail, _stripeToken);

                var customers = new CustomerService();
                var charges = new ChargeService();

                var customer = customers.Create(new CustomerCreateOptions
                {
                    Email = stripeEmail,
                    Source = stripeToken
                });

                //Calculate duration. Fee is $10 per hour 
                var hourlyCharge = 10;
                TimeSpan duration = session.EndTime - session.StartTime;
                var chargeAmount = hourlyCharge / 60.0 * duration.TotalMinutes * 100; 
                // Get the payment token submitted by the form:

                var charge = charges.Create(new ChargeCreateOptions
                {
                    Amount = (long)chargeAmount,
                    Description = "Sample Charge",
                    Currency = "usd",
                    //CustomerId = customer.Id
                    Customer = customer.Id

                });


                if (charge.Status == "succeeded")
                {
                    _context.Add(session);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index)).WithSuccess("Session booked!", "Remember to prepare your questions in advance so your session will be uberproductive.");
                }

                //_context.Add(session);
                //await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
            }
            ViewData["BookedId"] = new SelectList(_context.Users, "Id", "FirstName", session.BookedId);
            ViewData["BookerId"] = new SelectList(_context.Users, "Id", "FirstName", session.BookerId);
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "CourseTitle", session.CourseId);
            ViewBag.Secret = Configuration["MapsAPI"];
            ViewBag.Stripe = Configuration["StripePK"];
            return View(session);
        }

        // GET: Sessions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var session = await _context.Session.FindAsync(id);
            if (session == null)
            {
                return NotFound();
            }
            ViewData["BookedId"] = new SelectList(_context.Users, "Id", "FirstName", session.BookedId);
            ViewData["BookerId"] = new SelectList(_context.Users, "Id", "FirstName", session.BookerId);
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "CourseTitle", session.CourseId);
            ViewBag.Secret = Configuration["MapsAPI"];
            return View(session);
        }

        // POST: Sessions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Topic,Description,StartTime,EndTime,BookerId,BookedId,CourseId,Location")] Session session)
        {
            if (id != session.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(session);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SessionExists(session.Id))
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
            ViewData["BookedId"] = new SelectList(_context.Users, "Id", "Id", session.BookedId);
            ViewData["BookerId"] = new SelectList(_context.Users, "Id", "Id", session.BookerId);
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "CourseDescription", session.CourseId);
            return View(session);
        }

        // GET: Sessions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var session = await _context.Session
                .Include(s => s.Booked)
                .Include(s => s.Booker)
                .Include(s => s.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (session == null)
            {
                return NotFound();
            }

            return View(session);
        }

        // POST: Sessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var session = await _context.Session.FindAsync(id);
            _context.Session.Remove(session);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SessionExists(int id)
        {
            return _context.Session.Any(e => e.Id == id);
        }

      
    }
}
