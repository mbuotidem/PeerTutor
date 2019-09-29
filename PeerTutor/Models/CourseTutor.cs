using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeerTutor.Models
{
    public class CourseTutor
    {
        public enum Grades
        {
            [Display(Name = "A")] A,
            [Display(Name = "B")] B,
            [Display(Name = "C")] C,
            [Display(Name = "D")] D,
            [Display(Name = "E")] E,


        }
        
        [Required]
        public Grades Grade { get; set; }

        //From User
        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }


        //From Course
        [Required]
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }

        
    }
}