using Applications.ViewModels.AssignmentViewModels;
using Applications.ViewModels.UserViewModels;
using Applications.ViewModels.ClassViewModels;
using Applications.ViewModels.ModuleViewModels;
using AutoMapper;
using Domain.Entities;
using Application.ViewModels.QuizzViewModels;
using Applications.ViewModels.LectureViewModels;
using Application.ViewModels.UnitViewModels;


namespace Infrastructures.Mappers
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<ClassViewModel, Class>().ReverseMap();
            CreateMap<UpdateAssignmentViewModel, Assignment>().ReverseMap();
            CreateMap<CreateQuizzViewModel, Quizz>().ReverseMap();
            CreateMap<QuizzViewModel, Quizz>().ReverseMap();
            CreateMap<UpdateQuizzViewModel, Quizz>().ReverseMap();
            CreateMap<Quizz, QuizzViewModel>()
               .ForMember(dest => dest.QuizzId, src => src.MapFrom(x => x.Id));
            CreateMap<UserViewModel, User>().ReverseMap();
            CreateMap<UpdateUserViewModel, User>().ReverseMap();
            CreateMap<CreateLectureViewModel, Lecture>().ReverseMap();
            CreateMap<UpdateLectureViewModel, Lecture>().ReverseMap();
            CreateMap<LectureViewModel, Lecture>().ReverseMap();
            CreateMap<CreateUnitViewModel, Unit>().ReverseMap();
            CreateMap<UnitViewModel, Unit>().ReverseMap();
            CreateMap<Unit, UnitViewModel>()
               .ForMember(dest => dest.UnitId, src => src.MapFrom(x => x.Id));
            CreateMap<ModuleViewModels, Module>().ReverseMap();
        }
    }
}
