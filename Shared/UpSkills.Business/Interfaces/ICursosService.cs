using UpSkills.Models.DTO;

namespace UpSkills.Business.Interfaces;

public interface ICursosService
{
    Task<ApiResponse> GetPaginado(int pagina, int tamano, string? busqueda, long? categoriaId, decimal? precioMin, decimal? precioMax, int? duracionMax);
    Task<ApiResponse> GetById(long id);
    Task<ApiResponse> GetByInstructor(long instructorId);
    Task<ApiResponse> Create(CreateCursoDTO dto);
    Task<ApiResponse> Update(long id, CreateCursoDTO dto);
    Task<ApiResponse> CambiarEstado(long id, int estado);
    Task<ApiResponse> Delete(long id);
}
