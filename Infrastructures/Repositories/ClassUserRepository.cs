using Application.Repositories;
using Applications.Interfaces;
using Domain.EntityRelationship;
using Infrastructures;
using Infrastructures.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ClassUserRepository : GenericRepository<ClassUser>, IClassUserRepository
    {
        private readonly AppDBContext _dbContext;
        public ClassUserRepository(AppDBContext dbContext, ICurrentTime currentTime, IClaimService claimService) : base(dbContext, currentTime, claimService)
        {
            _dbContext = dbContext;
        }
        public async Task<List<ClassUser>> GetClassUserListByClassId(Guid ClassId)
        {
            return await _dbContext.ClassUser.Where(x => x.ClassId == ClassId).ToListAsync();
        }
        public async Task<ClassUser> GetClassUser(Guid ClassId, Guid UserId) => await _dbContext.ClassUser.FirstOrDefaultAsync(x => x.ClassId == ClassId && x.UserId == UserId);

        public async Task UploadClassUserListAsync(List<ClassUser> classUser) => await _dbContext.AddRangeAsync(classUser);
    }
}
