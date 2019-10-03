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
        public ApplicationUser()
        {
            
        }
        public enum MajorType
        {
            [Display(Name = "Computer Information Technology")] CIT,
            [Display(Name = "Computer Information Science")] CIS,
            [Display(Name = "Management Information Systems")] MIS,
            [Display(Name = "M.S Information Technology")] IT,
            [Display(Name = "M.S Data Science")] DS,


        }

        public string Photo { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Major { get; set; }

        public int ClassYear { get; set; }

        public ICollection<CourseTutor> CourseTutors { get; set; }

        public ICollection<Session> BookedBy { get; set; }

        public ICollection<Session> BookedWith { get; set; }

        public int RateCount
        {
            //get {
            //    if (Ratings != null)
            //    {
            //        return Ratings.Count;
            //    }
            //    return 0;
            //    }
            //get { return (Ratings.Count > 0 ? Ratings.Count : 0); }
            //get { return Ratings.Count; }
            get { return ReviewsFor.Count; }
        }

        public int RateTotal
        {
            get
            {
                //    if (Ratings != null)
                //    {
                //        return Ratings.Sum(m => m.Stars);
                //    }

                //    return 0;
                //return (Ratings.Sum(m => m.Stars) > 0 ? Ratings.Sum(m => m.Stars) : 0);
                //return (Ratings.Sum(m => m.Stars));
                return (ReviewsFor.Sum(m => m.Stars));
            }
        }


        public virtual ICollection<Review> ReviewsFor { get; set; }

        public virtual ICollection<Review> ReviewsBy { get; set; }

    }
}
