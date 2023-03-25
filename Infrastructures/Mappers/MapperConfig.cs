using Applications.ViewModels.AssignmentViewModels;
using Applications.ViewModels.UserViewModels;
using Applications.ViewModels.ClassViewModels;
using AutoMapper;
using Domain.Entities;
using Application.ViewModels.QuizzViewModels;
using Domain.EntityRelationship;
using Applications.ViewModels.ClassUserViewModels;
using Applications.ViewModels.ModuleViewModels;
using Applications.ViewModels.AuditPlanViewModel;
using Applications.ViewModels.LectureViewModels;
using Application.ViewModels.UnitViewModels;
using Application.ViewModels.TrainingProgramModels;
using Applications.ViewModels.OutputStandardViewModels;
using Applications.ViewModels.SyllabusViewModels;
using Applications.ViewModels.AssignmentQuestionViewModels;
using Applications.Commons;
using Applications.ViewModels.ClassTrainingProgramViewModels;
using Applications.ViewModels.AuditResultViewModels;
using Applications.ViewModels.PracticeViewModels;
using Applications.ViewModels.PracticeQuestionViewModels;
using Applications.ViewModels.TrainingProgramSyllabi;
using Applications.ViewModels.SyllabusOutputStandardViewModels;
using Applications.ViewModels.UnitModuleViewModel;
using Applications.ViewModels.UserAuditPlanViewModels;
using Applications.ViewModels.TrainingProgramModels;
using Applications.ViewModels.QuizzQuestionViewModels;
using Applications.ViewModels.SyllabusModuleViewModel;
using Applications.ViewModels.ModuleUnitViewModels;
using Applications.ViewModels.AttendanceViewModels;
using Infrastructures.Mappers.UserMapperResovlers;
using Applications.ViewModels.AbsentRequest;

namespace Infrastructures.Mappers
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<UpdateClassViewModel, Class>().ReverseMap();
            CreateMap<CreateClassViewModel, Class>().ReverseMap();
            CreateMap<ClassViewModel, Class>().ReverseMap();
            CreateMap<UpdateAssignmentViewModel, Assignment>().ReverseMap();
            CreateMap<AssignmentViewModel, Assignment>().ReverseMap();
            CreateMap<CreateAssignmentViewModel, Assignment>().ReverseMap();
            CreateMap<CreateQuizzViewModel, Quizz>().ReverseMap();
            CreateMap<QuizzViewModel, Quizz>().ReverseMap();
            CreateMap<UpdateQuizzViewModel, Quizz>().ReverseMap();
            CreateMap<Quizz, QuizzViewModel>();
            CreateMap<UserViewModel, User>();
            CreateMap<User, UserViewModel>()
                .ForMember(dest => dest.Gender, src => src.MapFrom(s => s.Gender == true ? "Male":"Female"))
                .ForMember(dest => dest.createByEmail, src => src.MapFrom<CreateByResolver>());
            CreateMap<UpdateUserViewModel, User>()
                .ForMember(dest => dest.Image, src => src.MapFrom<UpdateImageResovler>());
            CreateMap<CreateClassUserViewModel, ClassUser>().ReverseMap();
            CreateMap<AuditPlanViewModel, AuditPlan>().ReverseMap();
            CreateMap<UpdateAuditPlanViewModel, AuditPlan>().ReverseMap();
            CreateMap<CreateAuditPlanViewModel, AuditPlan>().ReverseMap();
            CreateMap<CreateLectureViewModel, Lecture>().ReverseMap();
            CreateMap<UpdateLectureViewModel, Lecture>().ReverseMap();
            CreateMap<LectureViewModel, Lecture>().ReverseMap();
            CreateMap<CreateUnitViewModel, Unit>().ReverseMap();
            CreateMap<UnitViewModel, Unit>().ReverseMap();
            CreateMap<Unit, UnitViewModel>()
               .ForMember(dest => dest.UnitId, src => src.MapFrom(x => x.Id));
            CreateMap<Attendance, CreateAttendanceViewModel>().ForMember(dest => dest.ClassCode, opt => opt.MapFrom(src => src.Class.ClassCode)).ForMember(dest => dest.fullname, opt => opt.MapFrom(src => src.User.firstName + " " + src.User.lastName));
            CreateMap<ModuleViewModels, Module>().ReverseMap();
            CreateMap<CreateModuleViewModel, Module>().ReverseMap();
            CreateMap<UpdateModuleViewModel, Module>().ReverseMap();
            CreateMap<CreateTrainingProgramViewModel, TrainingProgram>().ReverseMap();
            CreateMap<UpdateTrainingProgramViewModel, TrainingProgram>().ReverseMap();
            CreateMap<TrainingProgramViewModel, TrainingProgram>().ReverseMap();
            CreateMap<TrainingProgramSyllabiView, TrainingProgramSyllabus>().ReverseMap();
            CreateMap<UpdateOutputStandardViewModel, OutputStandard>().ReverseMap();
            CreateMap<OutputStandardViewModel, OutputStandard>().ReverseMap();
            CreateMap<CreateOutputStandardViewModel, OutputStandard>().ReverseMap();
            CreateMap<CreateSyllabusViewModel, Syllabus>().ReverseMap();
            CreateMap<UpdateSyllabusViewModel, Syllabus>().ReverseMap();
            CreateMap<Syllabus, SyllabusViewModel>().ReverseMap();
            CreateMap<CreateUnitViewModel, Unit>().ReverseMap();
            CreateMap<AssignmentQuestionViewModel, AssignmentQuestion>().ReverseMap();
            CreateMap<CreateClassTrainingProgramViewModel, ClassTrainingProgram>().ReverseMap();
            CreateMap<CreateSyllabusOutputStandardViewModel, SyllabusOutputStandard>().ReverseMap();
            CreateMap<PracticeViewModel, Practice>().ReverseMap();
            CreateMap<AuditResultViewModel, AuditResult>().ReverseMap();
            CreateMap<PracticeQuestionViewModel, PracticeQuestion>().ReverseMap();
            CreateMap<CreateTrainingProgramSyllabi, TrainingProgramSyllabus>().ReverseMap();
            CreateMap<ModuleUnitViewModel, ModuleUnit>().ReverseMap();
            CreateMap<CreateModuleUnitViewModel, ModuleUnit>().ReverseMap();
            CreateMap<CreateUserAuditPlanViewModel, UserAuditPlan>().ReverseMap();
            CreateMap<UpdateAuditResultViewModel, AuditResult>().ReverseMap();
            CreateMap<ClassTrainingProgramViewModel, ClassTrainingProgram>().ReverseMap();
            CreateMap<CreateClassTrainingProgramViewModel, ClassTrainingProgram>().ReverseMap();
            CreateMap<UpdatePracticeViewModel, Practice>().ReverseMap();
            CreateMap<UpdateAuditResultViewModel, AuditResult>().ReverseMap();
            CreateMap<UserAuditPlanViewModel, UserAuditPlan>().ReverseMap();
            CreateMap<CreatePracticeViewModel, Practice>().ReverseMap();
            CreateMap<QuizzQuestionViewModel, QuizzQuestion>().ReverseMap();
            CreateMap<SyllabusOutputStandardViewModel, SyllabusOutputStandard>().ReverseMap();
            CreateMap<SyllabusModuleViewModel, SyllabusModule>().ReverseMap();
            CreateMap<ClassDetailsViewModel, Class>().ReverseMap();
            CreateMap<AssignmentCreate, Assignment>().ReverseMap();
            CreateMap<QuizzCreate, Quizz>().ReverseMap();
            CreateMap<LectureCreate, Lecture>().ReverseMap();
            CreateMap<PracticeCreate, Practice>().ReverseMap();
            CreateMap<UnitCreate, Unit>().ReverseMap();
            CreateMap<ModuleCreate, Module>().ReverseMap();
            CreateMap<CreateSyllabusDetailModel, Syllabus>().ReverseMap();
            CreateMap<UpdateStatusOnlyOfSyllabus, Syllabus>().ReverseMap();
            CreateMap<UpdateStatusOnlyOfTrainingProgram, TrainingProgram>().ReverseMap();
            CreateMap<UpdateStatusOnlyOfClass, Class>().ReverseMap();
            /* pagination */
            CreateMap(typeof(Pagination<>), typeof(Pagination<>));
            CreateMap<AbsentRequestViewModel, AbsentRequest>().ReverseMap();
            CreateMap<CreateAbsentRequestViewModel, AbsentRequest>().ReverseMap();
            CreateMap<Attendance, AttendanceViewModel>().ReverseMap();
        }
    }
}
