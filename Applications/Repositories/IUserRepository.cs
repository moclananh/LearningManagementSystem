using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applications.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetUserByEmail(string email);
}
