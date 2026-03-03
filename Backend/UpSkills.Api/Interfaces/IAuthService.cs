namespace UpSkills.Api.Interfaces;

public interface IAuthService
{
    public string GenerateToken(string userId);
    public bool ValidateToken(string token);
}
