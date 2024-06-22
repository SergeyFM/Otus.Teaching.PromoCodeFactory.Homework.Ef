using Microsoft.EntityFrameworkCore;
using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain;
using Otus.Teaching.PromoCodeFactory.DataAccess.Data;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories;
internal class EfRepository<T> : IRepository<T> where T : BaseEntity {
    private readonly DatabaseContext _context;
    private readonly DbSet<T> _dbSet;

    public EfRepository(DatabaseContext context) {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

    public async Task<T> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id) ?? throw new Exception($"ID {id} not found!");
}
