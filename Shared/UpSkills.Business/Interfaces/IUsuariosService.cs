using UpSkills.Models.DTO;

namespace UpSkills.Business.Interfaces;

public interface IUsuariosService
{
    public Task<ApiResponse> GetAll();
    public Task<ApiResponse> GetById(long id);
    public Task<ApiResponse> GetByRol(long rolId);
    public Task<ApiResponse> GetByState(bool active);
    public Task<ApiResponse> Create(CreateUsuarioDTO usuarioDTO);
    public Task<ApiResponse> Update(long id, CreateUsuarioDTO usuarioDTO);
    public Task<ApiResponse> Patch(long id, object usuarioDTO);
    public Task<ApiResponse> Delete(long id);
}
