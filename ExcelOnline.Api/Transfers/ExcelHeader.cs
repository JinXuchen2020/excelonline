using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExcelOnline.Api.Transfers
{
    public class ExcelHeader
    {
        public string PropetyName { get; set; }

        public string HeaderName { get; set; }

        public int HeaderWidth { get; set; }

        public bool? Wrap { get; set; }

        public IEnumerable<ExcelHeader> Children { get; set; }= new List<ExcelHeader>();

        public IEnumerable<string> Constraints { get; set; }
    }
}
