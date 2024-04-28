using ExcelOnline.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection.Emit;

namespace ExcelOnline.Data.DbContexts
{
    public class AppDbContext : DbContext
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public AppDbContext(
            DbContextOptions<AppDbContext> options,
            IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                    .Property(u => u.Id)
                    .UseIdentityColumn()
                    .ValueGeneratedOnAdd();
            builder.Entity<User>().HasData(new User
            {
                Id = 1,
                Name = "管理员",
                PhoneNumber = "1234567",
                Role = "admin"
            });
            builder.Entity<SaleStatus>()
                    .Property(u => u.Id)
                    .UseIdentityColumn()
                    .ValueGeneratedOnAdd();
            builder.Entity<SaleStatus>()
                .Property(c=> c.LinkUpDate).HasColumnType("datetime");
            builder.Entity<SaleStatus>()
                .Property(c => c.BidDate).HasColumnType("datetime");
            builder.Entity<SaleStatus>()
                .Property(c => c.VisitDate).HasColumnType("datetime");
            builder.Entity<SaleStatus>()
                .Property(c => c.BidConfirmDate).HasColumnType("datetime");
            builder.Entity<SaleStatus>()
                .Property(c => c.ContractDate).HasColumnType("datetime");
            builder.Entity<SaleStatus>()
                .Property(c => c.SendDate).HasColumnType("datetime");
            builder.Entity<SaleStatus>()
                .Property(c => c.RemarkDate).HasColumnType("datetime");
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is BaseModel entity)
                {
                    switch (entry.State)
                    {
                        case EntityState.Deleted:
                            entry.State = EntityState.Modified;
                            RecordModifiedInfo(entity);
                            MarkAsDeleted(entity);
                            break;
                        case EntityState.Modified:
                            RecordModifiedInfo(entity);
                            break;
                        case EntityState.Added:
                            RecordCreatedInfo(entity);
                            break;
                        default:
                            break;
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        private void RecordCreatedInfo<T>(T entity) where T : BaseModel
        {
        }

        private void RecordModifiedInfo<T>(T entity) where T : BaseModel
        {
        }

        private static void MarkAsDeleted<T>(T entity) where T : BaseModel
        {
        }
    }
}
