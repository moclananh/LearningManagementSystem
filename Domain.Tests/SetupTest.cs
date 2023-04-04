using Application.Interfaces;
using Application.Repositories;
using Applications;
using Applications.Interfaces;
using Applications.IRepositories;
using Applications.Repositories;
using Applications.Services;
using AutoFixture;
using AutoMapper;
using Domain.Entities;
using Infrastructures;
using Infrastructures.Mappers;
using Infrastructures.Mappers.UserMapperResovlers;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Domain.Tests
{
    public class SetupTest : IDisposable
    {
        //
        protected readonly IMapper _mapperConfig;
        protected readonly Fixture _fixture;
        protected readonly Mock<IUnitOfWork> _unitOfWorkMock;
        protected readonly Mock<IAssignmentQuestionService> _assignmentQuestionServiceMock;
        protected readonly Mock<IAssignmentService> _assignmentServiceMock;
        protected readonly Mock<IAuditPlanService> _auditPlanServiceMock;
        protected readonly Mock<IClaimService> _claimServiceMock;
        protected readonly Mock<IUserService> _userServiceMock;
        protected readonly Mock<IClassService> _classServiceMock;
        protected readonly Mock<IClassUserServices> _classUserServicesMock;
        protected readonly Mock<ICurrentTime> _currentTimeMock;
        protected readonly Mock<ILectureService> _lectureServiceMock;
        protected readonly Mock<IModuleService> _moduleServiceMock;
        protected readonly Mock<IOutputStandardService> _outputStandardServiceMock;
        protected readonly Mock<IQuizzService> _quizzServiceMock;
        protected readonly Mock<ISyllabusServices> _syllabusServiceMock;
        protected readonly Mock<ITokenService> _tokenServiceMock;
        protected readonly Mock<ITrainingProgramService> _trainingProgramServiceMock;
        protected readonly Mock<IUnitServices> _unitServicesMock;
        protected readonly Mock<IAuditResultServices> _auditResultServicesMock;
        protected readonly Mock<IPracticeService> _practiceServiceMock;
        protected readonly Mock<IClassTrainingProgramService> _classTrainingProgramServiceMock;
        protected readonly Mock<ISyllabusModuleService> _syllabusModuleServiceMock;
        protected readonly Mock<IAbsentRequestServices> _absentRequestServiceMock;

        //
        protected readonly Mock<IAssignmentQuestionRepository> _assignmentQuestionRepositoryMock;
        protected readonly Mock<IAssignmentRepository> _assignmentRepositoryMock;
        protected readonly Mock<IAuditPlanRepository> _auditPlanRepositoryMock;
        protected readonly Mock<IClassRepository> _classRepositoryMock;
        protected readonly Mock<IClassTrainingProgramRepository> _classTrainingProgramRepositoryMock;
        protected readonly Mock<IClassUserRepository> _classUserRepositoryMock;
        protected readonly Mock<ILectureRepository> _lectureRepositoryMock;
        protected readonly Mock<IModuleRepository> _modulesRepositoryMock;
        protected readonly Mock<IOutputStandardRepository> _outputStandardRepositoryMock;
        protected readonly Mock<IQuizzRepository> _quizzRepositoryMock;
        protected readonly Mock<IUnitRepository> _unitRepositoryMock;
        protected readonly Mock<ISyllabusRepository> _syllabusRepositoryMock;
        protected readonly Mock<ITrainingProgramRepository> _trainingProgramRepositoryMock;
        protected readonly Mock<IUserRepository> _userRepositoryMock;
        protected readonly Mock<IAuditResultRepository> _auditResultRepositoryMock;
        protected readonly Mock<IPracticeRepository> _practiceRepositoryMock;
        protected readonly Mock<IRefreshTokenRepository> _refreshTokenRepositoryMock;
        protected readonly Mock<IAbsentRequestRepository> _absentRequestRepositoryMock;
        protected readonly AppDBContext _dbContext;

        public SetupTest()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperConfig());
                mc.ConstructServicesUsing(s => new CreateByResolver(_unitOfWorkMock.Object));
                //mc.ConstructServicesUsing(s => new UpdateImageResovler());
            });
            _mapperConfig = mappingConfig.CreateMapper();
            _fixture = new Fixture();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            //
            _auditResultServicesMock = new Mock<IAuditResultServices>();
            _assignmentQuestionServiceMock = new Mock<IAssignmentQuestionService>();
            _assignmentServiceMock = new Mock<IAssignmentService>();
            _auditPlanServiceMock = new Mock<IAuditPlanService>();
            _claimServiceMock = new Mock<IClaimService>();
            _classServiceMock = new Mock<IClassService>();
            _classUserServicesMock = new Mock<IClassUserServices>();
            _currentTimeMock = new Mock<ICurrentTime>();
            _lectureServiceMock = new Mock<ILectureService>();
            _moduleServiceMock = new Mock<IModuleService>();
            _outputStandardServiceMock = new Mock<IOutputStandardService>();
            _quizzServiceMock = new Mock<IQuizzService>();
            _syllabusServiceMock = new Mock<ISyllabusServices>();
            _tokenServiceMock = new Mock<ITokenService>();
            _trainingProgramServiceMock = new Mock<ITrainingProgramService>();
            _unitServicesMock = new Mock<IUnitServices>();
            _userServiceMock = new Mock<IUserService>();
            _practiceServiceMock = new Mock<IPracticeService>();
            _classTrainingProgramServiceMock = new Mock<IClassTrainingProgramService>();
            _syllabusModuleServiceMock = new Mock<ISyllabusModuleService>();
            _absentRequestServiceMock = new Mock<IAbsentRequestServices>();
            //
            _auditResultRepositoryMock = new Mock<IAuditResultRepository>();
            _assignmentQuestionRepositoryMock = new Mock<IAssignmentQuestionRepository>();
            _assignmentRepositoryMock = new Mock<IAssignmentRepository>();
            _auditPlanRepositoryMock = new Mock<IAuditPlanRepository>();
            _classRepositoryMock = new Mock<IClassRepository>();
            _classTrainingProgramRepositoryMock = new Mock<IClassTrainingProgramRepository>();
            _classUserRepositoryMock = new Mock<IClassUserRepository>();
            _lectureRepositoryMock = new Mock<ILectureRepository>();
            _modulesRepositoryMock = new Mock<IModuleRepository>();
            _outputStandardRepositoryMock = new Mock<IOutputStandardRepository>();
            _quizzRepositoryMock = new Mock<IQuizzRepository>();
            _syllabusRepositoryMock = new Mock<ISyllabusRepository>();
            _trainingProgramRepositoryMock = new Mock<ITrainingProgramRepository>();
            _unitRepositoryMock = new Mock<IUnitRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _practiceRepositoryMock= new Mock<IPracticeRepository>();
            _refreshTokenRepositoryMock = new Mock<IRefreshTokenRepository>();
            _absentRequestRepositoryMock= new Mock<IAbsentRequestRepository>();

            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _dbContext = new AppDBContext(options);

            _currentTimeMock.Setup(x => x.CurrentTime()).Returns(DateTime.UtcNow);
            _claimServiceMock.Setup(x => x.GetCurrentUserId).Returns(Guid.Empty);
        }
        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}