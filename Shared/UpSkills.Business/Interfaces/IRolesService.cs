using UpSkills.Models.DTO;

namespace UpSkills.Business.Interfaces;

public interface IRolesService
{
    public Task<ApiResponse> GetAll();
    public Task<ApiResponse> Create(RolDTO rolDTO);
}
