using HXQ.NWBC_Assignment05_v50.Data.Context;
using HXQ.NWBC_Assignment05_v50.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HXQ.NWBC_Assignment05_v50.API.Controllers
{
    #region Version1.0
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    //[Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CoursesV1Controller : ControllerBase
    {
        private readonly SchoolContext db;
        public CoursesV1Controller(SchoolContext db)
        {
            this.db = db;
        }
        //Test version
        [HttpGet("get-test-versioning")]
        public async Task<IActionResult> GetTestVersioning()
        {
            return Ok("Đây là CoursesController v1");
        }

        // GET: api/<CoursesController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var courses = await db.Courses.Include(s => s.Students).ToListAsync();
            return Ok(courses);
        }

        // GET api/<CoursesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var course = await db.Courses.Include(s => s.Students).FirstOrDefaultAsync(x => x.Id == id);
            return course != null ? Ok(course) : NotFound($"Không tìm thấy khóa học có id {id}");
        }

        // POST api/<CoursesController>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add([FromBody] Course course)
        {
            db.Courses.Add(course);
            await db.SaveChangesAsync();
            return CreatedAtAction(nameof(Add), new { id = course.Id }, course);
        }

        // PUT api/<CoursesController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] Course course)
        {
            var courseToUpdate = await db.Courses.FindAsync(id);
            if (courseToUpdate != null) return NotFound("$\"Không tìm thấy khóa học có id {id}");

            courseToUpdate.Title = course.Title;
            courseToUpdate.Description = course.Description;
            await db.SaveChangesAsync();
            return Ok(courseToUpdate);
        }

        // DELETE api/<CoursesController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var courseToDelete = await db.Courses.FindAsync(id);
            if (courseToDelete != null) return NotFound("$\"Không tìm thấy khóa học có id {id}");

            db.Courses.Remove(courseToDelete);
            await db.SaveChangesAsync();
            return NoContent();
        }

        //Thêm học sinh vào một khóa học 
        //Note: ở đây từng xảy ra lỗi 500 vòng lặp tuần hoàn giữa student và course, do student có danh sách course và course cũng có danh sách student, đã sửa bằng cách thêm jsonignore ở course.
        [HttpPost("{courseId}/students/{studentId}")]
        public async Task<IActionResult> AddAStudentToCourse(int courseId, int studentId)
        {
            var course = await db.Courses.Include(s => s.Students).FirstOrDefaultAsync(x => x.Id == courseId);
            if (course == null) return NotFound($"Không tìm thấy khóa học có id {courseId}");
            var student = await db.Students.FindAsync(studentId);
            if (student == null) return NotFound($"Không tìm thấy học sinh có id {studentId}");
            if (!course.Students.Contains(student))
            {
                course.Students.Add(student);
                await db.SaveChangesAsync();
            }
            return Ok(course);
        }
    }
    #endregion

    #region Version2.0

    /*Note: api v2 thử không có authorize, ko phải lỗi, là tính năng =))*/

    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    //[Route("api/[controller]")]
    [ApiController]
    public class CoursesV2Controller : ControllerBase
    {
        private readonly SchoolContext db;
        public CoursesV2Controller(SchoolContext db)
        {
            this.db = db;
        }
        //Test version
        [HttpGet("get-test-versioning")]
        public async Task<IActionResult> GetTestVersioning()
        {
            return Ok("Đây là CoursesController v2");
        }

        // GET: api/<CoursesController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var courses = await db.Courses.Include(s => s.Students).ToListAsync();
            return Ok(courses);
        }

        // GET api/<CoursesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var course = await db.Courses.Include(s => s.Students).FirstOrDefaultAsync(x => x.Id == id);
            return course != null ? Ok(course) : throw new KeyNotFoundException();
        }

        // POST api/<CoursesController>
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Course course)
        {
            db.Courses.Add(course);
            await db.SaveChangesAsync();
            return CreatedAtAction(nameof(Add), new { id = course.Id }, course);
        }

        // PUT api/<CoursesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Course course)
        {
            var courseToUpdate = await db.Courses.FindAsync(id);
            if (courseToUpdate != null) throw new KeyNotFoundException();

            courseToUpdate.Title = course.Title;
            courseToUpdate.Description = course.Description;
            await db.SaveChangesAsync();
            return Ok(courseToUpdate);
        }

        // DELETE api/<CoursesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var courseToDelete = await db.Courses.FindAsync(id);
            if (courseToDelete != null) throw new KeyNotFoundException();

            db.Courses.Remove(courseToDelete);
            await db.SaveChangesAsync();
            return NoContent();
        }

        //Thêm học sinh vào một khóa học 
        //Note: ở đây từng xảy ra lỗi 500 vòng lặp tuần hoàn giữa student và course, do student có danh sách course và course cũng có danh sách student, đã sửa bằng cách thêm jsonignore ở course.
        [HttpPost("{courseId}/students/{studentId}")]
        public async Task<IActionResult> AddAStudentToCourse(int courseId, int studentId)
        {
            var course = await db.Courses.Include(s => s.Students).FirstOrDefaultAsync(x => x.Id == courseId);
            if (course == null) throw new KeyNotFoundException();
            var student = await db.Students.FindAsync(studentId);
            if (student == null) throw new KeyNotFoundException();
            if (!course.Students.Contains(student))
            {
                course.Students.Add(student);
                await db.SaveChangesAsync();
            }
            return Ok(course);
        }
    } 
    #endregion
}
