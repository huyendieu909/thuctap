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
    public class StudentsV1Controller : ControllerBase
    {
        private readonly SchoolContext db;
        public StudentsV1Controller(SchoolContext db)
        {
            this.db = db;
        }
        //Test version
        [HttpGet("get-test-versioning")]
        public async Task<IActionResult> GetTestVersioning()
        {
            return Ok("Đây là StudentsController v1");
        }


        // GET: api/<StudentsController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await db.Students.Include(s => s.Courses).ToListAsync());
        }

        // GET api/<StudentsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var student = await db.Students.FirstOrDefaultAsync(s => s.Id == id);
            return student != null ? Ok(student) : NotFound($"Không tìm thấy học sinh có id {id}");
        }

        // POST api/<StudentsController>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add([FromBody] Student student)
        {
            db.Students.Add(student);
            await db.SaveChangesAsync();
            return CreatedAtAction(nameof(Add), new { id = student.Id }, student);
        }

        // PUT api/<StudentsController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] Student student)
        {
            var studentToUpdate = await db.Students.FindAsync(id);
            if (studentToUpdate == null) return NotFound("Không tìm thấy học sinh có id {id}");

            studentToUpdate.Name = student.Name;
            studentToUpdate.DateOfBirth = student.DateOfBirth;
            studentToUpdate.GradeId = student.GradeId;
            studentToUpdate.Grade = student.Grade;

            await db.SaveChangesAsync();
            return Ok(studentToUpdate);
        }

        // DELETE api/<StudentsController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var studentToDelete = await db.Students.FindAsync(id);
            if (studentToDelete == null) return NotFound("Không tìm thấy học sinh có id {id}");

            db.Students.Remove(studentToDelete);
            await db.SaveChangesAsync();
            return NoContent();
        }

        //Lấy toàn bộ khóa học của một học sinh
        [HttpGet("allcourses/{studentId}")]
        public async Task<IActionResult> GetAllCourses(int studentId)
        {
            var student = await db.Students.Include(c => c.Courses).FirstOrDefaultAsync(x => x.Id == studentId);
            if (student == null) return NotFound($"Không tìm thấy học sinh có id {studentId}");
            return Ok(student.Courses);
        }

        //Test throw global exception
        [HttpGet("get-student-throw-exception/{id}")]
        public async Task<IActionResult> GetStudentThrowException(int id)
        {
            throw new Exception("Global exception");
        }
    }
    #endregion

    #region Version2.0
    
    /*Note: api v2 thử không có authorize, ko phải lỗi, là tính năng =))*/
    
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    //[Route("api/[controller]")]
    [ApiController]
    
    public class StudentsV2Controller : ControllerBase
    {
        private readonly SchoolContext db;
        public StudentsV2Controller(SchoolContext db)
        {
            this.db = db;
        }
        //Test version
        [HttpGet("get-test-versioning")]
        public async Task<IActionResult> GetTestVersioning()
        {
            return Ok("Đây là StudentsController v2");
        }

        // GET: api/<StudentsController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await db.Students.Include(s => s.Courses).ToListAsync());
        }

        // GET api/<StudentsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var student = await db.Students.FirstOrDefaultAsync(s => s.Id == id);
            return student != null ? Ok(student) : throw new KeyNotFoundException();
        }

        // POST api/<StudentsController>
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Student student)
        {
            db.Students.Add(student);
            await db.SaveChangesAsync();
            return CreatedAtAction(nameof(Add), new { id = student.Id }, student);
        }

        // PUT api/<StudentsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Student student)
        {
            var studentToUpdate = await db.Students.FindAsync(id);
            if (studentToUpdate == null) throw new KeyNotFoundException();

            studentToUpdate.Name = student.Name;
            studentToUpdate.DateOfBirth = student.DateOfBirth;
            studentToUpdate.GradeId = student.GradeId;
            studentToUpdate.Grade = student.Grade;

            await db.SaveChangesAsync();
            return Ok(studentToUpdate);
        }

        // DELETE api/<StudentsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var studentToDelete = await db.Students.FindAsync(id);
            if (studentToDelete == null) throw new KeyNotFoundException();

            db.Students.Remove(studentToDelete);
            await db.SaveChangesAsync();
            return NoContent();
        }

        //Lấy toàn bộ khóa học của một học sinh
        [HttpGet("allcourses/{studentId}")]
        public async Task<IActionResult> GetAllCourses(int studentId)
        {
            var student = await db.Students.Include(c => c.Courses).FirstOrDefaultAsync(x => x.Id == studentId);
            if (student == null) throw new KeyNotFoundException();
            return Ok(student.Courses);
        }

        //Test throw global exception
        [HttpGet("get-student-throw-exception/{id}")]
        public async Task<IActionResult> GetStudentThrowException(int id)
        {
            throw new Exception("Global exception");
        }
    } 
    #endregion
}
