using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AppDist.Scaffolds;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Authorization;

namespace AppDist.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Student")]
    public class StudentsController : ControllerBase
    {

        private string base_url = "http://localhost:5000/Student/";
        private readonly schoolapiContext context;

        private LinkGenerator _linkgenerator;

        public StudentsController(schoolapiContext context, LinkGenerator linkGenerator)
        {
            this.context = context;
            this._linkgenerator = linkGenerator;
        }

        [HttpGet]
        [Route("getcourses/{id}")]
        async public Task<IActionResult> GetAllCoursesForStudent([FromRoute] string id)
        {
            var courses = await context.Student.Where(x => x.Id == id).Select(x => x.Coursemembership.Select(y => y.Course.CourseCode))
            .ToListAsync();
            return Ok(courses);
        }
        [HttpGet]
        [Route("all")]
        public IEnumerable<Student> GetAllStudents()
        {
            var students = context.Student.ToList();
            for (int i = 0; i < students.Count; ++i)
            {
                students[i] = CreateLinksForStudent(students[i]);
            }
            return students;
        }


        [HttpGet]
        [Route("byid/{id}")]
        public Student GetStudentByID(string id)
        {
            var student = context.Student.Where(s => s.Id == id).First();
            return CreateLinksForStudent(student);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddStudent([FromBody] Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            context.Student.Add(student);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStudentByID), new { id = student.Id }, CreateLinksForStudent(student));
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteStudent(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var student = await context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            context.Student.Remove(student);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateStudent([FromRoute] string id, [FromBody] Student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != student.Id)
            {
                return BadRequest();
            }

            context.Entry(student).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!context.Student.Any(s => student.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Accepted(CreateLinksForStudent(student));
        }

        private Student CreateLinksForStudent(Student student)
        {
            student.Links.Add(
                new LinkModel(base_url + "byid/" + student.Id,
                "Get student by id",
                "GET"));

            student.Links.Add(
                new LinkModel(base_url + "getcourses/" + student.Id,
                "Get all courses for the student with ID = " + student.Id,
                "GET"));

            student.Links.Add(
                new LinkModel(base_url + "/all",
                "Get all students = " + student.Id,
                "GET"));

            student.Links.Add(
                new LinkModel(base_url + "add",
                "Add a new student",
                "POST"));

            student.Links.Add(
                new LinkModel(base_url + "update/" + student.Id,
                "Update student with the ID = " + student.Id,
                "PUT"));

            student.Links.Add(
                new LinkModel(base_url + "delete" + "/" + student.Id,
                "Delete Student with ID = " + student.Id,
                "DELETE"));

            return student;
        }

    }
}
