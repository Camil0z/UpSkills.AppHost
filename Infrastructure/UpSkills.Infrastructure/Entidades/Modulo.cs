using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace UpSkills.Infrastructure.Entidades;

public class Modulo
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

    [Column(nameof(Orden))]
    [NotNull]
    [Required]
    public int Orden { get; set; }

    [JsonIgnore]
    public Curso Curso { get; set; } = null!;

    public ICollection<Material> Materiales { get; set; } = new List<Material>();
}
