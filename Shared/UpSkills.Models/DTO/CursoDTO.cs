using System.ComponentModel.DataAnnotations;

namespace UpSkills.Models.DTO;

public class CreateCursoDTO
{
    [Required(ErrorMessage = "El título es obligatorio")]
    [MaxLength(200)]
    public string Titulo { get; set; } = null!;

    [MaxLength(500)]
    public string? Imagen { get; set; }

    [Required(ErrorMessage = "La descripción es obligatoria")]
    public string Descripcion { get; set; } = null!;

    [Required(ErrorMessage = "La duración es obligatoria")]
    [Range(1, 10000, ErrorMessage = "La duración debe ser mayor a 0")]
    public int Duracion { get; set; }

    [Required(ErrorMessage = "El precio es obligatorio")]
    [Range(0, 999999.99, ErrorMessage = "Precio inválido")]
    public decimal Precio { get; set; }

    /// <summary>1=Borrador, 2=Publicado, 3=Inactivo</summary>
    public int Estado { get; set; } = 1;

    [Required(ErrorMessage = "La categoría es obligatoria")]
    public long CategoriaId { get; set; }

    [Required(ErrorMessage = "El instructor es obligatorio")]
    public long InstructorId { get; set; }
}

public class GetCursoDTO
{
    public long Id { get; set; }
    public string Titulo { get; set; } = null!;
    public string? Imagen { get; set; }
    public string Descripcion { get; set; } = null!;
    public int Duracion { get; set; }
    public decimal Precio { get; set; }
    public string Estado { get; set; } = null!;
    public int EstadoId { get; set; }
    public long CategoriaId { get; set; }
    public string CategoriaNombre { get; set; } = null!;
    public long InstructorId { get; set; }
    public string InstructorNombre { get; set; } = null!;
    public DateTime CreadoEn { get; set; }
    public int TotalModulos { get; set; }
    public int TotalInscritos { get; set; }
}

public class CursoPaginadoDTO
{
    public List<GetCursoDTO> Items { get; set; } = new();
    public int TotalItems { get; set; }
    public int PaginaActual { get; set; }
    public int TotalPaginas { get; set; }
    public int TamañoPagina { get; set; }
}
