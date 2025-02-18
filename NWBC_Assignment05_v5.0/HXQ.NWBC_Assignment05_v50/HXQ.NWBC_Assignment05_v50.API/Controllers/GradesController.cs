using HXQ.NWBC_Assignment05_v50.Data.Context;
using HXQ.NWBC_Assignment05_v50.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HXQ.NWBC_Assignment05_v50.API.Controllers
{
    #region Version1.0
    //Version 1.0

    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    //[Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GradesV1Controller : ControllerBase
    {
        //Đoạn này gọi là dependency injection, gg để bt thêm info
        private readonly SchoolContext db;
        public GradesV1Controller(SchoolContext db)
        {
            this.db = db;
        }

        //Test version
        [Authorize]
        [HttpGet("get-test-versioning")]
        public async Task<IActionResult> GetTestVersioning()
        {
            return Ok("Đây là GradesController v1");
        }

        // GET: api/<GradesController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await db.Grades.Include(g => g.Students).ToListAsync());
        }

        // GET api/<GradesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var grade = await db.Grades.Include(g => g.Students).FirstOrDefaultAsync(s => s.Id == id);
            return grade != null ? Ok(grade) : NotFound($"Không tìm thấy lớp có id {id}");
        }

        // POST api/<GradesController>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add([FromBody] Grade grade)
        {
            db.Grades.Add(grade);
            await db.SaveChangesAsync();
            return CreatedAtAction(nameof(Add), new { id = grade.Id }, grade);
        }

        // PUT api/<GradesController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] Grade grade)
        {
            var gradeToUpdate = await db.Grades.FindAsync(id);
            if (gradeToUpdate == null) return NotFound($"Không tìm thấy lớp có id {id}");

            gradeToUpdate.Name = grade.Name;
            gradeToUpdate.Students = grade.Students;
            await db.SaveChangesAsync();
            return Ok(grade);
        }

        // DELETE api/<GradesController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var gradeToDelete = await db.Grades.FindAsync(id);
            if (gradeToDelete == null) return NotFound($"Không tìm thấy lớp có id {id}");

            db.Grades.Remove(gradeToDelete);
            await db.SaveChangesAsync();
            return NoContent();
        }

        //Lấy toàn bộ học sinh của một lớp
        [HttpGet("allstudents/{gradeId}")]
        public async Task<IActionResult> GetAllStudents(int gradeId)
        {
            var grade = await db.Grades.Include(s => s.Students).FirstOrDefaultAsync(x => x.Id == gradeId);
            if (grade == null) return NotFound($"Không tìm thấy lớp có id {gradeId}");
            return Ok(grade.Students);
        }


    }
    #endregion

    #region Version2.0
    //Version 2.0

    /*Note: api v2 thử không có authorize, ko phải lỗi, là tính năng =))*/

    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    //[Route("api/[controller]")]
    [ApiController]
    public class GradesV2Controller : ControllerBase
    {
        //Đoạn này gọi là dependency injection, gg để bt thêm info
        private readonly SchoolContext db;
        public GradesV2Controller(SchoolContext db)
        {
            this.db = db;
        }

        //Test version
        [HttpGet("get-test-versioning")]
        public async Task<IActionResult> GetTestVersioning()
        {
            return Ok("Đây là GradesController v2");
        }

        // GET: api/<GradesController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await db.Grades.Include(g => g.Students).ToListAsync());
        }

        // GET api/<GradesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var grade = await db.Grades.Include(g => g.Students).FirstOrDefaultAsync(s => s.Id == id);
            return grade != null ? Ok(grade) : throw new KeyNotFoundException();
        }

        // POST api/<GradesController>
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Grade grade)
        {
            db.Grades.Add(grade);
            await db.SaveChangesAsync();
            return CreatedAtAction(nameof(Add), new { id = grade.Id }, grade);
        }

        // PUT api/<GradesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Grade grade)
        {
            var gradeToUpdate = await db.Grades.FindAsync(id);
            if (gradeToUpdate == null) throw new KeyNotFoundException();

            gradeToUpdate.Name = grade.Name;
            gradeToUpdate.Students = grade.Students;
            await db.SaveChangesAsync();
            return Ok(grade);
        }

        // DELETE api/<GradesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var gradeToDelete = await db.Grades.FindAsync(id);
            if (gradeToDelete == null) throw new KeyNotFoundException();

            db.Grades.Remove(gradeToDelete);
            await db.SaveChangesAsync();
            return NoContent();
        }

        //Lấy toàn bộ học sinh của một lớp
        [HttpGet("allstudents/{gradeId}")]
        public async Task<IActionResult> GetAllStudents(int gradeId)
        {
            var grade = await db.Grades.Include(s => s.Students).FirstOrDefaultAsync(x => x.Id == gradeId);
            if (grade == null) throw new KeyNotFoundException();
            return Ok(grade.Students);
        }


    } 
    #endregion
}
