using Applications;
using Applications.ViewModels.UserViewModels;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Mappers.UserMapperResovlers;

public class CreateByResolver : IValueResolver<User,UserViewModel,string>
{
    private readonly IUnitOfWork _unitOfWork;
    public CreateByResolver(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public string Resolve(User source, UserViewModel destination, string destMember, ResolutionContext context)
    {
        if (source.CreatedBy == Guid.Empty) return null;
        //var user = _dbContext.Users.SingleOrDefaultAsync(x => x.Id == source.CreatedBy).Result;
        var user = _unitOfWork.UserRepository.GetByIdAsync(source.CreatedBy).Result;
        if (user is null) return $"User information not found id : {source.CreatedBy} ";
        return user.Email;
    }
}