using ExcelOnline.Api.Options.Base;

namespace ExcelOnline.Api.Options
{
    public class UserQueryOption: QueryOption
    {
        public string? Name { get; set; }

        public string? PhoneNumber { get; set; }
    }
}
