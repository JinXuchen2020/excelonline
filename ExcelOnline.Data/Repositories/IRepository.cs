using ExcelOnline.Data.DbContexts;
using ExcelOnline.Data.Models;
using System.Linq.Expressions;

namespace ExcelOnline.Data.Repositories
{
    public interface IRepository
    {
        AppDbContext Context { get; }

        Task UseTransactionAsync(Func<Task> operation);

        Task<int> AddAsync<T>(T entity, CancellationToken cancellationToken = default) where T : BaseModel;

        Task<int> AddRangeAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default) where T : BaseModel;

        Task<int> UpdateAsync<T>(T entity, CancellationToken cancellationToken = default) where T : BaseModel;

        Task<int> UpdateRangeAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default) where T : BaseModel;

        Task<int> RemoveAsync<T>(int id, CancellationToken cancellationToken = default) where T : BaseModel, new();

        Task<int> RemoveAsync<T>(T entity, CancellationToken cancellationToken = default) where T : BaseModel, new();

        Task<int> RemoveRangeAsync<T>(IEnumerable<int> ids, CancellationToken cancellationToken = default) where T : BaseModel, new();

        Task<int> RemoveRangeAsync<T>(IEnumerable<T> models, CancellationToken cancellationToken = default) where T : BaseModel, new();

        Task<bool> AnyAsync<T>(Expression<Func<T, bool>> predicate = null, CancellationToken cancellationToken = default) where T : BaseModel;

        Task<T> GetAsync<T>(int id, CancellationToken cancellationToken = default) where T : BaseModel;

        IQueryable<T> GetQuery<T>() where T : BaseModel;
    }
}
