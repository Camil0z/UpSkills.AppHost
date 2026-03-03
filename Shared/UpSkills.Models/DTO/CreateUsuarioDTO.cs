using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace UpSkills.Models.DTO;

public class CreateUsuarioDTO
{
    [NotNull][Required]
    [MinLength(3)][MaxLength(20)]
    [JsonPropertyName(nameof(PrimerNombre))] public string PrimerNombre { get; set; } = null!;
    
    [MaxLength(20)]
    [JsonPropertyName(nameof(SegundoNombre))] public string SegundoNombre { get; set; } = null!;

    [NotNull][Required]
    [MinLength(3)][MaxLength(20)]
    [JsonPropertyName(nameof(PrimerApellido))] public string PrimerApellido { get; set; } = null!;

    [MaxLength(20)]
    [JsonPropertyName(nameof(SegundoApellido))] public string SegundoApellido { get; set; } = null!;

    [NotNull][Required]
    [EmailAddress]
    [MaxLength(100)]
    [JsonPropertyName(nameof(Correo))] public string Correo { get; set; } = null!;

    [NotNull][Required]
    [MinLength(8)][MaxLength(30)]
    [JsonPropertyName(nameof(Contrasena))] public string Contrasena { get; set; } = null!;
    [JsonPropertyName(nameof(Pais))] public long? Pais { get; set; } = null;

    [NotNull][Required]
    [JsonPropertyName(nameof(Rol))] public long Rol { get; set; }
}