using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeerTutor.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Major { get; set; }

        public string MajorShortCode { get; set; }

        public string CourseNumber { get; set; }
        public string CourseTitle { get; set; }
        public string CourseDescription { get; set; }
        public string CourseImageThumbnailUrl { get; set; }
    }
}


