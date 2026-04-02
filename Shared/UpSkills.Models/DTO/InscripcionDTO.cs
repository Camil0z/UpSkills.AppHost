using System.ComponentModel.DataAnnotations;

namespace UpSkills.Models.DTO;

public class CreateInscripcionDTO
{
    [Required(ErrorMessage = "El usuario es obligatorio")]
    public long UsuarioId { get; set; }

    [Required(ErrorMessage = "El curso es obligatorio")]
    public long CursoId { get; set; }

    [MaxLength(100)]
    public string? Referencia { get; set; }
}

public class GetInscripcionDTO
{
    public long Id { get; set; }
    public long UsuarioId { get; set; }
    public string UsuarioNombre { get; set; } = null!;
    public long CursoId { get; set; }
    public string CursoTitulo { get; set; } = null!;
    public string? CursoImagen { get; set; }
    public string Estado { get; set; } = null!;
    public int EstadoId { get; set; }
    public string? Referencia { get; set; }
    public decimal Valor { get; set; }
    public DateTime FechaInscripcion { get; set; }
}
