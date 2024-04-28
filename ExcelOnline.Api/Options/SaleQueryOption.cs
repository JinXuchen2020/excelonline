using ExcelOnline.Api.Options.Base;

namespace ExcelOnline.Api.Options
{
    public class SaleQueryOption : QueryOption
    {
        public string? CompanyName { get; set; }

        public string? ShopName { get; set; }

        public string? SalerName { get; set; }  
    }
}
