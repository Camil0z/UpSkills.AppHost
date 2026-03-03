using System.Net;
using System.Text.Json.Serialization;

namespace UpSkills.Models.DTO;

public class ApiResponse
{
    [JsonPropertyName(nameof(statusCode))] public HttpStatusCode statusCode;
    
    [JsonPropertyName(nameof(value))] public object? value;

    /// <summary>
    /// Used for the Api responses
    /// </summary>
    /// <param name="statusCode"></param>
    /// <param name="value"></param>
    public ApiResponse(HttpStatusCode statusCode, object? value)
    {
        this.statusCode = statusCode;
        this.value = value;
    }

    /// <summary>
    /// Used for the Api error responses.
    /// </summary>
    /// <param name="statusCode">A <see cref="HttpStatusCode"/> error code.</param>
    /// <param name="message">The error message <see cref="string"/> detail.</param>
    public ApiResponse(HttpStatusCode statusCode, string message)
    {
        this.statusCode = statusCode;
        this.value = new { statusCode, message };
    }
}
