using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace UpSkills.Infrastructure.Entidades;

public class Usuario
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column(nameof(Id))]
    public long Id { get; set; }

    [Column(nameof(Activo))]
    [NotNull]
    [Required]
    public bool Activo { get; set; } = true;

    [Column(nameof(PrimerNombre))]
    [NotNull]
    [Required]
    public string PrimerNombre { get; set; } = null!;

    [Column(nameof(SegundoNombre))]
    public string? SegundoNombre { get; set; } = null;

    [Column(nameof(PrimerApellido))]
    [NotNull]
    [Required]
    public string PrimerApellido { get; set; } = null!;

    [Column(nameof(SegundoApellido))]
    public string? SegundoApellido { get; set; } = null;

    [Column(nameof(Correo))]
    [NotNull]
    [Required]
    [EmailAddress]
    public string Correo { get; set; } = null!;

    [Column(nameof(Contrasena))]
    [NotNull]
    [Required]
    [MaxLength(255)]
    public string Contrasena { get; set; } = null!;

    public Pais? Pais { get; set; }

    [Required]
    [NotNull]
    public Rol Rol { get; set; } = null!;
    
    [Column(nameof(CreadoEn))]
    [NotNull]
    [Required]
    public DateTime CreadoEn { get; set; } = DateTime.Now;
}