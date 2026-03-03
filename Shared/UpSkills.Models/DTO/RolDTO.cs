using System.Text.Json.Serialization;

namespace UpSkills.Models.DTO;

public class RolDTO
{
    [JsonPropertyName(nameof(Id))] public long? Id { get; set; } = null;
    [JsonPropertyName(nameof(Nombre))] public string? Nombre { get; set; } = null;
}
