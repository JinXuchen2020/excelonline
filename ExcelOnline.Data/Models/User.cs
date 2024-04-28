using System.ComponentModel.DataAnnotations;

namespace ExcelOnline.Data.Models
{
    public class User : BaseModel
    {
        [MaxLength(512)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string PhoneNumber { get; set; }

        [MaxLength(100)]
        public string Role
        {
            get; set;
        }
    }
}
