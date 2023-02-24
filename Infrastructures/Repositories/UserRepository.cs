using Applications.Interfaces;
using Applications.Repositories;
using Domain.Entities;


namespace Infrastructures.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
	private AppDBContext _dbContext;
	public UserRepository(AppDBContext context, ICurrentTime currentTime, IClaimService claimService) : base(context, currentTime, claimService)
	{
		_dbContext = context;
	}

	public async Task<User?> GetUserByEmail(string email) => _dbContext.Users.FirstOrDefault(x => x.Email == email);
}