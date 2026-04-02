using UpSkills.Models.DTO;

namespace UpSkills.Business.Interfaces;

public interface IModulosService
{
    Task<ApiResponse> GetByCurso(long cursoId);
    Task<ApiResponse> Create(CreateModuloDTO dto);
    Task<ApiResponse> Update(long id, CreateModuloDTO dto);
    Task<ApiResponse> Delete(long id);
}
