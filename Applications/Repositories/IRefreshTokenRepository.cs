using System.Linq.Expressions;
using Domain.Entities;

namespace Applications.Repositories;

public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken entity);
    void Update(RefreshToken entity);
    Task<List<RefreshToken>> Find(Expression<Func<RefreshToken, bool>> expression);
}