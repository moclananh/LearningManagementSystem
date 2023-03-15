using Applications;
using Applications.ViewModels.UserViewModels;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Mappers;

public class CreateByResolver : IValueResolver<User,UserViewModel,string>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly AppDBContext _dbContext;
    public CreateByResolver(IUnitOfWork unitOfWork, AppDBContext context)
    {
        _unitOfWork = unitOfWork;
        _dbContext = context;
    }
    public string? Resolve(User source, UserViewModel destination, string destMember, ResolutionContext context)
    {
        var userCreateBy =  _dbContext.Users.Where(x => x.Id == source.CreatedBy).FirstOrDefaultAsync();
        return null;
    }
}