namespace EMS.API.Commond;
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = [];
    public ApiMeta Meta { get; set; } = new();

    public static ApiResponse<T> SuccessResponse(T data, string message = "Operation completed successfully")
    {
        return new ApiResponse<T>
        {
            Success = true,
            Data = data,
            Message = message,
            Meta = new ApiMeta
            {
                TimeStamp = DateTime.UtcNow,
                Version = "v1",
            }
        };
    }
    public static ApiResponse<T> ErrorResponse(string message, List<string>? errors = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Errors = errors,
            Meta = new ApiMeta
            {
                TimeStamp = DateTime.UtcNow,
                Version = "v1",
            }
        };
    }
}
public class ApiMeta
{
    public DateTime TimeStamp { get; set; }
    public string Version { get; set; } = "v1";
    public string? RequestId { get; set; }
}
