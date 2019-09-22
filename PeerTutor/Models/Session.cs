using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PeerTutor.Models
{
    public class Session : IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        public string Topic { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Display(Name ="Start Time")]
        public DateTime StartTime { get; set; }

        [Required]
        [Display(Name = "End Time")]
        public DateTime EndTime { get; set; }

        [Required]
        [Display(Name = "Booker")]
        public string BookerId { get; set; }
        public ApplicationUser Booker { get; set; }

        [Required]
        [Display(Name = "Booked With")]
        public string BookedId { get; set; }

        public ApplicationUser Booked { get; set; }

        public int CourseId { get; set; }

        [Display(Name ="Course")]
        public virtual Course Course { get; set; }

        public string Location { get; set; }

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            if (EndTime < StartTime)
            {
                yield return new ValidationResult("End Time must be greater than Start Time");
            }

            TimeSpan duration = EndTime - StartTime;

            if (duration.TotalMinutes < 60)
            {
                yield return new ValidationResult("Minimum booking length is 1 hour");
            }
        }
    }
}
