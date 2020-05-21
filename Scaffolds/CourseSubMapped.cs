using System;
namespace AppDist.Scaffolds
{
    [Serializable]
    public partial class CourseSubMapped
    {

        private string courseName { get; set; }
        private string studentname { get; set; }
        public CourseSubMapped(string courseName, string studentname)
        {
            this.courseName = courseName;
            this.studentname = studentname;
        }
    }
}