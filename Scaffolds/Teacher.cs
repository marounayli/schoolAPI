using System;
using System.Collections.Generic;

namespace AppDist.Scaffolds
{
    public partial class Teacher : LinkedResourceBaseDto
    {
        public Teacher()
        {
            Course = new HashSet<Course>();
        }

        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<Course> Course { get; set; }
    }
}
