using Applications.Interfaces;
using Applications.Services;
using Applications.ViewModels.AbsentRequest;
using AutoFixture;
using Domain.Entities;
using Domain.Tests;
using Moq;

namespace Applications.Tests.Services.AbsentServices
{
    public class AbsentServiceTest: SetupTest
    {
        private readonly IAbsentRequestServices _absentService;
        public AbsentServiceTest()
        {
            _absentService = new AbsentRequestService(_unitOfWorkMock.Object, _mapperConfig);
        }

        [Fact]
        public async Task GetAbsentRequestById_ShouldReturnCorrectData()
        {
            var Mocks = _fixture.Build<AbsentRequest>()
                                .Without(x => x.AbsentReason)
                                .Without(x => x.AbsentDate)
                                .Without(x => x.IsAccepted)
                                .Without(x => x.User)
                                .Without(x => x.Class)
                                .Create();
            var expected = _mapperConfig.Map<AbsentRequestViewModel>(Mocks);
            var createBy = new User { Email = "mock@example.com" };
            _unitOfWorkMock.Setup(x => x.UserRepository.GetByIdAsync(Mocks.CreatedBy)).ReturnsAsync(createBy);
            expected.CreatedBy = createBy.Email;
            _unitOfWorkMock.Setup(x => x.AbsentRequestRepository.GetByIdAsync(Mocks.Id))
                           .ReturnsAsync(Mocks);
            //act
            var result = await _absentService.GetAbsentById(Mocks.Id);
            //assert
            _unitOfWorkMock.Verify(x => x.AbsentRequestRepository.GetByIdAsync(Mocks.Id), Times.Once());
        }
    }
}
