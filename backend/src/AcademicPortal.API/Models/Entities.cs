using System;
using System.Collections.Generic;

namespace AcademicPortal.API.Models
{
    public enum UserRole
    {
        Student,
        Professor,
        Administrator
    }

    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
    }

    public class Course
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string CourseName { get; set; } = string.Empty;
        public string CourseCode { get; set; } = string.Empty;
        public Guid ProfessorId { get; set; }
        public User? Professor { get; set; }
        public string AcademicYear { get; set; } = "2026-2027";

        public ICollection<LabModule> LabModules { get; set; } = new List<LabModule>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }

    public class Enrollment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid StudentId { get; set; }
        public User? Student { get; set; }
        public Guid CourseId { get; set; }
        public Course? Course { get; set; }
        public string StudyGroup { get; set; } = "CEN 4.S2 B";
        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
    }

    public class LabModule
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CourseId { get; set; }
        public Course? Course { get; set; }
        public int LabNumber { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? AttachmentUrl { get; set; }

        public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
    }

    public class Assignment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid LabModuleId { get; set; }
        public LabModule? LabModule { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal MaxScore { get; set; } = 10.0m;
        public DateTime Deadline { get; set; }
        public string AllowedExtension { get; set; } = ".zip";

        public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
    }

    public class Submission
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid AssignmentId { get; set; }
        public Assignment? Assignment { get; set; }
        public Guid StudentId { get; set; }
        public User? Student { get; set; }
        public string OriginalFileName { get; set; } = string.Empty;
        public string StoredFilePath { get; set; } = string.Empty;
        public long FileSizeBytes { get; set; }
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
        public bool IsLate { get; set; }
        public string Status { get; set; } = "Submitted"; // Submitted, Graded

        public Grade? Grade { get; set; }
    }

    public class Grade
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid SubmissionId { get; set; }
        public Submission? Submission { get; set; }
        public Guid GradedById { get; set; }
        public User? GradedBy { get; set; }
        public decimal Score { get; set; }
        public string Feedback { get; set; } = string.Empty;
        public DateTime GradedAt { get; set; } = DateTime.UtcNow;
    }
}
