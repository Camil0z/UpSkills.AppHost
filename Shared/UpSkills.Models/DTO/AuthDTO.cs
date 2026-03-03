using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace UpSkills.Models.DTO;

public class AuthDTO
{
    [NotNull][Required]
    [EmailAddress]
    [MaxLength(100)]
    [JsonPropertyName(nameof(Correo))] public string Correo { get; set; } = string.Empty;

    [NotNull][Required]
    [MinLength(8)][MaxLength(30)]
    [JsonPropertyName(nameof(Contrasena))] public string Contrasena { get; set; } = string.Empty;
}
