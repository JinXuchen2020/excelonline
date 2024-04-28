using ExcelOnline.Api.Transfers.Base;

namespace ExcelOnline.Api.Transfers
{
    public class UserTransOut : TransOut
    {
        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string Role
        {
            get; set;
        }
    }
}
