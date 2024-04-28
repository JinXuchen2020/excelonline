using ExcelOnline.Api.Transfers;

namespace ExcelOnline.Api.Unities
{
    public class Constants
    {
        public static List<ExcelHeader> SaleStatusHeaders = new()
        {
            new ExcelHeader 
            { 
                PropetyName = nameof(SaleStatusTransOut.SalerName), 
                HeaderName = "对接销售",
                HeaderWidth = 10
            },
            new ExcelHeader
            {
                PropetyName = nameof(SaleStatusTransOut.CompanyName),
                HeaderName = "公司名称",
                HeaderWidth = 20
            },
            new ExcelHeader
            {
                PropetyName = nameof(SaleStatusTransOut.BrandName),
                HeaderName = "品牌名称",
                HeaderWidth = 20
            },
            new ExcelHeader
            {
                PropetyName = nameof(SaleStatusTransOut.ShopName),
                HeaderName = "店铺名称",
                HeaderWidth = 20
            },
            new ExcelHeader
            {
                PropetyName = nameof(SaleStatusTransOut.StoreName),
                HeaderName = "业务落地仓",
                HeaderWidth = 20
            },
            new ExcelHeader
            {
                PropetyName = nameof(SaleStatusTransOut.SuccessfulRate),
                HeaderName = "客户成交可能性",
                HeaderWidth = 15
            },
            new ExcelHeader
            {
                HeaderName = "联系人",
                Children = new[]
                {
                    new ExcelHeader
                    {
                        PropetyName = nameof(SaleStatusTransOut.ContactName),
                        HeaderName = "姓名",
                        HeaderWidth = 10
                    },
                    new ExcelHeader
                    {
                        PropetyName = nameof(SaleStatusTransOut.ContactJob),
                        HeaderName = "岗位",
                        HeaderWidth = 10
                    },
                    new ExcelHeader
                    {
                        PropetyName = nameof(SaleStatusTransOut.ContactPhone),
                        HeaderName = "联系方式",
                        HeaderWidth = 10
                    }
                }
            },
            new ExcelHeader 
            { 
                HeaderName = "甘特图进展说明（填写日期）",
                Children = new[]
                {
                    new ExcelHeader
                    {
                        PropetyName = nameof(SaleStatusTransOut.LinkUpDate),
                        HeaderName = "洽谈沟通",
                        HeaderWidth = 15
                    },
                    new ExcelHeader
                    {
                        PropetyName = nameof(SaleStatusTransOut.BidDate),
                        HeaderName = "初步报价",
                        HeaderWidth = 15,
                    },
                    new ExcelHeader
                    {
                        PropetyName = nameof(SaleStatusTransOut.VisitDate),
                        HeaderName = "参观现场",
                        HeaderWidth = 15,
                    },
                    new ExcelHeader
                    {
                        PropetyName = nameof(SaleStatusTransOut.BidConfirmDate),
                        HeaderName = "确定报价",
                        HeaderWidth = 15,
                    },
                    new ExcelHeader
                    {
                        PropetyName = nameof(SaleStatusTransOut.ContractDate),
                        HeaderName = "签订合同",
                        HeaderWidth = 15,
                    },
                    new ExcelHeader
                    {
                        PropetyName = nameof(SaleStatusTransOut.SendDate),
                        HeaderName = "入仓发货",
                        HeaderWidth = 15
                    },
                    new ExcelHeader
                    {
                        PropetyName = nameof(SaleStatusTransOut.RemarkDate),
                        HeaderName = "备注说明",
                        HeaderWidth = 20
                    }
                }
            }            
        };
    }
}
