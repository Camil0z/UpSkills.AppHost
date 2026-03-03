using System.Net;
using System.Text.Json.Serialization;

namespace UpSkills.Models.DTO;

public class ErrorResponse
{
    [JsonPropertyName(nameof(statusCode))] public HttpStatusCode statusCode;
    [JsonPropertyName(nameof(message))] public string message { get; set; } = string.Empty;
}
