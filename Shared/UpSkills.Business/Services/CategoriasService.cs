using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;
using UpSkills.Business.Interfaces;
using UpSkills.Infrastructure;
using UpSkills.Infrastructure.Entidades;
using UpSkills.Models.DTO;

namespace UpSkills.Business.Services;

public class CategoriasService(AppDbContext _AppDbContext, IMapper _Mapper) : ICategoriasService
{
    public async Task<ApiResponse> GetAll()
    {
        try
        {
            var categorias = await _AppDbContext.Categoria.AsNoTracking()
                .OrderBy(c => c.Nombre)
                .ToListAsync();

            return new(
                statusCode: categorias.Any() ? HttpStatusCode.OK : HttpStatusCode.NoContent,
                value: _Mapper.Map<IEnumerable<CategoriaDTO>>(categorias)
            );
        }
        catch (Exception ex)
        {
            string msg = ex.InnerException?.Message ?? ex.Message;
            return new(HttpStatusCode.InternalServerError, message: $"Error de consulta: {msg}.");
        }
    }

    public async Task<ApiResponse> Create(CategoriaDTO dto)
    {
        try
        {
            bool existe = await _AppDbContext.Categoria.AnyAsync(c => c.Nombre.ToLower() == dto.Nombre.ToLower());
            if (existe)
                return new(HttpStatusCode.Conflict, message: "Ya existe una categoría con ese nombre.");

            var categoria = _Mapper.Map<Categoria>(dto);
            await _AppDbContext.Categoria.AddAsync(categoria);
            await _AppDbContext.SaveChangesAsync();

            return new(HttpStatusCode.Created, value: _Mapper.Map<CategoriaDTO>(categoria));
        }
        catch (Exception ex)
        {
            string msg = ex.InnerException?.Message ?? ex.Message;
            return new(HttpStatusCode.InternalServerError, message: $"Error de creación: {msg}.");
        }
    }
}
