namespace Hao.Authentication.Domain.Paging
{
    public class BaseResponseResult
    {
        public int StatusCode { get; set; } = 200;
        public List<string> Messages { get; set; } = new List<string>();
        public bool Success { get; set; } = true;
        public string AllMessages { get => string.Join('\n', Messages); }

        public void AddError(Exception e)
        {
            Success = false;
            StatusCode = 500;
            Messages.Add($"{e.Message} {e.InnerException?.Message}");
        }

        public void AddMessage(string msg)
        {
            if (!string.IsNullOrEmpty(msg)) Messages.Add(msg);
        }
    }

    public class ResponseResult<T>: BaseResponseResult
    {
        public T Data { get; set; }
    }

    public class ResponsePagingResult<T>: BaseResponseResult
    {
        public List<T> Data { get; set; } = new List<T>();

        public int RowsCount { get; set; }
    }

    public class PagingParameter<T>
    {
        public T Filter { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public string Sort { get; set; }

        public string SortColumn { get; set; }
    }

    public class OptionItem<T>
    {
        public T Key { get; set; }

        public string Value { get; set; }
    }

    public static class PagingProfile
    {
        public static IQueryable<T> AsPaging<T>(this IQueryable<T> query, int pageIndex = 1, int pageSize = 10)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            if (pageIndex <= 0) pageIndex = 1;


            if (pageSize <= 1) pageSize = 10;

            return query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }
    }
}
