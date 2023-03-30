using Applications.Commons;
using Applications.Interfaces;
using Applications.Repositories;
using Applications.Services;
using Applications.ViewModels.PracticeQuestionViewModels;
using Applications.ViewModels.Response;
using AutoFixture;
using ClosedXML.Excel;
using Domain.Entities;
using Domain.Tests;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OfficeOpenXml;
using System.Net;

namespace Applications.Tests.Services.PracticeQuestionServices
{
    public class PracticeQuestionServiceTest : SetupTest
    {
        private readonly IPracticeQuestionService _practiceQuestionService;
        public PracticeQuestionServiceTest()
        {
            _practiceQuestionService = new PracticeQuestionService(_unitOfWorkMock.Object, _mapperConfig);
        }

        [Fact]
        public async Task GetPracticeQuestionById_ShouldReturnCorrectData()
        {
            //arrange
            var id = Guid.NewGuid();
            var MockData = new Pagination<PracticeQuestion>
            {
                Items = _fixture.Build<PracticeQuestion>()
                                .Without(x => x.Practice)
                                .With(x => x.PracticeId, id)
                                .CreateMany(30)
                                .ToList(),
                PageIndex = 0,
                PageSize = 10,
                TotalItemsCount = 30,
            };
            var practiceQuestion = _mapperConfig.Map<Pagination<PracticeQuestion>>(MockData);
            _unitOfWorkMock.Setup(x => x.PracticeQuestionRepository.GetAllPracticeQuestionById(id, 0, 10)).ReturnsAsync(MockData);
            var expected = _mapperConfig.Map<Pagination<PracticeQuestionViewModel>>(practiceQuestion);
            //act
            var result = await _practiceQuestionService.GetPracticeQuestionByPracticeId(id);
            //assert
            _unitOfWorkMock.Verify(x => x.PracticeQuestionRepository.GetAllPracticeQuestionById(id, 0, 10), Times.Once());
        }

        [Fact]
        public async Task ExportPracticeQuestionByPracticeId_WithValidInput_ReturnsExcelFile()
        {
            // Arrange
            Guid practiceId = Guid.NewGuid();
            List<PracticeQuestion> questions = new List<PracticeQuestion>
            {
                new PracticeQuestion { Id = Guid.NewGuid(), PracticeId = practiceId, Question = "Question 1", Answer = "Answer 1", Note = "Note 1" },
                new PracticeQuestion { Id = Guid.NewGuid(), PracticeId = practiceId, Question = "Question 2", Answer = "Answer 2", Note = "Note 2" },
                new PracticeQuestion { Id = Guid.NewGuid(), PracticeId = practiceId, Question = "Question 3", Answer = "Answer 3", Note = "Note 3" }
            };
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            var mockRepository = new Mock<IPracticeQuestionRepository>();
            _unitOfWorkMock.Setup(uow => uow.PracticeQuestionRepository).Returns(mockRepository.Object);
            mockRepository.Setup(repo => repo.GetAllPracticeQuestionByPracticeId(practiceId)).ReturnsAsync(questions);

            // Act
            byte[] result = await _practiceQuestionService.ExportPracticeQuestionByPracticeId(practiceId);

            // Assert
            using (var stream = new MemoryStream(result))
            {
                using (var excelPackage = new ExcelPackage(stream))
                {
                    // Make sure that the workbook contains at least one worksheet
                    Assert.True(excelPackage.Workbook.Worksheets.Count > 0);

                    var worksheet = excelPackage.Workbook.Worksheets[0];

                    // Check that the worksheet name is correct
                    Assert.Equal("Practice Questions", worksheet.Name);

                    // Check the headers
                    Assert.Equal("PracticeID", worksheet.Cells[1, 1].Value.ToString());
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
        public async Task ExportPracticeQuestionByPracticeId_ShouldReturnCorrectData()
        {
            // Arrange
            var practiceId = Guid.NewGuid();
            var expected = GetExpectedResult();

            var mockService = new Mock<IPracticeQuestionService>();
            mockService.Setup(x => x.ExportPracticeQuestionByPracticeId(practiceId)).ReturnsAsync(expected);

            var service = mockService.Object;

            // Act
            var result = await service.ExportPracticeQuestionByPracticeId(practiceId);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected, options => options
                .WithStrictOrdering());
        }

        private byte[] GetExpectedResult()
        {
            var praId = Guid.NewGuid();
            var questions = new List<PracticeQuestion>
            {
                new PracticeQuestion { Id = Guid.NewGuid(), PracticeId = praId, Question = "Question 1", Answer = "Answer 1", Note = "Note 1" },
                new PracticeQuestion { Id = Guid.NewGuid(), PracticeId = praId, Question = "Question 2", Answer = "Answer 2", Note = "Note 2" },
                new PracticeQuestion { Id = Guid.NewGuid(), PracticeId = praId, Question = "Question 3", Answer = "Answer 3", Note = "Note 3" }
            };

            using var expectedWorkbook = new XLWorkbook();
            var expectedWorksheet = expectedWorkbook.Worksheets.Add("Practice Questions");
            expectedWorksheet.Cell(1, 1).Value = "PracticeID";
            expectedWorksheet.Cell(2, 1).Value = "Question";
            expectedWorksheet.Cell(2, 2).Value = "Answer";
            expectedWorksheet.Cell(2, 3).Value = "Note";
            expectedWorksheet.Cell(1, 2).Value = praId.ToString();
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
        public async Task DeletePracticeQuestionByCreationDate_WithValidParameters_ShouldReturnFailedResponse()
        {
            // Arrange
            var practiceId = Guid.NewGuid();
            var PQMocks = _fixture.Build<PracticeQuestion>()
                                .Without(x => x.Practice)
                                .With(x => x.PracticeId, practiceId)
                                .With(x => x.CreationDate, DateTime.Today.Date)
                                .CreateMany(30);
            var startDate = DateTime.Today.Date.AddDays(1);
            var endDate = DateTime.Today.Date.AddDays(10);
            var PQresult = _mapperConfig.Map<List<PracticeQuestion>>(PQMocks);
            // Mock the repository and unit of work
            var mockPracticeQuestionRepository = _unitOfWorkMock.Setup(x => x.PracticeQuestionRepository.GetPracticeQuestionListByCreationDate(startDate, endDate, practiceId))
                .ReturnsAsync(new List<PracticeQuestion>());

            // Act
            var result = await _practiceQuestionService.DeletePracticeQuestionByCreationDate(startDate, endDate, practiceId);
            var expected = new Response(HttpStatusCode.NoContent, "No Practice Question Found");

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task DeletePracticeQuestionByCreationDate_WithValidParameters_ShouldReturnSuccessResponse()
        {
            // Arrange
            var practiceId = Guid.NewGuid();
            var PQMocks = _fixture.Build<PracticeQuestion>()
                                .Without(x => x.Practice)
                                .With(x => x.PracticeId , practiceId)
                                .With(x => x.CreationDate, DateTime.Today.Date)  
                                .CreateMany(30);
            var startDate = DateTime.Today.Date.AddDays(-1);
            var endDate = DateTime.Today.Date.AddDays(1);
            var PQresult = _mapperConfig.Map<List<PracticeQuestion>>(PQMocks);
            // Mock the repository and unit of work
            var mockPracticeQuestionRepository = _unitOfWorkMock.Setup(x => x.PracticeQuestionRepository.GetPracticeQuestionListByCreationDate(startDate, endDate, practiceId))
                .ReturnsAsync(PQresult);

            // Act
            var result = await _practiceQuestionService.DeletePracticeQuestionByCreationDate(startDate, endDate, practiceId);
            var expected = new Response(HttpStatusCode.OK, "Delete Succeed");

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}
