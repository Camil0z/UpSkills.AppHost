namespace UpSkills.Models.DTO;

public class LoginResponseDTO
{
    public string Token { get; set; } = null!;
    public long UserId { get; set; }
    public string Rol { get; set; } = null!;
    public string NombreCompleto { get; set; } = null!;
    public TimeSpan SesionTimeout { get; set; }
}
