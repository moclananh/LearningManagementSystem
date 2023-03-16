using Applications.ViewModels.UserViewModels;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Mappers.UserMapperResovlers;

public class CreateByResolver : IValueResolver<User,UserViewModel,string>
{
    private readonly AppDBContext _dbContext;
    public CreateByResolver(AppDBContext context)
    {
        _dbContext = context;
    }
    public string Resolve(User source, UserViewModel destination, string destMember, ResolutionContext context)
    {
        if (source.CreatedBy == Guid.Empty) return null;
        var user = _dbContext.Users.SingleOrDefaultAsync(x => x.Id == source.CreatedBy).Result;
        return user.Email;
    }
}