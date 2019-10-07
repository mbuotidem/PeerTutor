using PeerTutor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeerTutor.ViewModels
{
    public class UserViewModel
    {
        public string UserName { get; set; }

        public string Photo { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int ClassYear { get; set; }

        public int RateCount { get; set; }

        public int RateTotal { get; set; }

        public virtual ICollection<Review> ReviewsFor { get; set; }

        public virtual ICollection<Review> ReviewsBy { get; set; }
    }
}
