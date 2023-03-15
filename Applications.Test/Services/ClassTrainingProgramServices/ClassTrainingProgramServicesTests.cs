using Applications.Commons;
using Applications.Interfaces;
using Applications.Services;
using Applications.ViewModels.ClassTrainingProgramViewModels;
using AutoFixture;
using Domain.Entities;
using Domain.EntityRelationship;
using Domain.Tests;
using Moq;

namespace Applications.Tests.Services.ClassTrainingProgramServices
{
    public class ClassTrainingProgramServicesTests : SetupTest
    {
        private readonly IClassTrainingProgramService _classTrainingProgramService;
        public ClassTrainingProgramServicesTests()
        {
            _classTrainingProgramService = new ClassTrainingProgramService(_unitOfWorkMock.Object, _mapperConfig);
        }

        [Fact]
        public async Task GetAllClassTrainingProgram_ShouldReturnCorrectData()
        {
            // arrange
            var mockdata = new Pagination<ClassTrainingProgram>
            {
                Items = _fixture.Build<ClassTrainingProgram>()
                                .Without(x => x.Class)
                                .Without(x => x.TrainingProgram)
                                .CreateMany(30)
                                .ToList(),
                PageIndex = 0,
                PageSize = 10,
                TotalItemsCount = 30
            };
            var expected = _mapperConfig.Map<Pagination<ClassTrainingProgramViewModel>>(mockdata);
            var guidList = mockdata.Items.Select(x => x.CreatedBy).ToList();
            foreach (var item in expected.Items)
            {
                foreach (var user in guidList)
                {
                    var createBy = new User { Email = "mock@example.com" };
                    _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(user)).ReturnsAsync(createBy);
                    item.CreatedBy = createBy.Email;
                }
            }
            _unitOfWorkMock.Setup(x => x.ClassTrainingProgramRepository.ToPagination(0, 10)).ReturnsAsync(mockdata);

            // act
            var result = await _classTrainingProgramService.GetAllClassTrainingProgram();

            // assert
            _unitOfWorkMock.Verify(x => x.ClassTrainingProgramRepository.ToPagination(0, 10), Times.Once());
        }
    }
}
