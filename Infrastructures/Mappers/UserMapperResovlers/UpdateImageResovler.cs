using Applications.ViewModels.UserViewModels;
using AutoMapper;
using Domain.Entities;

namespace Infrastructures.Mappers.UserMapperResovlers;

public class UpdateImageResovler : IValueResolver<UpdateUserViewModel,User,string>
{
    public string Resolve(UpdateUserViewModel source, User destination, string destMember, ResolutionContext context)
    {
        if (source.Image == null || source.Image == string.Empty || source.Image == "")
        {
            return destination.Image;
        }

        return source.Image;
    }
}