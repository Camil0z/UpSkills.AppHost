using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace UpSkills.Infrastructure.Entidades;

public enum EstadoCurso
{
    Borrador = 1,
    Publicado = 2,
    Inactivo = 3
}

public class Curso
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

    [Column(nameof(Imagen))]
    [MaxLength(500)]
    public string? Imagen { get; set; }

    [Column(nameof(Descripcion))]
    [NotNull]
    [Required]
    public string Descripcion { get; set; } = null!;

    [Column(nameof(Duracion))]
    [NotNull]
    [Required]
    public int Duracion { get; set; } // Horas

    [Column(nameof(Precio), TypeName = "numeric(10,2)")]
    [NotNull]
    [Required]
    public decimal Precio { get; set; }

    [Column(nameof(Estado))]
    [NotNull]
    [Required]
    public EstadoCurso Estado { get; set; } = EstadoCurso.Borrador;

    [NotNull]
    [Required]
    public Categoria Categoria { get; set; } = null!;

    [NotNull]
    [Required]
    public Usuario Instructor { get; set; } = null!;

    [Column(nameof(CreadoEn))]
    [NotNull]
    [Required]
    public DateTime CreadoEn { get; set; } = DateTime.Now;

    public ICollection<Modulo> Modulos { get; set; } = new List<Modulo>();
    public ICollection<Inscripcion> Inscripciones { get; set; } = new List<Inscripcion>();
}
