using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace UpSkills.Infrastructure.Entidades;

public class Categoria
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column(nameof(Id))]
    public long Id { get; set; }

    [Column(nameof(Nombre))]
    [NotNull]
    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = null!;

    public ICollection<Curso> Cursos { get; set; } = new List<Curso>();
}
