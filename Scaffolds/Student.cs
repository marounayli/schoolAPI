using System;
using System.Collections.Generic;

namespace AppDist.Scaffolds
{
    public class Student : LinkedResourceBaseDto
    {
        public Student()
        {
            Coursemembership = new HashSet<Coursemembership>();
        }

        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<Coursemembership> Coursemembership { get; set; }
    }
}
