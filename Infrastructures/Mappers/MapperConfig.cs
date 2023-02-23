using Applications.ViewModels.AssignmentViewModels;
using Applications.ViewModels.UserViewModels;
using Applications.ViewModels.ClassViewModels;
using AutoMapper;
using Domain.Entities;
using Application.ViewModels.QuizzViewModels;

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
        }
    }
}
