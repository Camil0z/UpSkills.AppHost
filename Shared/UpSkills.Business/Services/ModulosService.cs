using Microsoft.EntityFrameworkCore;
using System.Net;
using UpSkills.Business.Interfaces;
using UpSkills.Infrastructure;
using UpSkills.Infrastructure.Entidades;
using UpSkills.Models.DTO;

namespace UpSkills.Business.Services;

public class ModulosService(AppDbContext _AppDbContext) : IModulosService
{
    public async Task<ApiResponse> GetByCurso(long cursoId)
    {
        try
        {
            var modulos = await _AppDbContext.Modulo.AsNoTracking()
                .Include(m => m.Materiales.OrderBy(mat => mat.Orden))
                .Where(m => EF.Property<long>(m, "CursoId") == cursoId)
                .OrderBy(m => m.Orden)
                .ToListAsync();

            return new(
                statusCode: modulos.Any() ? HttpStatusCode.OK : HttpStatusCode.NoContent,
                value: modulos.Select(m => new GetModuloDTO
                {
                    Id = m.Id,
                    Titulo = m.Titulo,
                    Orden = m.Orden,
                    CursoId = cursoId,
                    Materiales = m.Materiales.Select(mat => new GetMaterialDTO
                    {
                        Id = mat.Id,
                        Titulo = mat.Titulo,
                        Tipo = mat.Tipo.ToString(),
                        TipoId = (int)mat.Tipo,
                        Url = mat.Url,
                        Orden = mat.Orden,
                        ModuloId = m.Id
                    }).ToList()
                }).ToList()
            );
        }
        catch (Exception ex)
        {
            string msg = ex.InnerException?.Message ?? ex.Message;
            return new(HttpStatusCode.InternalServerError, message: $"Error de consulta: {msg}.");
        }
    }

    public async Task<ApiResponse> Create(CreateModuloDTO dto)
    {
        try
        {
            var curso = await _AppDbContext.Curso.FindAsync(dto.CursoId);
            if (curso == null)
                return new(HttpStatusCode.BadRequest, message: "Curso no válido.");

            var modulo = new Modulo
            {
                Titulo = dto.Titulo,
                Orden = dto.Orden,
                Curso = curso
            };

            _AppDbContext.Attach(curso);
            await _AppDbContext.Modulo.AddAsync(modulo);
            await _AppDbContext.SaveChangesAsync();

            return new(HttpStatusCode.Created, value: new GetModuloDTO
            {
                Id = modulo.Id,
                Titulo = modulo.Titulo,
                Orden = modulo.Orden,
                CursoId = dto.CursoId
            });
        }
        catch (Exception ex)
        {
            string msg = ex.InnerException?.Message ?? ex.Message;
            return new(HttpStatusCode.InternalServerError, message: $"Error de creación: {msg}.");
        }
    }

    public async Task<ApiResponse> Update(long id, CreateModuloDTO dto)
    {
        try
        {
            var modulo = await _AppDbContext.Modulo.FindAsync(id);
            if (modulo == null)
                return new(HttpStatusCode.NotFound, message: "Módulo no encontrado.");

            modulo.Titulo = dto.Titulo;
            modulo.Orden = dto.Orden;
            await _AppDbContext.SaveChangesAsync();
            return new(HttpStatusCode.NoContent, value: null);
        }
        catch (Exception ex)
        {
            string msg = ex.InnerException?.Message ?? ex.Message;
            return new(HttpStatusCode.InternalServerError, message: $"Error de actualización: {msg}.");
        }
    }

    public async Task<ApiResponse> Delete(long id)
    {
        try
        {
            var modulo = await _AppDbContext.Modulo.FindAsync(id);
            if (modulo == null)
                return new(HttpStatusCode.NotFound, message: "Módulo no encontrado.");

            _AppDbContext.Modulo.Remove(modulo);
            await _AppDbContext.SaveChangesAsync();
            return new(HttpStatusCode.NoContent, value: null);
        }
        catch (Exception ex)
        {
            string msg = ex.InnerException?.Message ?? ex.Message;
            return new(HttpStatusCode.InternalServerError, message: $"Error de eliminación: {msg}.");
        }
    }
}
