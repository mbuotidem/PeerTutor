using PeerTutor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeerTutor.ViewModels
{
    public class HomeViewModel
    {
        public string Title { get; set; }
        public List<Course> Courses { get; set; }
    }
}
