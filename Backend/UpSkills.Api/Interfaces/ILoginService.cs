using UpSkills.Models.DTO;

namespace UpSkills.Api.Interfaces;

public interface ILoginService
{
    public Task<ApiResponse> Authenticate(AuthDTO auth);
}
