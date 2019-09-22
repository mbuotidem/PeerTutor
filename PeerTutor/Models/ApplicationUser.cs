using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PeerTutor.Models
{
    /// <summary>
    /// The user data and profile for our application
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public enum MajorType
        {
            [Display(Name = "Computer Information Technology")] CIT,
            [Display(Name = "Computer Information Science")] CIS,
            [Display(Name = "Management Information Systems")] MIS,
            [Display(Name = "M.S Information Technology")] IT,
            [Display(Name = "M.S Data Science")] DS,


        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Major { get; set; }

        public int ClassYear { get; set; }

        public ICollection<CourseTutor> CourseTutors { get; set; }

        public ICollection<Session> BookedBy { get; set; }

        public ICollection<Session> BookedWith { get; set; }

    }
}
