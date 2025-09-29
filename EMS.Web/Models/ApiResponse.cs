namespace EMS.Web.Models;
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string>? Errors { get; set; }
    public ApiMeta Meta { get; set; } = new();
}

public class ApiMeta
{
    public DateTime TimeStamp { get; set; }
    public string Version { get; set; } = "v1";
    public string? RequestId { get; set; }
}
public class PagedResult<T>
{
    public List<T> Items { get; set; } = [];
    public PaginationMeta Pagination { get; set; } = new();
    public PaginationLinks Links { get; set; } = new();
}

public class PaginationMeta
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
    public bool HasNext { get; set; }
    public bool HasPrevious { get; set; }
}

public class PaginationLinks
{
    public string? Self { get; set; }
    public string? First { get; set; }
    public string? Previous { get; set; }
    public string? Next { get; set; }
    public string? Last { get; set; }
}
