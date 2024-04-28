using ExcelOnline.Api.Transfers.Base;

namespace ExcelOnline.Api.Transfers
{
    public class SaleStatusTransOut : TransOut
    {
        public string CompanyName { get; set; }

        public string? BrandName { get; set; }
        public string? ShopName { get; set; }
        public string? StoreName { get; set; }
        public string? ContactName { get; set; }
        public string? ContactJob { get; set; }
        public string? ContactPhone { get; set; }
        public string? SalerName { get; set; }
        public decimal? SuccessfulRate { get; set; }
        public string? LinkUpDate { get; set; }
        public string? BidDate { get; set; }
        public string? VisitDate { get; set; }
        public string? BidConfirmDate { get; set; }
        public string? ContractDate { get; set; }
        public string? SendDate { get; set; }
        public string? RemarkDate { get; set; }
    }
}
