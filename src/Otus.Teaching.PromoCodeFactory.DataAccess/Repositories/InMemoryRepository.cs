using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories;
#nullable disable

public class InMemoryRepository<T>
    : IRepository<T>
    where T : BaseEntity {
    protected IEnumerable<T> Data { get; set; }

    public InMemoryRepository(IEnumerable<T> data) => Data = data;

    public Task<IEnumerable<T>> GetAllAsync() => Task.FromResult(Data);

    public Task<T> GetByIdAsync(Guid id) => Task.FromResult(Data.FirstOrDefault(x => x.Id == id));
}