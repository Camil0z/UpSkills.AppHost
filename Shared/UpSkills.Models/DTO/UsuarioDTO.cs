using System.Text.Json.Serialization;

namespace UpSkills.Models.DTO;

public class UsuarioDTO
{
    [JsonPropertyName(nameof(Id))] public long? Id { get; set; } = null;
    [JsonPropertyName(nameof(Activo))] public bool? Activo { get; set; } = null;
    [JsonPropertyName(nameof(PrimerNombre))] public string? PrimerNombre { get; set; } = null;
    [JsonPropertyName(nameof(SegundoNombre))] public string? SegundoNombre { get; set; } = null;
    [JsonPropertyName(nameof(PrimerApellido))] public string? PrimerApellido { get; set; } = null;
    [JsonPropertyName(nameof(SegundoApellido))] public string? SegundoApellido { get; set; } = null;
    [JsonPropertyName(nameof(Correo))] public string? Correo { get; set; } = null;
    [JsonPropertyName(nameof(Contrasena))] public string? Contrasena { get; set; } = null;
    [JsonPropertyName(nameof(Pais))] public long? Pais { get; set; } = null;
    [JsonPropertyName(nameof(Rol))] public long? Rol { get; set; } = null;
    [JsonPropertyName(nameof(CreadoEn))] public DateTime? CreadoEn { get; set; } = null;
}