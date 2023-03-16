using Applications.Commons;
using Applications.Repositories;
using AutoFixture;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Domain.Entities;
using Domain.EntityRelationship;
using Domain.Tests;
using FluentAssertions;
using Infrastructures.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Tests.Repositories
{
    public class SyllabusRepositoryTests : SetupTest
    {
        private readonly ISyllabusRepository _syllabusRepository;
        public SyllabusRepositoryTests()
        {
            _syllabusRepository = new SyllabusRepository(
                _dbContext,
                _currentTimeMock.Object,
                _claimServiceMock.Object
                );
        }

        [Fact]
        public async Task SyllabusRepository_GetEnableSyllabus_ShouldReturnCorrectData()
        {
            //arrange
            var mockData = _fixture.Build<Syllabus>()
                           .Without(s => s.TrainingProgramSyllabi)
                           .Without(s => s.SyllabusOutputStandards)
                           .Without(s => s.SyllabusModules)
                           .With(x => x.Status, Domain.Enum.StatusEnum.Status.Enable)
                           .CreateMany(30)
                           .ToList();
            await _dbContext.Syllabi.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();
            var expected = _dbContext.Syllabi.Where(s => s.Status == Domain.Enum.StatusEnum.Status.Enable)
                                             .Include(s => s.SyllabusOutputStandards).ThenInclude(s => s.OutputStandard)
                                             .OrderByDescending(s => s.CreationDate)
                                             .Take(10)
                                             .ToList();

            //act
            var resultPaging = await _syllabusRepository.GetEnableSyllabus();
            var result = resultPaging.Items;

            //assert
            resultPaging.Previous.Should().BeFalse();
            resultPaging.Next.Should().BeTrue();
            resultPaging.Items.Count.Should().Be(10);
            resultPaging.TotalItemsCount.Should().Be(30);
            resultPaging.TotalPagesCount.Should().Be(3);
            resultPaging.PageIndex.Should().Be(0);
            resultPaging.PageSize.Should().Be(10);
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task SyllabusRepository_GetDisableSyllabus_ShouldReturnCorrectData()
        {
            //arrange
            var mockData = _fixture.Build<Syllabus>()
                           .Without(s => s.TrainingProgramSyllabi)
                           .Without(s => s.SyllabusOutputStandards)
                           .Without(s => s.SyllabusModules)
                           .With(x => x.Status, Domain.Enum.StatusEnum.Status.Disable)
                           .CreateMany(30)
                           .ToList();
            await _dbContext.Syllabi.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();
            var expected = _dbContext.Syllabi.Where(s => s.Status == Domain.Enum.StatusEnum.Status.Disable)
                                             .Include(s => s.SyllabusOutputStandards).ThenInclude(s => s.OutputStandard)
                                             .OrderByDescending(s => s.CreationDate)
                                             .Take(10)
                                             .ToList();

            //act
            var resultPaging = await _syllabusRepository.GetDisableSyllabus();
            var result = resultPaging.Items;

            //assert
            resultPaging.Previous.Should().BeFalse();
            resultPaging.Next.Should().BeTrue();
            resultPaging.Items.Count.Should().Be(10);
            resultPaging.TotalItemsCount.Should().Be(30);
            resultPaging.TotalPagesCount.Should().Be(3);
            resultPaging.PageIndex.Should().Be(0);
            resultPaging.PageSize.Should().Be(10);
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task SyllabusRepository_GetSyllabusByName_ShouldReturnCorrectData()
        {
            //arrange
            var mockData = _fixture.Build<Syllabus>()
                           .Without(s => s.TrainingProgramSyllabi)
                           .Without(s => s.SyllabusOutputStandards)
                           .Without(s => s.SyllabusModules)
                           .With(s => s.SyllabusName, "Mock")
                           .CreateMany(30)
                           .ToList();
            await _dbContext.Syllabi.AddRangeAsync(mockData);
            await _dbContext.SaveChangesAsync();
            var expected = _dbContext.Syllabi.Where(s => s.SyllabusName!.Contains("Mock"))
                                             .Include(s => s.SyllabusOutputStandards).ThenInclude(s => s.OutputStandard)
                                             .OrderByDescending(s => s.CreationDate)
                                             .Take(10)
                                             .ToList();

            //act
            var resultPaging = await _syllabusRepository.GetSyllabusByName("Mock");
            var result = resultPaging.Items;

            //assert
            resultPaging.Previous.Should().BeFalse();
            resultPaging.Next.Should().BeTrue();
            resultPaging.Items.Count.Should().Be(10);
            resultPaging.TotalItemsCount.Should().Be(30);
            resultPaging.TotalPagesCount.Should().Be(3);
            resultPaging.PageIndex.Should().Be(0);
            resultPaging.PageSize.Should().Be(10);
            result.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task SyllabusRepository_GetSyllabusByOutputStandardId_ShouldReturnCorrectData()
        {
            //arrange
            var syllabusMockData = _fixture.Build<Syllabus>()
                                       .Without(s => s.SyllabusModules)
                                       .Without(S => S.SyllabusOutputStandards)
                                       .Without(s => s.TrainingProgramSyllabi)
                                       .CreateMany(10)
                                       .ToList();
            var outputStandardMockData = _fixture.Build<OutputStandard>()
                                                 .Without(s => s.SyllabusOutputStandards)
                                                 .Create();
            await _dbContext.Syllabi.AddRangeAsync(syllabusMockData);
            await _dbContext.OutputStandards.AddAsync(outputStandardMockData);
            await _dbContext.SaveChangesAsync();
            var dataList = new List<SyllabusOutputStandard>();
            foreach (var item in syllabusMockData)
            {
                var data = new SyllabusOutputStandard
                {
                    OutputStandard = outputStandardMockData,
                    Syllabus = item
                };
                dataList.Add(data);
            }
            await _dbContext.SyllabusOutputStandard.AddRangeAsync(dataList);
            await _dbContext.SaveChangesAsync();

            var expected = _dbContext.SyllabusOutputStandard.Where(s => s.OutputStandardId.Equals(outputStandardMockData.Id)).Select(s => s.Syllabus).ToList();

            //act
            var resultPaging = await _syllabusRepository.GetSyllabusByOutputStandardId(outputStandardMockData.Id);
            var result = resultPaging.Items;

            //assert
            resultPaging.Previous.Should().BeFalse();
            resultPaging.Next.Should().BeFalse();
            resultPaging.Items.Count.Should().Be(10);
            resultPaging.TotalItemsCount.Should().Be(10);
            resultPaging.TotalPagesCount.Should().Be(1);
            resultPaging.PageIndex.Should().Be(0);
            resultPaging.PageSize.Should().Be(10);
            result.Should().BeEquivalentTo(expected, op => op.Excluding(s => s.SyllabusOutputStandards));
        }

        [Fact]
        public async Task SyllabusRepository_GetSyllabusByTrainingProgramId_ShouldReturnCorrectData()
        {
            //arrange
            var syllabusMockData = _fixture.Build<Syllabus>()
                                       .Without(s => s.SyllabusModules)
                                       .Without(S => S.SyllabusOutputStandards)
                                       .Without(s => s.TrainingProgramSyllabi)
                                       .CreateMany(10)
                                       .ToList();
            var trainingProgramMockData = _fixture.Build<TrainingProgram>()
                                                 .Without(s => s.ClassTrainingPrograms)
                                                 .Without(s => s.TrainingProgramSyllabi)
                                                 .Create();
            await _dbContext.Syllabi.AddRangeAsync(syllabusMockData);
            await _dbContext.TrainingPrograms.AddAsync(trainingProgramMockData);
            await _dbContext.SaveChangesAsync();
            var dataList = new List<TrainingProgramSyllabus>();
            foreach (var item in syllabusMockData)
            {
                var data = new TrainingProgramSyllabus
                {
                    TrainingProgram = trainingProgramMockData,
                    Syllabus = item
                };
                dataList.Add(data);
            }
            await _dbContext.TrainingProgramSyllabi.AddRangeAsync(dataList);
            await _dbContext.SaveChangesAsync();

            var expected = _dbContext.TrainingProgramSyllabi.Where(s => s.TrainingProgramId.Equals(trainingProgramMockData.Id)).Select(s => s.Syllabus).ToList();

            //act
            var resultPaging = await _syllabusRepository.GetSyllabusByTrainingProgramId(trainingProgramMockData.Id);
            var result = resultPaging.Items;

            //assert
            resultPaging.Previous.Should().BeFalse();
            resultPaging.Next.Should().BeFalse();
            resultPaging.Items.Count.Should().Be(10);
            resultPaging.TotalItemsCount.Should().Be(10);
            resultPaging.TotalPagesCount.Should().Be(1);
            resultPaging.PageIndex.Should().Be(0);
            resultPaging.PageSize.Should().Be(10);
            result.Should().BeEquivalentTo(expected, op => op.Excluding(s => s.TrainingProgramSyllabi));
        }

        [Fact]
        public async Task GetSyllabusDetail_ShouldReturnCorrectData()
        {
            //arrange
            var mockData = _fixture.Build<Syllabus>()
                                    .Without(s => s.SyllabusModules)
                                    .Without(S => S.SyllabusOutputStandards)
                                    .Without(s => s.TrainingProgramSyllabi)
                                   .Create();
            _dbContext.Syllabi.Add(mockData);
            await _dbContext.SaveChangesAsync();
            //act
            var result = await _syllabusRepository.GetSyllabusDetail(mockData.Id);
            //assert
            result.Should().NotBeNull();
            result.Id.Should().Be(mockData.Id);
        }

        [Fact]
        public async Task GetAllSyllabusDetail_ShouldReturnCorrectData()
        {
            //arrange
            var syllabusMockData = _fixture.Build<Syllabus>()
                                   .Without(s => s.TrainingProgramSyllabi)
                                   .Without(s => s.SyllabusModules)
                                   .Without(s => s.SyllabusOutputStandards)
                                   .CreateMany(30);

            await _dbContext.AddRangeAsync(syllabusMockData);
            await _dbContext.SaveChangesAsync();

            var expected = _dbContext.Syllabi.Include(x => x.SyllabusOutputStandards).ThenInclude(x => x.OutputStandard)
                                                    .Include(x => x.SyllabusModules)
                                                    .Include(x => x.TrainingProgramSyllabi).OrderByDescending(x => x.CreationDate).Take(10).ToList();

            //act
            var resultPaging = await _syllabusRepository.GetAllSyllabusDetail();
            var result = resultPaging.Items;
            //assert
            resultPaging.Previous.Should().BeFalse();
            resultPaging.Next.Should().BeTrue();
            resultPaging.Items.Count.Should().Be(10);
            resultPaging.TotalItemsCount.Should().Be(30);
            resultPaging.TotalPagesCount.Should().Be(3);
            resultPaging.PageIndex.Should().Be(0);
            resultPaging.PageSize.Should().Be(10);
            result.Should().BeEquivalentTo(expected);
        }
        [Fact]
        public async Task GetAllSyllabusModule_ShouldReturnCorrectData()
        {
            //arrange
            var syllabusMockData = _fixture.Build<Syllabus>()
                                   .Without(s => s.TrainingProgramSyllabi)
                                   .Without(s => s.SyllabusModules)
                                   .Without(s => s.SyllabusOutputStandards)
                                   .CreateMany(30);

            await _dbContext.AddRangeAsync(syllabusMockData);
            await _dbContext.SaveChangesAsync();

            var expected = _dbContext.Syllabi.Include(x => x.SyllabusOutputStandards).ThenInclude(x => x.OutputStandard)
                                                    .Include(x => x.SyllabusModules)
                                                    .Include(x => x.TrainingProgramSyllabi).OrderByDescending(x => x.CreationDate).Take(10).ToList();

            //act
            var resultPaging = await _syllabusRepository.GetAllSyllabusDetail();
            var result = resultPaging.Items;
            //assert
            resultPaging.Previous.Should().BeFalse();
            resultPaging.Next.Should().BeTrue();
            resultPaging.Items.Count.Should().Be(10);
            resultPaging.TotalItemsCount.Should().Be(30);
            resultPaging.TotalPagesCount.Should().Be(3);
            resultPaging.PageIndex.Should().Be(0);
            resultPaging.PageSize.Should().Be(10);
            result.Should().BeEquivalentTo(expected);
        }


        [Fact]
        public async Task GetSyllabusByCreationDate_ShouldReturnCorrectData()
        {
            var startDate = new DateTime(2023, 3, 1);
            var endDate = new DateTime(2023, 3, 31);

            var syllabusMockdata = _fixture.Build<Syllabus>()
                .Without(s => s.TrainingProgramSyllabi)
                .Without(s => s.SyllabusModules)
                .Without(s => s.SyllabusOutputStandards)
                .With(s => s.CreationDate, new DateTime(2023, 3, 10))
                .CreateMany(30)
                .ToList();

            await _dbContext.AddRangeAsync(syllabusMockdata);
            await _dbContext.SaveChangesAsync();

            var expected = syllabusMockdata.Where(x => x.CreationDate == new DateTime(2023, 3, 10)).ToList();

            var pageNumber = 0;
            var pageSize = 10;
            var result = await _syllabusRepository.GetSyllabusByCreationDate(startDate, endDate, pageNumber, pageSize);

            result.PageIndex.Should().Be(pageNumber);
            result.PageSize.Should().Be(pageSize);
            result.TotalItemsCount.Should().Be(expected.Count);

            var expectedPageCount = (int)Math.Ceiling((double)expected.Count / pageSize);
            result.TotalPagesCount.Should().Be(expectedPageCount);

            result.Items.Should().BeEquivalentTo(expected);
        }

    }
}
