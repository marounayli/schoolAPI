using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AppDist.Scaffolds;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace AppDist.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TeacherController : ControllerBase
    {
        private readonly schoolapiContext context;
        private static string base_url = "http://localhost:5000/Teacher/";

        public TeacherController(schoolapiContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("all")]
        async public Task<IActionResult> GetAllTeachers()
        {
            var teachers = await context.Teacher.Select(x => CreateLinksForTeacher(x)).ToListAsync();
            return Ok(teachers);
        }

        [HttpGet]
        [Route("byid/{id}")]
        public async Task<IActionResult> GetbyID(string id)
        {
            var teacher = await context.Teacher.Where(x => x.Id == id).Select(x => CreateLinksForTeacher(x))
            .ToListAsync();
            return Ok(teacher);
        }


        [HttpGet]
        [Route("getcourses/{id}")]
        async public Task<IActionResult> GetAllCourses([FromRoute] string id)
        {
            var courses = await context.Teacher.Where(x => x.Id == id)
            .Select(x => x.Course.Select(x => x.CourseName))
            .ToListAsync();
            return Ok(courses);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddTeacher([FromBody] Teacher teacher)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            context.Teacher.Add(teacher);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetbyID), new { id = teacher.Id }, CreateLinksForTeacher(teacher));
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteTeacher(string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var teacher = await context.Teacher.FindAsync(id);
            if (teacher == null)
            {
                return NotFound();
            }

            context.Teacher.Remove(teacher);
            await context.SaveChangesAsync();

            return NoContent();
        }


        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateTeacher([FromRoute] string id, [FromBody] Teacher teacher)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != teacher.Id)
            {
                return BadRequest();
            }

            context.Entry(teacher).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!context.Teacher.Any(s => teacher.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Accepted(CreateLinksForTeacher(teacher));
        }

        static private Teacher CreateLinksForTeacher(Teacher teacher)
        {
            teacher.Links.Add(
                new LinkModel(base_url + "byid/" + teacher.Id,
                "Get teacher by id",
                "GET"));

            teacher.Links.Add(
                new LinkModel(base_url + "all",
                "Get all teachers",
                "GET"));

            teacher.Links.Add(
                new LinkModel(base_url + "get_courses/" + teacher.Id,
                "Add a new teacher",
                "GET"));

            teacher.Links.Add(
                new LinkModel(base_url + "update/" + teacher.Id,
                "Update teacher with ID = " + teacher.Id,
                "POST"));

            teacher.Links.Add(
                new LinkModel(base_url + "delete" + "/" + teacher.Id,
                "Delete Student with ID = " + teacher.Id,
                "PUT"));

            teacher.Links.Add(new LinkModel(base_url + "delete" + teacher.Id,
                    "Delete Teacher with ID = " + teacher.Id,
                    "DELETE"));

            return teacher;
        }


    }

}