using System.Linq.Expressions;
using Applications.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructures.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AppDBContext _context;
    public RefreshTokenRepository(AppDBContext context)
    {
        _context = context;
    }
    public async Task AddAsync(RefreshToken entity)
    {
        await _context.RefreshTokens.AddAsync(entity);
    }
    public void Update(RefreshToken entity)
    {
        _context.RefreshTokens.Update(entity);
    }
    public async Task<List<RefreshToken>> Find(Expression<Func<RefreshToken, bool>> expression)
    {
        return await _context.RefreshTokens.Where(expression).ToListAsync();
    }
    
}