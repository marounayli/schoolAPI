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

namespace AppDist.Controllers
{
    [ApiController]
    [Route("Course")]
    public class CourseController : ControllerBase
    {

        private string base_url = "http:localhost:5000/Course/";
        private readonly schoolapiContext context;
        private LinkGenerator _linkgenerator;
        public CourseController(schoolapiContext context, LinkGenerator linkGenerator)
        {
            this.context = context;
            this._linkgenerator = linkGenerator;
        }

        [HttpGet]
        [Route("all")]
        public IEnumerable<Course> getAllCourses()
        {
            var courses = context.Course.ToList();
            for (int i = 0; i < courses.Count; ++i)
            {
                courses[i] = CreateLinksForCourse(courses[i]);
            }
            return courses;
        }

        [HttpGet]
        [Route("byid/{id}")]
        public async Task<IActionResult> GetCourseByID(string id)
        {
            var course = await context.Course.FindAsync(id);
            return Ok(CreateLinksForCourse(course));
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddCourse([FromBody] Course course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            context.Course.Add(course);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCourseByID), new { id = course.Id }, course);
        }

        [HttpDelete]
        [Route("delete{id}")]
        public async Task<IActionResult> DeleteCourse(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var course = await context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            context.Course.Remove(course);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateCourse([FromRoute] string id, [FromBody] Course course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != course.Id)
            {
                return BadRequest("URL ID DIFFERENT THAN PAYLOAD ID");
            }

            context.Entry(course).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!context.Course.Any(s => course.Id == id))
                {
                    return NotFound("Course with ID" + id + "was not found");
                }
                else
                {
                    throw;
                }
            }

            return Accepted(CreateLinksForCourse(course));
        }

        private Course CreateLinksForCourse(Course course)
        {
            course.Links.Add(
                new LinkModel(base_url + "byid/" + course.Id,
                "Get Course By ID",
                "GET"));

            course.Links.Add(
                new LinkModel(base_url + "all",
                "Get all courses",
                "GET"));

            course.Links.Add(
                new LinkModel(base_url + "add",
                "Get the student with ID = " + course.Id,
                "POST"));

            course.Links.Add(
                new LinkModel(base_url + "update" + course.Id,
                "Update course with the ID = " + course.Id,
                "PUT"));

            course.Links.Add(
                new LinkModel(base_url + "delete/" + course.Id,
                "Delete Course with ID = " + course.Id,
                "DELETE"));

            return course;
        }
    }
}