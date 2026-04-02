using System.ComponentModel.DataAnnotations;

namespace UpSkills.Models.DTO;

public class CreateMaterialDTO
{
    [Required(ErrorMessage = "El título es obligatorio")]
    [MaxLength(200)]
    public string Titulo { get; set; } = null!;

    /// <summary>1=Archivo, 2=Enlace</summary>
    [Required(ErrorMessage = "El tipo es obligatorio")]
    public int Tipo { get; set; } = 2;

    [Required(ErrorMessage = "La URL es obligatoria")]
    [MaxLength(500)]
    public string Url { get; set; } = null!;

    [Required(ErrorMessage = "El orden es obligatorio")]
    [Range(1, 9999)]
    public int Orden { get; set; }

    [Required(ErrorMessage = "El módulo es obligatorio")]
    public long ModuloId { get; set; }
}

public class GetMaterialDTO
{
    public long Id { get; set; }
    public string Titulo { get; set; } = null!;
    public string Tipo { get; set; } = null!;
    public int TipoId { get; set; }
    public string Url { get; set; } = null!;
    public int Orden { get; set; }
    public long ModuloId { get; set; }
}
