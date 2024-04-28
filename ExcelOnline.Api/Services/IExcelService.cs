using ExcelOnline.Api.Transfers;
using NPOI.SS.UserModel;

namespace ExcelOnline.Api.Services
{
    public interface IExcelService
    {
        Task<IEnumerable<T>> ImportFile<T>(IFormFile file, List<ExcelHeader> expectHeaders);

        Task<byte[]> CreateExcel<T>(string sheetName, List<ExcelHeader> headers, IEnumerable<T> dataList);

        Task<List<T>> ParseExcel<T>(ISheet sheet, List<ExcelHeader> expectHeaders);
    }
}
