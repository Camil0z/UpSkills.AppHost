using Microsoft.EntityFrameworkCore;
using System.Net;
using UpSkills.Business.Interfaces;
using UpSkills.Infrastructure;
using UpSkills.Infrastructure.Entidades;
using UpSkills.Models.DTO;

namespace UpSkills.Business.Services;

public class MaterialesService(AppDbContext _AppDbContext) : IMaterialesService
{
    public async Task<ApiResponse> GetByModulo(long moduloId)
    {
        try
        {
            var materiales = await _AppDbContext.Material.AsNoTracking()
                .Where(m => EF.Property<long>(m, "ModuloId") == moduloId)
                .OrderBy(m => m.Orden)
                .ToListAsync();

            return new(
                statusCode: materiales.Any() ? HttpStatusCode.OK : HttpStatusCode.NoContent,
                value: materiales.Select(m => new GetMaterialDTO
                {
                    Id = m.Id,
                    Titulo = m.Titulo,
                    Tipo = m.Tipo.ToString(),
                    TipoId = (int)m.Tipo,
                    Url = m.Url,
                    Orden = m.Orden,
                    ModuloId = moduloId
                }).ToList()
            );
        }
        catch (Exception ex)
        {
            string msg = ex.InnerException?.Message ?? ex.Message;
            return new(HttpStatusCode.InternalServerError, message: $"Error de consulta: {msg}.");
        }
    }

    public async Task<ApiResponse> Create(CreateMaterialDTO dto)
    {
        try
        {
            var modulo = await _AppDbContext.Modulo.FindAsync(dto.ModuloId);
            if (modulo == null)
                return new(HttpStatusCode.BadRequest, message: "Módulo no válido.");

            if (!Enum.IsDefined(typeof(TipoMaterial), dto.Tipo))
                return new(HttpStatusCode.BadRequest, message: "Tipo de material no válido.");

            var material = new Material
            {
                Titulo = dto.Titulo,
                Tipo = (TipoMaterial)dto.Tipo,
                Url = dto.Url,
                Orden = dto.Orden,
                Modulo = modulo
            };

            _AppDbContext.Attach(modulo);
            await _AppDbContext.Material.AddAsync(material);
            await _AppDbContext.SaveChangesAsync();

            return new(HttpStatusCode.Created, value: new GetMaterialDTO
            {
                Id = material.Id,
                Titulo = material.Titulo,
                Tipo = material.Tipo.ToString(),
                TipoId = (int)material.Tipo,
                Url = material.Url,
                Orden = material.Orden,
                ModuloId = dto.ModuloId
            });
        }
        catch (Exception ex)
        {
            string msg = ex.InnerException?.Message ?? ex.Message;
            return new(HttpStatusCode.InternalServerError, message: $"Error de creación: {msg}.");
        }
    }

    public async Task<ApiResponse> Update(long id, CreateMaterialDTO dto)
    {
        try
        {
            var material = await _AppDbContext.Material.FindAsync(id);
            if (material == null)
                return new(HttpStatusCode.NotFound, message: "Material no encontrado.");

            if (!Enum.IsDefined(typeof(TipoMaterial), dto.Tipo))
                return new(HttpStatusCode.BadRequest, message: "Tipo de material no válido.");

            material.Titulo = dto.Titulo;
            material.Tipo = (TipoMaterial)dto.Tipo;
            material.Url = dto.Url;
            material.Orden = dto.Orden;
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
            var material = await _AppDbContext.Material.FindAsync(id);
            if (material == null)
                return new(HttpStatusCode.NotFound, message: "Material no encontrado.");

            _AppDbContext.Material.Remove(material);
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
