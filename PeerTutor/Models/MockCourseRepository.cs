﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeerTutor.Models
{
    public class MockCourseRepository : ICourseRepository
    {
        private List<Course> _courses;

        public MockCourseRepository()
        {
            if (_courses == null)
            {
                InitializeCourses();
            }
        }

        private void InitializeCourses()
        {
            _courses = new List<Course>
                {
                    new Course {Id = 1, Major = "Computer Information Science", CourseTitle = "Big Ideas of Computer Science", CourseDescription = "This course introduces and explores seven “big ideas” of computer science. Students will develop computational thinking skills vital for success across all disciplines, including algorithmic creativity, data abstraction, and modeling and simulation. ", CourseImageThumbnailUrl = "" , CourseNumber="115", MajorShortCode = "CIS" },
                    new Course {Id = 2, Major = "Computer Information Science", CourseTitle = "Introduction to Programming", CourseDescription = "This course provides conceptual and logical tools for students planning to major in a computing-based major. Programming in a high-level language such as C++, Python, or Java, and the development of skills in abstraction, problem-solving, and algorithmic thinking are emphasized. ", CourseImageThumbnailUrl = "" , CourseNumber="121", MajorShortCode = "CIS" },
                    new Course {Id = 3, Major = "Computer Information Science", CourseTitle = "Data Structures", CourseDescription = "This course is a continuation of CIS 121. Students develop a basic knowledge of programming skills and object-oriented concepts, and use fundamental data structures such as lists, stacks, queues, and trees. ", CourseImageThumbnailUrl = "" , CourseNumber="122", MajorShortCode = "CIS" },
                    new Course {Id = 4, Major = "Computer Information Science", CourseTitle = "Algorithms", CourseDescription = "This course builds on CS 122 (Data Structures) with coverage of advanced data structures and associated algorithms, including trees, graphs, hashing, searching, priority queues, and memory management. ", CourseImageThumbnailUrl = "" , CourseNumber="223", MajorShortCode = "CIS" },
                    new Course {Id = 5, Major = "Information Technology", CourseTitle = "Business Application Programming", CourseDescription = "Business application development using a non-object oriented programming language. ", CourseImageThumbnailUrl = "" , CourseNumber="310", MajorShortCode = "IT" },
                    new Course {Id = 6, Major = "Information Technology", CourseTitle = "Machine Structures and Operating Systems", CourseDescription = "Introduction to computer hardware, Boolean logic, digital circuits, data representations, digital arithmetic, digital storage, performance metrics, pipelining, memory hierarchy, and I/O; ", CourseImageThumbnailUrl = "" , CourseNumber="320", MajorShortCode = "IT" },
                    new Course {Id = 7, Major = "Information Technology", CourseTitle = "Introduction to Database Systems", CourseDescription = "Introduction to database systems, entity relationship models, relational algebra, database design, data modeling, normalization, and conversion of business rules into relational model. ", CourseImageThumbnailUrl = "" , CourseNumber="340", MajorShortCode = "IT" },
                    new Course {Id = 8, Major = "Information Technology", CourseTitle = "Introduction to Networking", CourseDescription = "This course covers basic concepts related to computer networking. Topics addressed will include the OSI model, the Internet model, network management, network protocols and data security.  ", CourseImageThumbnailUrl = "" , CourseNumber="360", MajorShortCode = "IT" }
                };
        }


        public Course GetCourseById(int courseId)
        {
            return _courses.FirstOrDefault(c => c.Id == courseId);
        }

        public IEnumerable<Course> GetCourses()
        {
            return _courses;
        }
    }
}

