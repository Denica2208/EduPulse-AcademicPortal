using Microsoft.EntityFrameworkCore;
using AcademicPortal.API.Models;

namespace AcademicPortal.API.Data
{
    public class AcademicPortalDbContext : DbContext
    {
        public AcademicPortalDbContext(DbContextOptions<AcademicPortalDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Enrollment> Enrollments => Set<Enrollment>();
        public DbSet<LabModule> LabModules => Set<LabModule>();
        public DbSet<Assignment> Assignments => Set<Assignment>();
        public DbSet<Submission> Submissions => Set<Submission>();
        public DbSet<Grade> Grades => Set<Grade>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Role).HasConversion<string>();
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Professor)
                      .WithMany()
                      .HasForeignKey(e => e.ProfessorId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Submission>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Assignment)
                      .WithMany(a => a.Submissions)
                      .HasForeignKey(e => e.AssignmentId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Student)
                      .WithMany(u => u.Submissions)
                      .HasForeignKey(e => e.StudentId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Grade>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Submission)
                      .WithOne(s => s.Grade)
                      .HasForeignKey<Grade>(e => e.SubmissionId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
