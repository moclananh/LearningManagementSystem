using Applications.Commons;
using Applications.Interfaces;
using Applications.Repositories;
using Applications.Services;
using Applications.ViewModels.AssignmentQuestionViewModels;
using AutoFixture;
using AutoMapper;
using ClosedXML.Excel;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;
using Moq;
using OfficeOpenXml;
using System.Net;

namespace Applications.Tests.Services.AssignmentQuestionServices
{
    public class AssignmentQuestionServicesTests : SetupTest
    {
        private readonly IAssignmentQuestionService _assignmentQuestionService;
        public AssignmentQuestionServicesTests()
        {
            _assignmentQuestionService = new AssignmentQuestionService(_unitOfWorkMock.Object, _mapperConfig);
        }

        [Fact]
        public async Task ExportAssignmentQuestionByAssignmentId_WithValidInput_ReturnsExcelFile()
        {
            // Arrange
            Guid assignmentId = Guid.NewGuid();
            List<AssignmentQuestion> questions = new List<AssignmentQuestion>
            {
                new AssignmentQuestion { Id = Guid.NewGuid(), AssignmentId = assignmentId, Question = "Question 1", Answer = "Answer 1", Note = "Note 1" },
                new AssignmentQuestion { Id = Guid.NewGuid(), AssignmentId = assignmentId, Question = "Question 2", Answer = "Answer 2", Note = "Note 2" },
                new AssignmentQuestion { Id = Guid.NewGuid(), AssignmentId = assignmentId, Question = "Question 3", Answer = "Answer 3", Note = "Note 3" }
            };
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IAssignmentQuestionRepository>();
            _unitOfWorkMock.Setup(uow => uow.AssignmentQuestionRepository).Returns(mockRepository.Object);
            mockRepository.Setup(repo => repo.GetAssignmentQuestionListByAssignmentId(assignmentId)).ReturnsAsync(questions);

            // Act
            byte[] result = await _assignmentQuestionService.ExportAssignmentQuestionByAssignmentId(assignmentId);

            // Assert
            using (var stream = new MemoryStream(result))
            {
                using (var excelPackage = new ExcelPackage(stream))
                {
                    // Make sure that the workbook contains at least one worksheet
                    Assert.True(excelPackage.Workbook.Worksheets.Count > 0);

                    var worksheet = excelPackage.Workbook.Worksheets[0];

                    // Check that the worksheet name is correct
                    Assert.Equal("Assignment Questions", worksheet.Name);

                    // Check the headers
                    Assert.Equal("AsignmentID", worksheet.Cells[1, 1].Value.ToString());
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

        [Fact]
        public async Task GetAssignmentQuestionByAssignmentId_ShouldReturnCorrectData()
        {
            //arrange
            var id = Guid.NewGuid();
            var assignmentMockData = new Pagination<AssignmentQuestion>
            {
                Items = _fixture.Build<AssignmentQuestion>()
                                .Without(x => x.Assignment)
                                .With(x => x.AssignmentId, id)
                                .CreateMany(30)
                                .ToList(),
                PageIndex = 0,
                PageSize = 10,
                TotalItemsCount = 30,
            };
            var asm = _mapperConfig.Map<Pagination<AssignmentQuestion>>(assignmentMockData);
            _unitOfWorkMock.Setup(x => x.AssignmentQuestionRepository.GetAllAssignmentQuestionByAssignmentId(id, 0, 10)).ReturnsAsync(assignmentMockData);
            var expected = _mapperConfig.Map<Pagination<AssignmentQuestionViewModel>>(asm);
            //act
            var result = await _assignmentQuestionService.GetAssignmentQuestionByAssignmentId(id);
            //assert
            _unitOfWorkMock.Verify(x => x.AssignmentQuestionRepository.GetAllAssignmentQuestionByAssignmentId(id, 0, 10), Times.Once());
        }

        [Fact]
        public async Task DeleteAssignmentQuestionByCreationDate_Should_Delete_QuizzQuestions_And_Return_DeleteSucceed()
        {
            // Arrange
            var startDate = new DateTime(2023, 03, 20);
            var endDate = new DateTime(2023, 03, 22);
            var assignmentId = Guid.NewGuid();
            var assignmentQuestionList = new List<AssignmentQuestion>()
        {
            new AssignmentQuestion(),
            new AssignmentQuestion(),
            new AssignmentQuestion()
        };

            _unitOfWorkMock.Setup(x => x.AssignmentQuestionRepository.GetAssignmentQuestionListByCreationDate(startDate, endDate, assignmentId)).ReturnsAsync(assignmentQuestionList);

            // Act
            var result = await _assignmentQuestionService.DeleteAssignmentQuestionByCreationDate(startDate, endDate, assignmentId);

            // Assert
            _unitOfWorkMock.Verify(x => x.AssignmentQuestionRepository.SoftRemoveRange(assignmentQuestionList), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangeAsync(), Times.Once);
            Assert.Equal(HttpStatusCode.OK.ToString(), result.Status);
            Assert.Equal("Delete Succeed", result.Message);
        }

        [Fact]
        public async Task DeleteAssignmentQuestionByCreationDate_Should_Return_NotFound_If_QuizzQuestion_List_Is_Empty()
        {
            // Arrange
            var startDate = new DateTime(2023, 03, 20);
            var endDate = new DateTime(2023, 03, 22);
            var assignmentId = Guid.NewGuid();
            var assignmentQuestionList = new List<AssignmentQuestion>();

            _unitOfWorkMock.Setup(x => x.AssignmentQuestionRepository.GetAssignmentQuestionListByCreationDate(startDate, endDate, assignmentId)).ReturnsAsync(assignmentQuestionList);

            // Act
            var result = await _assignmentQuestionService.DeleteAssignmentQuestionByCreationDate(startDate, endDate, assignmentId);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent.ToString(), result.Status);
            Assert.Equal("Not Found", result.Message);
        }
    }
}
