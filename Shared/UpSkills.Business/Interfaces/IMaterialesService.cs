using UpSkills.Models.DTO;

namespace UpSkills.Business.Interfaces;

public interface IMaterialesService
{
    Task<ApiResponse> GetByModulo(long moduloId);
    Task<ApiResponse> Create(CreateMaterialDTO dto);
    Task<ApiResponse> Update(long id, CreateMaterialDTO dto);
    Task<ApiResponse> Delete(long id);
}
