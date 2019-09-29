using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PeerTutor.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Review Date")]
        public DateTime ReviewDate { get; set; }

        [Required]
        [Display(Name = "Review Content")]
        public string ReviewContent { get; set; }

        [Required]
        [Display(Name = "Review By")]
        public string ReviewerId { get; set; }
        public ApplicationUser Reviewer { get; set; }

        [Required]
        [Display(Name = "Review For")]
        public string RevieweeId { get; set; }

        public ApplicationUser Reviewed { get; set; }
    }
}
