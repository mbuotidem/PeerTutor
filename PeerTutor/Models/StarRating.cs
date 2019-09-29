using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PeerTutor.Models
{
    public class StarRating
    {
        [Key]
        public int RateId { get; set; }

        [Required]
        public int Stars { get; set; }

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }
}
