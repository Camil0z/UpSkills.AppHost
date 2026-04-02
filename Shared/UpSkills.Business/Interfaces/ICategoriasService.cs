using UpSkills.Models.DTO;

namespace UpSkills.Business.Interfaces;

public interface ICategoriasService
{
    Task<ApiResponse> GetAll();
    Task<ApiResponse> Create(CategoriaDTO dto);
}
