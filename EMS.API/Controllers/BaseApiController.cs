using EMS.API.Commond;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    public abstract class BaseApiController : ControllerBase
    {
        protected ActionResult<ApiResponse<T>> Success<T>(T data, string message = "Operation completed successfully")
        {
            var response = ApiResponse<T>.SuccessResponse(data, message);
            response.Meta.RequestId = HttpContext.TraceIdentifier;
            return Ok(response);
        }

        protected ActionResult<ApiResponse<T>> Error<T>(string message, List<string>? errors = null, int statusCode = 400)
        {
            var response = ApiResponse<T>.ErrorResponse(message, errors);
            response.Meta.RequestId = HttpContext.TraceIdentifier;

            return statusCode switch
            {
                400 => BadRequest(response),
                401 => Unauthorized(response),
                403 => StatusCode(403, response),
                404 => NotFound(response),
                409 => Conflict(response),
                500 => StatusCode(500, response),
                _ => BadRequest(response)
            };
        }

        protected PagedResult<T> CreatePagedResult<T>(List<T> items, int totalItems, PagedQuery query, string baseUrl)
        {
            var totalPages = (int)Math.Ceiling((double)totalItems / query.PageSize);

            return new PagedResult<T>
            {
                Items = items,
                Pagination = new PaginationMeta
                {
                    Page = query.Page,
                    PageSize = query.PageSize,
                    TotalItems = totalItems,
                    TotalPages = totalPages,
                    HasNext = query.Page < totalPages,
                    HasPrevious = query.Page > 1
                },
                Links = new PaginationLinks
                {
                    Self = $"{baseUrl}?page={query.Page}&pageSize={query.PageSize}",
                    First = $"{baseUrl}?page=1&pageSize={query.PageSize}",
                    Previous = query.Page > 1 ? $"{baseUrl}?page={query.Page - 1}&pageSize={query.PageSize}" : null,
                    Next = query.Page < totalPages ? $"{baseUrl}?page={query.Page + 1}&pageSize={query.PageSize}" : null,
                    Last = $"{baseUrl}?page={totalPages}&pageSize={query.PageSize}"
                }
            };
        }
    }
}
