using UpSkills.Models.DTO;

namespace UpSkills.Business.Interfaces;

public interface IInscripcionesService
{
    Task<ApiResponse> GetByUsuario(long usuarioId);
    Task<ApiResponse> GetByCurso(long cursoId);
    Task<ApiResponse> Create(CreateInscripcionDTO dto);
    Task<ApiResponse> CambiarEstado(long id, int estado);
}
