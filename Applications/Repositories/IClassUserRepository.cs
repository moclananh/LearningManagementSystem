using Applications.Repositories;
using Domain.Entities;
using Domain.EntityRelationship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IClassUserRepository : IGenericRepository<ClassUser>
    {
        Task UploadClassUserListAsync(List<ClassUser> classUser);
    }
}
