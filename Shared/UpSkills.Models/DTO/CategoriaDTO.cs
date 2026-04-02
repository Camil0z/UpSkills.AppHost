using System.ComponentModel.DataAnnotations;

namespace UpSkills.Models.DTO;

public class CategoriaDTO
{
    public long? Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [MaxLength(100)]
    public string Nombre { get; set; } = null!;
}
