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
using Applications.ViewModels.SyllabusViewModels;

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
            CreateMap<CreateQuizzViewModel, Quizz>().ReverseMap();
            CreateMap<QuizzViewModel, Quizz>().ReverseMap();
            CreateMap<UpdateQuizzViewModel, Quizz>().ReverseMap();
            CreateMap<Quizz, QuizzViewModel>()
               .ForMember(dest => dest.QuizzId, src => src.MapFrom(x => x.Id));
            CreateMap<UserViewModel, User>().ReverseMap();
            CreateMap<UpdateUserViewModel, User>().ReverseMap();
            CreateMap<CreateClassUserViewModel, ClassUser>().ReverseMap();
            CreateMap<AuditPlanViewModel, AuditPlan>().ReverseMap();
            CreateMap<UpdateAuditPlanViewModel, AuditPlan>().ReverseMap();
            CreateMap<CreateLectureViewModel, Lecture>().ReverseMap();
            CreateMap<UpdateLectureViewModel, Lecture>().ReverseMap();
            CreateMap<LectureViewModel, Lecture>().ReverseMap();
            CreateMap<CreateUnitViewModel, Unit>().ReverseMap();
            CreateMap<UnitViewModel, Unit>().ReverseMap();
            CreateMap<Unit, UnitViewModel>()
               .ForMember(dest => dest.UnitId, src => src.MapFrom(x => x.Id));
            CreateMap<ModuleViewModels, Module>().ReverseMap();
            CreateMap<ViewTrainingProgram, TrainingProgram>().ReverseMap();
            CreateMap<CreateSyllabusViewModel, Syllabus>().ReverseMap();
            CreateMap<UpdateSyllabusViewModel, Syllabus>().ReverseMap();
            CreateMap<SyllabusViewModel, Syllabus>().ReverseMap();
        }
    }
}
