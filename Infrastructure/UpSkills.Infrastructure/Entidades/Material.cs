using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace UpSkills.Infrastructure.Entidades;

public enum TipoMaterial
{
    Archivo = 1,
    Enlace = 2
}

public class Material
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column(nameof(Id))]
    public long Id { get; set; }

    [Column(nameof(Titulo))]
    [NotNull]
    [Required]
    [MaxLength(200)]
    public string Titulo { get; set; } = null!;

    [Column(nameof(Tipo))]
    [NotNull]
    [Required]
    public TipoMaterial Tipo { get; set; } = TipoMaterial.Enlace;

    [Column(nameof(Url))]
    [NotNull]
    [Required]
    [MaxLength(500)]
    public string Url { get; set; } = null!;

    [Column(nameof(Orden))]
    [NotNull]
    [Required]
    public int Orden { get; set; }

    [JsonIgnore]
    public Modulo Modulo { get; set; } = null!;
}
