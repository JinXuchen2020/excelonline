using ExcelOnline.Data.DbContexts;
using ExcelOnline.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ExcelOnline.Data.Repositories.Implementations
{
    public class Repository : IRepository
    {
        public AppDbContext Context { get; }

        public Repository(AppDbContext context)
        {
            Context = context;
            context.Database.EnsureCreated();
        }

        public async Task UseTransactionAsync(Func<Task> operation)
        {
            using var transaction = Context.Database.BeginTransaction();

            try
            {
                await operation();

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Dispose();
                throw;
            }
        }

        public async Task<int> AddAsync<T>(T entity, CancellationToken cancellationToken = default) where T : BaseModel
        {
            Context.Add(entity);
            return await Context.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> AddRangeAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default) where T : BaseModel
        {
            Context.AddRange(entities);
            return await Context.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> UpdateAsync<T>(T entity, CancellationToken cancellationToken = default) where T : BaseModel
        {
            Context.Update(entity);
            return await Context.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> UpdateRangeAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default) where T : BaseModel
        {
            Context.UpdateRange(entities);
            return await Context.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> RemoveAsync<T>(int id, CancellationToken cancellationToken = default) where T : BaseModel, new()
        {
            var obj = new T { Id = id };
            Context.Attach(obj);
            Context.Remove(obj);
            return await Context.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> RemoveAsync<T>(T entity, CancellationToken cancellationToken = default) where T : BaseModel, new()
        {
            Context.Remove(entity);
            return await Context.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> RemoveRangeAsync<T>(IEnumerable<int> ids, CancellationToken cancellationToken = default) where T : BaseModel, new()
        {
            var objs = ids.Select(id => new T { Id = id });
            Context.AttachRange(objs);
            Context.RemoveRange(objs);
            return await Context.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> RemoveRangeAsync<T>(IEnumerable<T> models, CancellationToken cancellationToken = default) where T : BaseModel, new()
        {
            Context.RemoveRange(models);
            return await Context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> AnyAsync<T>(Expression<Func<T, bool>> predicate = null, CancellationToken cancellationToken = default) where T : BaseModel
        {
            return await Context.Set<T>().AsNoTracking().AnyAsync(predicate, cancellationToken);
        }

        public async Task<T> GetAsync<T>(int id, CancellationToken cancellationToken = default) where T : BaseModel
        {
            return await Context.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public IQueryable<T> GetQuery<T>() where T : BaseModel
        {
            return Context.Set<T>().AsNoTracking();
        }
    }
}
