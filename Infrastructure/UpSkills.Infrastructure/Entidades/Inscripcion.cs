using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace UpSkills.Infrastructure.Entidades;

public enum EstadoInscripcion
{
    Pendiente = 1,
    Aprobada = 2,
    Rechazada = 3
}

public class Inscripcion
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column(nameof(Id))]
    public long Id { get; set; }

    [NotNull]
    [Required]
    public Usuario Usuario { get; set; } = null!;

    [NotNull]
    [Required]
    [JsonIgnore]
    public Curso Curso { get; set; } = null!;

    [Column(nameof(Estado))]
    [NotNull]
    [Required]
    public EstadoInscripcion Estado { get; set; } = EstadoInscripcion.Pendiente;

    [Column(nameof(Referencia))]
    [MaxLength(100)]
    public string? Referencia { get; set; }

    [Column(nameof(Valor), TypeName = "numeric(10,2)")]
    public decimal Valor { get; set; }

    [Column(nameof(FechaInscripcion))]
    [NotNull]
    [Required]
    public DateTime FechaInscripcion { get; set; } = DateTime.Now;
}
