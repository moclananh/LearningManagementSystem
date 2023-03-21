using Applications.Commons;
using Applications.Interfaces;
using Applications.Repositories;
using Domain.Entities;
using Domain.Enum.RoleEnum;
using Microsoft.EntityFrameworkCore;

using Applications.ViewModels.UserViewModels;

namespace Infrastructures.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
	private AppDBContext _dbContext;
	public UserRepository(AppDBContext context, ICurrentTime currentTime, IClaimService claimService) : base(context, currentTime, claimService)
	{
		_dbContext = context;
	}

	public async Task<User?> GetUserByEmail(string email) => _dbContext.Users.FirstOrDefault(x => x.Email == email);

    public async Task<User?> GetUserByPasswordResetToken(string token) => _dbContext.Users.FirstOrDefault(u => u.PasswordResetToken == token);
    public async Task<Pagination<User?>> GetUsersByRole(Role role, int pageNumber = 0, int pageSize = 10)
    {
        var itemCount = await _dbContext.Users.CountAsync();
        var items = await _dbContext.Users.Where(x => x.Role == role)
            .OrderByDescending(x => x.CreationDate)
            .Skip(pageNumber * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();

        var result = new Pagination<User>()
        {
            PageIndex = pageNumber,
            PageSize = pageSize,
            TotalItemsCount= itemCount,
            Items = items
        };
        return result;
    }

    public async Task<Pagination<User>> SearchUserByName(string name, int pageNumber = 0, int pageSize = 10)
    {
        var itemCount = await _dbContext.Users.CountAsync();
        var items = await _dbContext.Users.Where(x => x.firstName.ToLower().Contains(name.ToLower()) || x.lastName.ToLower().Contains(name.ToLower()))
            .OrderByDescending(x => x.CreationDate)
            .Skip(pageNumber *pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();

        var result = new Pagination<User>()
        {
            PageIndex = pageNumber,
            PageSize = pageSize,
            TotalItemsCount = itemCount,
            Items = items
        };
        return result;
    }

    public async Task<Pagination<User>> FilterUser(FilterUserRequest filterUserRequest,int pageNumber = 0, int pageSize = 10)
    {
        var itemCount = await _dbContext.Users.CountAsync();
        var query =  _dbContext.Users.AsQueryable();
        if(filterUserRequest.FullName is not null)
            query = query.Where(x => x.firstName.ToLower().Contains(filterUserRequest.FullName.ToLower()) || x.lastName.ToLower().Contains(filterUserRequest.FullName.ToLower()));
        if(filterUserRequest.Email is not null)
            query = query.Where(user => user.Email.ToLower().Contains(filterUserRequest.Email.ToLower()));
        if(filterUserRequest.DOB is not null)
            query = query.Where(user => user.DOB.Equals(filterUserRequest.DOB));
        query = filterUserRequest.Roles.TakeWhile(role => role.HasValue).Aggregate(query, (current, role) => current.Where(user => user.Role == role));
        query = filterUserRequest.OverallStatus.TakeWhile(overallStatus => overallStatus.HasValue).Aggregate(query, (current, overallStatus) => current.Where(user => user.OverallStatus == overallStatus));
        query = filterUserRequest.Genders.TakeWhile(gender => gender.HasValue).Aggregate(query, (current, gender) => current.Where(user => user.Gender == gender));
        foreach (var level in filterUserRequest.Levels)
        {
            if(!level.HasValue) break;
            query = query.Where(user => user.Level == level);
        }
        var items = await query
                .OrderByDescending(x => x.CreationDate)
                .Skip(pageNumber *pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

        var result = new Pagination<User>()
        {
            PageIndex = pageNumber,
            PageSize = pageSize,
            TotalItemsCount = itemCount,
            Items = items
        };
        return result;
    }


    public async Task<Pagination<User>> GetUserByClassId(Guid classId, int pageNumber = 0, int pageSize = 10)
	{
        var itemCount = await _dbContext.Users.CountAsync();
        var items = await _dbContext.ClassUser.Where(x => x.ClassId.Equals(classId))
                                .Select(x => x.User)
                                .OrderByDescending(x => x.CreationDate)
                                .Skip(pageNumber * pageSize)
                                .Take(pageSize)
                                .AsNoTracking()
                                .ToListAsync();

        var result = new Pagination<User>()
        {
            PageIndex = pageNumber,
            PageSize = pageSize,
            TotalItemsCount = itemCount,
            Items = items,
        };

        return result;
    }

}