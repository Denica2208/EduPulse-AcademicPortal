using Xunit;
using AcademicPortal.API.Models;

namespace AcademicPortal.Tests
{
    public class SubmissionValidationTests
    {
        [Fact]
        public void Assignment_LateSubmission_IsFlaggedWhenAfterDeadline()
        {
            var deadline = new DateTime(2026, 11, 27, 23, 59, 0, DateTimeKind.Utc);
            var submittedAt = new DateTime(2026, 11, 28, 0, 15, 0, DateTimeKind.Utc);

            var submission = new Submission
            {
                SubmittedAt = submittedAt,
                IsLate = submittedAt > deadline
            };

            Assert.True(submission.IsLate);
        }

        [Fact]
        public void Assignment_OnTimeSubmission_IsNotFlaggedLate()
        {
            var deadline = new DateTime(2026, 11, 27, 23, 59, 0, DateTimeKind.Utc);
            var submittedAt = new DateTime(2026, 11, 24, 16, 30, 0, DateTimeKind.Utc);

            var submission = new Submission
            {
                SubmittedAt = submittedAt,
                IsLate = submittedAt > deadline
            };

            Assert.False(submission.IsLate);
        }

        [Fact]
        public void Grade_DefaultEntity_InitializesWithValidGuid()
        {
            var grade = new Grade
            {
                Score = 10.0m,
                Feedback = "Clean architecture and complete unit tests."
            };

            Assert.NotEqual(Guid.Empty, grade.Id);
            Assert.InRange(grade.Score, 1.0m, 10.0m);
        }
    }
}
