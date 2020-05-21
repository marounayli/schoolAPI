using System;
using System.Collections.Generic;

namespace AppDist.Scaffolds
{
    public partial class Coursemembership
    {
        public string StudentId { get; set; }
        public string CourseId { get; set; }

        public virtual Course Course { get; set; }
        public virtual Student Student { get; set; }
    }
}
