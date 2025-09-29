namespace EMS.API.Commond;
public class PagedResult<T>
{
    public List<T> Items { get; set; } = new();
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

public class PagedQuery
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Search { get; set; }
    public string? OrderBy { get; set; }
    public string OrderDirection { get; set; } = "asc";

    public int Skip => (Page - 1) * PageSize;
    public int Take => PageSize;
}
