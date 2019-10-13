using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeerTutor.ViewModels
{
    public class SessionViewModel
    {
        public int Id { get; set; }

        public string Topic { get; set; }

        public string Start { get; set; }

        public string End { get; set; }
    }
}
