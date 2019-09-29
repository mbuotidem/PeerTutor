using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeerTutor.ViewModels
{
    public class EditReviewAndRatingViewModel
    {
        public EditReviewAndRatingViewModel()
        {
            ReviewDate = DateTime.Now;
        }
        public int RateId { get; set; }
        public int Rate { get; set; }
        public int ReviewId { get; set; }

        public DateTime ReviewDate { get; set; }

        public string ReviewContent { get; set; }

        public string ReviewerId { get; set; }

        public string RevieweeId { get; set; }
    }
}
