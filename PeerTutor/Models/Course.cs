using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PeerTutor.Models
{
    public class Course
    {
        public int Id { get; set; }
        [Required]
        public string Major { get; set; }
        [Required]
        public string MajorShortCode { get; set; }
        [Required]
        public string CourseNumber { get; set; }
        [Required]
        public string CourseTitle { get; set; }
        [Required]
        public string CourseDescription { get; set; }
        [Required]
        public string CourseImageThumbnailUrl { get; set; }

        public ICollection<CourseTutor> CourseTutors { get; set; }
    }
}


