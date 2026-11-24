using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AcademicPortal.API.Data;

namespace AcademicPortal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CoursesController : ControllerBase
    {
        private readonly AcademicPortalDbContext _context;

        public CoursesController(AcademicPortalDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCourses()
        {
            var courses = await _context.Courses
                .Include(c => c.LabModules)
                .ThenInclude(l => l.Assignments)
                .ToListAsync();

            return Ok(courses);
        }

        [HttpGet("{courseId:guid}/labs")]
        public async Task<IActionResult> GetCourseLabs(Guid courseId)
        {
            var labs = await _context.LabModules
                .Where(l => l.CourseId == courseId)
                .Include(l => l.Assignments)
                .OrderBy(l => l.LabNumber)
                .ToListAsync();

            return Ok(labs);
        }
    }
}
