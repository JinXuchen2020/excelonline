using ExcelOnline.Api.Options;
using ExcelOnline.Data.Models;
using ExcelOnline.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ExcelOnline.Api.Services.Implementations
{
    public class SaleStatusService : ISaleStatusService
    {
        protected readonly IRepository repository;

        protected readonly IHttpContextAccessor httpContextAccessor;

        public IRepository Repository { get => this.repository; }

        public SaleStatusService(IRepository repository, IHttpContextAccessor httpContextAccessor) 
        {
            this.repository = repository;
            this.httpContextAccessor = httpContextAccessor;
        }        

        public async Task<IEnumerable<SaleStatus>> GetSaleInfos(SaleQueryOption option)
        {
            var query = repository.GetQuery<SaleStatus>().AsNoTracking();
            if (!string.IsNullOrEmpty(option.CompanyName)) 
            {
                query = query.Where(c => c.CompanyName.Contains(option.CompanyName));
            }

            if (!string.IsNullOrEmpty(option.SalerName))
            {
                query = query.Where(c => !string.IsNullOrEmpty(c.SalerName) && c.SalerName.Contains(option.SalerName));
            }

            option.Total = query.Count();

            if (option.PageSize.HasValue)
            {
                query = query.Skip(((option.PageNo ?? 1) - 1) * option.PageSize.Value).Take(option.PageSize.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<SaleStatus> GetSaleStatus(int id)
        {
            return await repository.GetQuery<SaleStatus>().Where(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddSaleStatus(SaleStatus input)
        {
            await repository.AddAsync(input);
        }

        public async Task UpdateSaleStatus(SaleStatus input)
        {
            await repository.UpdateAsync(input);
        }

        public async Task DeleteSaleStatus(int id)
        {
            var result = await this.repository.GetQuery<SaleStatus>()
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();
            if (result != null)
            {
                await this.repository.RemoveAsync(result);
            }
        }

        public async Task<bool> ValidateSaleStatus(SaleQueryOption option)
        {
            var result = this.repository.GetQuery<SaleStatus>();
            if (!string.IsNullOrEmpty(option.CompanyName) && string.IsNullOrEmpty(option.ShopName))
            {
                result = result.Where(c => c.CompanyName == option.CompanyName);
            }
            else if (string.IsNullOrEmpty(option.CompanyName) && !string.IsNullOrEmpty(option.ShopName))
            {
                result = result.Where(c => c.ShopName == option.ShopName);
            }
            else
            {
                result = result.Where(c => c.CompanyName == option.CompanyName || c.ShopName == option.ShopName);
            }

            return (await result.FirstOrDefaultAsync()) != null;
        }
    }
}
