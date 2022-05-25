namespace Hao.Authentication.Domain.Paging
{
    public class ResponseResult<T>
    {
        public T Data { get; set; }
        public int StatusCode { get; set; } = 200;
        public List<string> Messages { get; set; } = new List<string>();
        public bool Success { get; set; }
        public string AllMessages { get => string.Join('\n', Messages); }

        public void AddError(Exception e)
        {
            Success = false;
            StatusCode = 500;
            Messages.Add(e.Message);
        }

        public void AddMessage(string msg)
        {
            if(!string.IsNullOrEmpty(msg)) Messages.Add(msg);
        }
    }

    public class ResponsePagingResult<T>
    {
        public List<T> Data { get; set; } = new List<T>();

        public int RowsCount { get; set; }

        public int StatusCode { get; set; } = 200;

        public List<string> Messages { get; set; } = new List<string>();

        public bool Success { get; set; }

        public string AllMessages { get => string.Join('\n', Messages); }

        public void AddError(Exception e)
        {
            Success = false;
            StatusCode = 500;
            Messages.Add(e.Message);
        }

        public void AddMessage(string msg)
        {
            if (!string.IsNullOrEmpty(msg)) Messages.Add(msg);
        }
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
}
