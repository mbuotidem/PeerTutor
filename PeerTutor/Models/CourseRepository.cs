using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeerTutor.Models
{
    public class CourseRepository: ICourseRepository
    {
        private readonly AppDbContext _appDbContext;

        public CourseRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IEnumerable<Course> GetCourses()
        {
            return _appDbContext.Courses;
        }

        public Course GetCourseById(int courseId)
        {
            return _appDbContext.Courses.FirstOrDefault(c => c.Id == courseId);
        }
    }
}
