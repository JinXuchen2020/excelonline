using ExcelOnline.Api.Transfers.Base;

namespace ExcelOnline.Api.Transfers
{
    public class UserTokenTransOut : TransOut
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public UserTransOut User { get; set; }
    }
}
