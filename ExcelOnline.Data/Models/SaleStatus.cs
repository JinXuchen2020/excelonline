using System.ComponentModel.DataAnnotations;

namespace ExcelOnline.Data.Models
{
    public class SaleStatus : BaseModel
    {
        [MaxLength(1024)]
        public string CompanyName { get; set; }
        [MaxLength(1024)]
        public string? BrandName { get; set; }
        [MaxLength(1024)]
        public string? ShopName { get; set; }
        [MaxLength(1024)]
        public string? StoreName { get; set; }
        [MaxLength(1024)]
        public string? ContactName { get; set; }
        [MaxLength(1024)]
        public string? ContactJob { get; set; }
        [MaxLength(1024)]
        public string? ContactPhone { get; set; }
        [MaxLength(1024)]
        public string? SalerName { get; set; }
        public decimal? SuccessfulRate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? LinkUpDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? BidDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? VisitDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? BidConfirmDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? ContractDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? SendDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? RemarkDate { get; set; }

        public bool IsEditing { get; set; } = false;
    }
}
