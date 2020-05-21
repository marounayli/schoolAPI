using System;
using System.Collections.Generic;

namespace AppDist.Scaffolds
{
    public partial class Course : LinkedResourceBaseDto
    {
        public Course()
        {
            Coursemembership = new HashSet<Coursemembership>();
        }

        public string Id { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string TeacherId { get; set; }

        public virtual Teacher Teacher { get; set; }
        public virtual ICollection<Coursemembership> Coursemembership { get; set; }
    }
}
