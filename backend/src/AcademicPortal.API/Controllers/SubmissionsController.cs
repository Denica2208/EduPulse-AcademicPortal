using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AcademicPortal.API.Data;
using AcademicPortal.API.Models;

namespace AcademicPortal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SubmissionsController : ControllerBase
    {
        private readonly AcademicPortalDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SubmissionsController(AcademicPortalDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        /// <summary>
        /// Student upload endpoint for laboratory project archives (.zip)
        /// </summary>
        [HttpPost("upload/{assignmentId:guid}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> UploadSolution(Guid assignmentId, [FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file was uploaded.");

            var assignment = await _context.Assignments.FindAsync(assignmentId);
            if (assignment == null)
                return NotFound("Assignment not found.");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (extension != ".zip" && extension != ".rar")
                return BadRequest("Only .zip and .rar archives are allowed.");

            if (file.Length > 25 * 1024 * 1024)
                return BadRequest("Maximum file size is 25 MB.");

            var studentIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(studentIdStr, out var studentId))
                return Unauthorized();

            // Check if already submitted -> overwrite or create new
            var existingSubmission = await _context.Submissions
                .FirstOrDefaultAsync(s => s.AssignmentId == assignmentId && s.StudentId == studentId);

            var uploadsFolder = Path.Combine(_env.ContentRootPath, "uploads", "submissions");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{assignmentId}_{studentId}_{Guid.NewGuid()}{extension}";
            var fullPath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var now = DateTime.UtcNow;
            var isLate = now > assignment.Deadline;

            if (existingSubmission != null)
            {
                existingSubmission.OriginalFileName = file.FileName;
                existingSubmission.StoredFilePath = fullPath;
                existingSubmission.FileSizeBytes = file.Length;
                existingSubmission.SubmittedAt = now;
                existingSubmission.IsLate = isLate;
                existingSubmission.Status = "Resubmitted";
            }
            else
            {
                var submission = new Submission
                {
                    AssignmentId = assignmentId,
                    StudentId = studentId,
                    OriginalFileName = file.FileName,
                    StoredFilePath = fullPath,
                    FileSizeBytes = file.Length,
                    SubmittedAt = now,
                    IsLate = isLate,
                    Status = "Submitted"
                };
                _context.Submissions.Add(submission);
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Solution successfully uploaded.", isLate, submittedAt = now });
        }

        /// <summary>
        /// Professor endpoint to evaluate and assign grades with feedback
        /// </summary>
        [HttpPost("{submissionId:guid}/grade")]
        [Authorize(Roles = "Professor")]
        public async Task<IActionResult> GradeSubmission(Guid submissionId, [FromBody] GradeDto dto)
        {
            var submission = await _context.Submissions
                .Include(s => s.Grade)
                .FirstOrDefaultAsync(s => s.Id == submissionId);

            if (submission == null)
                return NotFound("Submission not found.");

            if (dto.Score < 1.0m || dto.Score > 10.0m)
                return BadRequest("Score must be between 1.0 and 10.0.");

            var professorIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(professorIdStr, out var profId))
                return Unauthorized();

            if (submission.Grade != null)
            {
                submission.Grade.Score = dto.Score;
                submission.Grade.Feedback = dto.Feedback;
                submission.Grade.GradedAt = DateTime.UtcNow;
                submission.Grade.GradedById = profId;
            }
            else
            {
                var grade = new Grade
                {
                    SubmissionId = submissionId,
                    GradedById = profId,
                    Score = dto.Score,
                    Feedback = dto.Feedback,
                    GradedAt = DateTime.UtcNow
                };
                _context.Grades.Add(grade);
            }

            submission.Status = "Graded";
            await _context.SaveChangesAsync();

            return Ok(new { message = "Grade successfully saved.", score = dto.Score });
        }
    }

    public record GradeDto(decimal Score, string Feedback);
}
