using Microsoft.EntityFrameworkCore;
using UpSkills.Business.Interfaces;
using UpSkills.Infrastructure;
using UpSkills.Models.DTO;
using System.Net;
using UpSkills.Infrastructure.Entidades;
using AutoMapper;

namespace UpSkills.Business.Services;

public class RolesService(AppDbContext _AppDbContext, IMapper _Mapper) : IRolesService
{
    public async Task<ApiResponse> GetAll()
    {
        try
        {
            var roles = await _AppDbContext.Rol.AsNoTracking()
                .Where(x => x.Id != 1)
                .ToListAsync();

            return new(
                statusCode: roles.Count != 0 ? HttpStatusCode.OK : HttpStatusCode.NoContent,
                value: _Mapper.Map<List<RolDTO>>(roles)
            );
        }
        catch (Exception ex)
        {
            string msg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            return new(
                statusCode: HttpStatusCode.InternalServerError,
                message: $"Error de consulta: {msg}."
            );
        }
    }

    public async Task<ApiResponse> Create(RolDTO rolDTO)
    {
        try
        {
            if (rolDTO == null || string.IsNullOrEmpty(rolDTO.Nombre))
                return new(
                    statusCode: HttpStatusCode.BadRequest,
                    message: $"El valor '{nameof(rolDTO.Nombre)}' es requerido."
                );
            
            if (rolDTO.Nombre.Length < 3 || rolDTO.Nombre.Length > 15)
                return new(
                    statusCode: HttpStatusCode.BadRequest,
                    message: $"El valor '{nameof(rolDTO.Nombre)}' debe estar entre 3 y 15 caracteres."
                );
            
            Rol rol = new() { Nombre = rolDTO.Nombre };
            await _AppDbContext.Rol.AddAsync(rol);
            await _AppDbContext.SaveChangesAsync();

            return new(
                statusCode: HttpStatusCode.Created,
                value: _Mapper.Map<RolDTO>(rol)
            );
        }
        catch (Exception ex)
        {
            string msg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            return new(
                statusCode: HttpStatusCode.InternalServerError,
                message: $"Error de creación: {msg}."
            );
        }
    }
}
