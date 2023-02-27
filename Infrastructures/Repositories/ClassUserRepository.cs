using Application.Repositories;
using Applications.Interfaces;
using Applications.Repositories;
using Domain.Entities;
using Domain.EntityRelationship;
using Infrastructures;
using Infrastructures.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ClassUserRepository : GenericRepository<ClassUser>, IClassUserRepository
    {
        private readonly AppDBContext _dbContext;
        public ClassUserRepository(AppDBContext dbContext, ICurrentTime currentTime, IClaimService claimService) : base(dbContext, currentTime, claimService)
        {
            _dbContext = dbContext;
        }
        public async Task UploadClassUserListAsync(List<ClassUser> classUser) => await _dbContext.AddRangeAsync(classUser);
    }
}
