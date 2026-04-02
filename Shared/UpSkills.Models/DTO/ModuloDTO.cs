using System.ComponentModel.DataAnnotations;

namespace UpSkills.Models.DTO;

public class CreateModuloDTO
{
    [Required(ErrorMessage = "El título es obligatorio")]
    [MaxLength(200)]
    public string Titulo { get; set; } = null!;

    [Required(ErrorMessage = "El orden es obligatorio")]
    [Range(1, 9999)]
    public int Orden { get; set; }

    [Required(ErrorMessage = "El curso es obligatorio")]
    public long CursoId { get; set; }
}

public class GetModuloDTO
{
    public long Id { get; set; }
    public string Titulo { get; set; } = null!;
    public int Orden { get; set; }
    public long CursoId { get; set; }
    public List<GetMaterialDTO> Materiales { get; set; } = new();
}
