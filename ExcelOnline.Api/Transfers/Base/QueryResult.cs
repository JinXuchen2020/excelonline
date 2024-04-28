using ExcelOnline.Api.Options.Base;

namespace ExcelOnline.Api.Transfers.Base
{
    public class QueryResult<TLiTransOut> where TLiTransOut : TransOut
    {
        public int Total { get; set; }

        public IEnumerable<TLiTransOut> Results { get; set; }

        public QueryResult(IEnumerable<TLiTransOut> results, QueryOption option)
        {
            Total = option.Total ?? 0;
            Results = results;
        }
    }
}
