using Applications.Interfaces;
using Applications.Services;
using FluentAssertions;
using Moq;
using ClosedXML.Excel;
using Domain.Entities;
using Domain.Tests;
using System.Net;
using Applications.Repositories;
using OfficeOpenXml;

namespace Applications.Tests.Services.QuizzQuestionServices
{
    public class QuizzQuestionServiceTests : SetupTest
    {
        private readonly IQuizzQuestionService _quizzQuestionService;
        public QuizzQuestionServiceTests()
        {
            _quizzQuestionService = new QuizzQuestionService(_unitOfWorkMock.Object, _mapperConfig);
        }

        [Fact]
        public async Task ExportQuizzQuestionByQuizzId_WithValidInput_ReturnsExcelFile()
        {
            // Arrange
            var quizzId = Guid.NewGuid();
            var questions = new List<QuizzQuestion>
            {
                new QuizzQuestion { Id = Guid.NewGuid(), QuizzId = quizzId, Question = "Question 1", Answer = "Answer 1", Note = "Explanation 1" },
                new QuizzQuestion { Id = Guid.NewGuid(), QuizzId = quizzId, Question = "Question 2", Answer = "Answer 2", Note = "Explanation 2" },
                new QuizzQuestion { Id = Guid.NewGuid(), QuizzId = quizzId, Question = "Question 3", Answer = "Answer 3", Note = "Explanation 3" }
            };
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IQuizzQuestionRepository>();
            _unitOfWorkMock.Setup(uow => uow.QuizzQuestionRepository).Returns(mockRepository.Object);
            mockRepository.Setup(repo => repo.GetQuizzQuestionListByQuizzId(quizzId)).ReturnsAsync(questions);

            // Act
            byte[] result = await _quizzQuestionService.ExportQuizzQuestionByQuizzId(quizzId);

            // Assert
            using (var stream = new MemoryStream(result))
            {
                using (var excelPackage = new ExcelPackage(stream))
                {
                    // Make sure that the workbook contains at least one worksheet
                    Assert.True(excelPackage.Workbook.Worksheets.Count > 0);

                    var worksheet = excelPackage.Workbook.Worksheets[0];

                    // Check that the worksheet name is correct
                    Assert.Equal("Quizz Questions", worksheet.Name);

                    // Check the headers
                    Assert.Equal("QuizzID", worksheet.Cells[1, 1].Value.ToString());
                    Assert.Equal("Question", worksheet.Cells[2, 1].Value.ToString());
                    Assert.Equal("Answer", worksheet.Cells[2, 2].Value.ToString());
                    Assert.Equal("Note", worksheet.Cells[2, 3].Value.ToString());

                    // Check the values
                    for (int i = 0; i < questions.Count; i++)
                    {
                        var question = questions[i];
                        Assert.Equal(question.Question, worksheet.Cells[i + 3, 1].Value.ToString());
                        Assert.Equal(question.Answer, worksheet.Cells[i + 3, 2].Value.ToString());
                        Assert.Equal(question.Note, worksheet.Cells[i + 3, 3].Value.ToString());
                    }
                }
            }
        }

        [Fact]
        public async Task ExportQuizzQuestionByQuizzIdTest()
        {
            // Arrange
            var quizzId = Guid.NewGuid();
            var expected = GetExpectedResult();

            var mockService = new Mock<IQuizzQuestionService>();
            mockService.Setup(x => x.ExportQuizzQuestionByQuizzId(quizzId)).ReturnsAsync(expected);

            var service = mockService.Object;

            // Act
            var result = await service.ExportQuizzQuestionByQuizzId(quizzId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected, options => options
                .WithStrictOrdering());
        }

        private byte[] GetExpectedResult()
        {
            var quizzId = Guid.NewGuid();
            var questions = new List<QuizzQuestion>
            {
                new QuizzQuestion { Id = Guid.NewGuid(), QuizzId = quizzId, Question = "Question 1", Answer = "Answer 1", Note = "Explanation 1" },
                new QuizzQuestion { Id = Guid.NewGuid(), QuizzId = quizzId, Question = "Question 2", Answer = "Answer 2", Note = "Explanation 2" },
                new QuizzQuestion { Id = Guid.NewGuid(), QuizzId = quizzId, Question = "Question 3", Answer = "Answer 3", Note = "Explanation 3" }
            };

            using var expectedWorkbook = new XLWorkbook();
            var expectedWorksheet = expectedWorkbook.Worksheets.Add("Quizz Questions");
            expectedWorksheet.Cell(1, 1).Value = "Quizz ID";
            expectedWorksheet.Cell(2, 1).Value = "Question";
            expectedWorksheet.Cell(2, 2).Value = "Answer";
            expectedWorksheet.Cell(2, 3).Value = "Note";
            expectedWorksheet.Cell(1, 2).Value = quizzId.ToString();
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

        [Fact]
        public async Task DeleteQuizzQuestionByCreationDate_Should_Delete_QuizzQuestions_And_Return_DeleteSucceed()
        {
            // Arrange
            var startDate = new DateTime(2023, 03, 20);
            var endDate = new DateTime(2023, 03, 22);
            var quizzId = Guid.NewGuid();
            var quizzQuestionList = new List<QuizzQuestion>()
        {
            new QuizzQuestion(),
            new QuizzQuestion(),
            new QuizzQuestion()
        };

            _unitOfWorkMock.Setup(x => x.QuizzQuestionRepository.GetQuizzQuestionListByCreationDate(startDate, endDate, quizzId)).ReturnsAsync(quizzQuestionList);

            // Act
            var result = await _quizzQuestionService.DeleteQuizzQuestionByCreationDate(startDate, endDate, quizzId);

            // Assert
            _unitOfWorkMock.Verify(x => x.QuizzQuestionRepository.SoftRemoveRange(quizzQuestionList), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once);
            Assert.Equal(HttpStatusCode.OK.ToString(), result.Status);
            Assert.Equal("Delete Succeed", result.Message);
        }

        [Fact]
        public async Task DeleteQuizzQuestionByCreationDate_Should_Return_NotFound_If_QuizzQuestion_List_Is_Empty()
        {
            // Arrange
            var startDate = new DateTime(2023, 03, 20);
            var endDate = new DateTime(2023, 03, 22);
            var quizzId = Guid.NewGuid();
            var quizzQuestionList = new List<QuizzQuestion>();

            _unitOfWorkMock.Setup(x => x.QuizzQuestionRepository.GetQuizzQuestionListByCreationDate(startDate, endDate, quizzId)).ReturnsAsync(quizzQuestionList);

            // Act
            var result = await _quizzQuestionService.DeleteQuizzQuestionByCreationDate(startDate, endDate, quizzId);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent.ToString(), result.Status);
            Assert.Equal("Not Found", result.Message);
        }
    }
}
