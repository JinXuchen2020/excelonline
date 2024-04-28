using ExcelOnline.Api.Options.Base;

namespace ExcelOnline.Api.Transfers
{
    public class QueryResponse<T>
    {
        public QueryResponse(QueryOption option, IEnumerable<T> results)
        {
            Total = option.Total ?? 0;
            //Total = results.Count();
            Data = results;
        }

        public int Total { get; set; }

        public IEnumerable<T> Data { get; set; }
    }

    public class QueryResponse
    {
        public static QueryResponse<T> New<T>(QueryOption option, IEnumerable<T> results)
            => new(option, results);
    }
}
