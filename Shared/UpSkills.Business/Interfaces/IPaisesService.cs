using UpSkills.Models.DTO;

namespace UpSkills.Business.Interfaces;

public interface IPaisesService
{
    public Task<ApiResponse> GetAll();
}
