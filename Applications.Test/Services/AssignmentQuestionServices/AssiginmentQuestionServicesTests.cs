using Applications.Interfaces;
using Applications.Services;
using ClosedXML.Excel;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;
using Moq;

namespace Applications.Tests.Services.AssignmentQuestionServices
{
    public class AssiginmentQuestionServicesTests : SetupTest
    {
        private readonly IAssignmentQuestionService _assignmentQuestionService;
        public AssiginmentQuestionServicesTests()
        {
            _assignmentQuestionService = new AssignmentQuestionService(_unitOfWorkMock.Object, _mapperConfig);
        }

        [Fact]
        public async Task ExportAssignmentQuestionByAssignmentIdTest()
        {
            // Arrange
            var assignmentId = Guid.NewGuid();
            var expected = GetExpectedResult();

            var mockService = new Mock<IAssignmentQuestionService>();
            mockService.Setup(x => x.ExportAssignmentQuestionByAssignmentId(assignmentId)).ReturnsAsync(expected);

            var service = mockService.Object;

            // Act
            var result = await service.ExportAssignmentQuestionByAssignmentId(assignmentId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected, options => options
                .WithStrictOrdering());
        }

        private byte[] GetExpectedResult()
        {
            var assId = Guid.NewGuid();
            var questions = new List<AssignmentQuestion>
            {
                new AssignmentQuestion { Id = Guid.NewGuid(), AssignmentId = assId, Question = "Question 1", Answer = "Answer 1", Note = "Note 1" },
                new AssignmentQuestion { Id = Guid.NewGuid(), AssignmentId = assId, Question = "Question 2", Answer = "Answer 2", Note = "Note 2" },
                new AssignmentQuestion { Id = Guid.NewGuid(), AssignmentId = assId, Question = "Question 3", Answer = "Answer 3", Note = "Note 3" }
            };

            using var expectedWorkbook = new XLWorkbook();
            var expectedWorksheet = expectedWorkbook.Worksheets.Add("Assignment Questions");
            expectedWorksheet.Cell(1, 1).Value = "AsignmentID";
            expectedWorksheet.Cell(2, 1).Value = "Question";
            expectedWorksheet.Cell(2, 2).Value = "Answer";
            expectedWorksheet.Cell(2, 3).Value = "Note";
            expectedWorksheet.Cell(1, 2).Value = assId.ToString();
            expectedWorksheet.Cell(3, 1).Value = "Question 1";
            expectedWorksheet.Cell(3, 2).Value = "Answer 1";
            expectedWorksheet.Cell(3, 3).Value = "Note 1";
            expectedWorksheet.Cell(4, 1).Value = "Question 2";
            expectedWorksheet.Cell(4, 2).Value = "Answer 2";
            expectedWorksheet.Cell(4, 3).Value = "Note 2";
            expectedWorksheet.Cell(5, 1).Value = "Question 3";
            expectedWorksheet.Cell(5, 2).Value = "Answer 3";
            expectedWorksheet.Cell(5, 3).Value = "Note 3";

            using var expectedStream = new MemoryStream();
            expectedWorkbook.SaveAs(expectedStream);
            var expectedContent = expectedStream.ToArray();

            return expectedContent;
        }
    }
}
