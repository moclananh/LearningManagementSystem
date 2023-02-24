using Applications.Interfaces;
using Applications.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repositories
{
    public class AssignmentQuestionRepository : GenericRepository<AssignmentQuestion>, IAssignmentQuestionRepository
    {
        private readonly AppDBContext _dbContext;
        public AssignmentQuestionRepository(AppDBContext dbContext,
            ICurrentTime currentTime,
            IClaimService claimService) : base(dbContext, currentTime, claimService)
        {
            _dbContext = dbContext;
        }
        public async Task<List<AssignmentQuestion>> GetAllAssignmentQuestionByAssignmentId(Guid AssignmentId)
        {
            return await _dbContext.AssignmentQuestions.Where(a => a.AssignmentId == AssignmentId).ToListAsync();
        }
    }
}
