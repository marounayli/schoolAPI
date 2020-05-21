using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AppDist.Scaffolds;
using Microsoft.EntityFrameworkCore;

namespace AppDist.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SubscriptionController : ControllerBase
    {

        private readonly schoolapiContext context;

        public SubscriptionController(schoolapiContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSubscriptions()
        {
            var result = await context.Coursemembership.Join(
                context.Student,
                c => c.StudentId,
                s => s.Id,
                (c, s) => new
                {
                    course = c.CourseId,
                    studentName = s.FirstName
                }
            ).Join(
                context.Course,
                r => r.course,
                c => c.Id,
                (r, c) => new { courseName = c.CourseName, studentName = r.studentName }
            ).ToListAsync();

            return Ok(result);
        }

        [HttpGet]
        [Route("bycourse/{id}")]
        public async Task<IActionResult> GetAllStudentByCourse(string id)
        {
            var students = await context.Coursemembership.Where(x => x.CourseId == id)
                        .Include(sub => sub.Student).Select(x => new { Student = x.Student }).ToListAsync();
            return Ok(students);

        }

        [HttpGet]
        [Route("bystudent/{id}")]

        public async Task<IActionResult> GetAllCoursesByStudent(string id)
        {
            var courses = await context.Coursemembership.Where(x => x.StudentId == id)
            .Include(sub => sub.Student).Select(x => new { Course = x.Course }).ToListAsync();

            return Ok(courses);
        }
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddSubscription([FromBody] Coursemembership sub)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            context.Coursemembership.Add(sub);
            await context.SaveChangesAsync();

            return CreatedAtAction("Successfully subscribed student", new { CourseID = sub.CourseId, StudentID = sub.StudentId });
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> RemoveSubscription([FromBody] Coursemembership sub)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            context.Coursemembership.Add(sub);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}