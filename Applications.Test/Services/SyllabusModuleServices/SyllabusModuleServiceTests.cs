
using Applications.Interfaces;
using Domain.Tests;

namespace Applications.Tests.Services.SyllabusModuleServices
{
    public class SyllabusModuleServiceTests : SetupTest
    {
        private readonly ISyllabusModuleService _syllabusModuleService;
        public SyllabusModuleServiceTests()
        {
            _syllabusModuleService = new(_unitOfWorkMock.Object, _mapperConfig);
        }
    }
}
