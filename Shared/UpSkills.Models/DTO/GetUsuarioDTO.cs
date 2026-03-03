using System.Text.Json.Serialization;

namespace UpSkills.Models.DTO;

public class GetUsuarioDTO
{
    [JsonPropertyName(nameof(Id))] public long Id { get; set; }
    [JsonPropertyName(nameof(Activo))] public bool Activo { get; set; } = false;
    [JsonPropertyName(nameof(PrimerNombre))] public string PrimerNombre { get; set; } = string.Empty;
    [JsonPropertyName(nameof(SegundoNombre))] public string SegundoNombre { get; set; } = string.Empty;
    [JsonPropertyName(nameof(PrimerApellido))] public string PrimerApellido { get; set; } = string.Empty;
    [JsonPropertyName(nameof(SegundoApellido))] public string SegundoApellido { get; set; } = string.Empty;
    [JsonPropertyName(nameof(Correo))] public string Correo { get; set; } = string.Empty;
    [JsonPropertyName(nameof(Pais))] public long? Pais { get; set; } = null;
    [JsonPropertyName(nameof(Rol))] public long Rol { get; set; }
    [JsonPropertyName(nameof(CreadoEn))] public DateTime CreadoEn { get; set; }
}