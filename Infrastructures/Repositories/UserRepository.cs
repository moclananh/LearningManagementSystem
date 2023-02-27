using Applications.Commons;
using Applications.Interfaces;
using Applications.Repositories;
using Domain.Entities;
using Domain.Enum.RoleEnum;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using System.Reflection;

namespace Infrastructures.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
	private AppDBContext _dbContext;
	public UserRepository(AppDBContext context, ICurrentTime currentTime, IClaimService claimService) : base(context, currentTime, claimService)
	{
		_dbContext = context;
	}

	public async Task<User?> GetUserByEmail(string email) => _dbContext.Users.FirstOrDefault(x => x.Email == email);

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

   
	public async Task<Pagination<User>> GetUserByClassId(Guid ClassId, int pageNumber = 0, int pageSize = 10)
	{
        var itemCount = await _dbContext.Users.CountAsync();
        var items = await _dbContext.ClassUser.Where(x => x.ClassId.Equals(ClassId))
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