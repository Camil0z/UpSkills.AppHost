using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace UpSkills.Infrastructure.Entidades;

public class Rol
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column(nameof(Id))]
    public long Id { get; set; }
    
    [Column(nameof(Nombre))]
    [System.Diagnostics.CodeAnalysis.NotNull]
    [Required]
    public string Nombre { get; set; } = null!;
    
    [JsonIgnore]
    public ICollection<Usuario> Usuario { get; set; } = [];
}