using HR_ManagementSystem.Helpers;
using System.Text.Json;

namespace HR_ManagementSystem.Extensions
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader (this HttpResponse response,PaginationHeader header)
        {
            var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            response.Headers.Append("Pagination", JsonSerializer.Serialize(header, jsonOptions));
            response.Headers.Append("Access-Control-Expose-Headers", "Pagination");
        }
    }
}
