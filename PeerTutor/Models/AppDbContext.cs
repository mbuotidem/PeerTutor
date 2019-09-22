using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PeerTutor.Models;

namespace PeerTutor.Models
{
    public class AppDbContext: IdentityDbContext<ApplicationUser> 
    {
        public AppDbContext()
        {

        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        // Specify DbSet properties etc
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CourseTutor>()
                .HasKey(ct => new { ct.UserId, ct.CourseId });

            modelBuilder.Entity<CourseTutor>()
                .HasOne(ct => ct.User)
                .WithMany(u => u.CourseTutors)
                .HasForeignKey(ct => ct.UserId);

            modelBuilder.Entity<CourseTutor>()
                .HasOne(ct => ct.Course)
                .WithMany(c => c.CourseTutors)
                .HasForeignKey(ct => ct.CourseId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.BookedBy)
                .WithOne(r => r.Booker)
                .HasForeignKey(r => r.BookerId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.BookedWith)
                .WithOne(r => r.Booked)
                .HasForeignKey(r => r.BookedId);

            

        }

        public DbSet<Course> Courses { get; set; }

        public DbSet<CourseTutor> CourseTutors { get; set; }

        public DbSet<PeerTutor.Models.Session> Session { get; set; }
    }
}
