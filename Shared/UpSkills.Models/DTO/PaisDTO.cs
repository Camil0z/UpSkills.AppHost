using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace UpSkills.Models.DTO;

public class PaisDTO
{
    [JsonPropertyName(nameof(Id))]
    public long? Id { get; set; }

    [JsonPropertyName(nameof(Nombre))]
    [Required]
    [NotNull]
    public string Nombre { get; set; } = null!;
}
