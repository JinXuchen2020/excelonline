using ExcelOnline.Api.Options;
using ExcelOnline.Data.Models;

namespace ExcelOnline.Api.Services
{
    public interface ISaleStatusService
    {
        Task<IEnumerable<SaleStatus>> GetSaleInfos(SaleQueryOption option);
        Task<SaleStatus> GetSaleStatus(int id);
        Task AddSaleStatus(SaleStatus input);
        Task UpdateSaleStatus(SaleStatus input);
        Task DeleteSaleStatus(int id);

        Task<bool> ValidateSaleStatus(SaleQueryOption option);
    }    
}
