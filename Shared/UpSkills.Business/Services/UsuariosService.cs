using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;
using UpSkills.Business.Interfaces;
using UpSkills.Infrastructure;
using UpSkills.Infrastructure.Entidades;
using UpSkills.Models.DTO;

namespace UpSkills.Business.Services;

public class UsuariosService (AppDbContext _AppDbContext, IMapper _Mapper) : IUsuariosService
{
    public async Task<ApiResponse> GetAll()
    {
        try
        {
            var usuarios = await _AppDbContext.Usuario.AsNoTracking()
                .Include(x => x.Rol)
                .Include(x => x.Pais)
                .ToListAsync();

            return new(
                statusCode: usuarios.Any() ? HttpStatusCode.OK : HttpStatusCode.NoContent,
                value: _Mapper.Map<IEnumerable<GetUsuarioDTO>>(usuarios)
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

    public async Task<ApiResponse> GetById(long id)
    {
        try
        {
            Usuario? usuario = await _AppDbContext.Usuario.AsNoTracking()
                .Include(x => x.Rol)
                .Include(x => x.Pais)
                .FirstOrDefaultAsync(usuario => usuario.Id == id);

            return new(
                statusCode: usuario != null ? HttpStatusCode.OK : HttpStatusCode.NotFound,
                value: usuario != null ? _Mapper.Map<GetUsuarioDTO>(usuario) : null
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
    
    public async Task<ApiResponse> GetByRol(long rolId)
    {
        try
        {
            var usuarios = await _AppDbContext.Usuario.AsNoTracking()
                .Include(x => x.Rol)
                .Include(x => x.Pais)
                .Where(usuario => usuario.Rol.Id == rolId).ToListAsync();

            return new(
                statusCode: usuarios.Any() ? HttpStatusCode.OK : HttpStatusCode.NoContent,
                value: _Mapper.Map<IEnumerable<UsuarioDTO>>(usuarios)
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

    public async Task<ApiResponse> GetByState(bool active)
    {
        try
        {
            var usuarios = await _AppDbContext.Usuario.AsNoTracking()
                .Include(x => x.Rol)
                .Include(x => x.Pais)
                .Where(usuario => usuario.Activo == active).ToListAsync();

            return new(
                statusCode: usuarios.Any() ? HttpStatusCode.OK : HttpStatusCode.NoContent,
                value: _Mapper.Map<IEnumerable<UsuarioDTO>>(usuarios)
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

    public async Task<ApiResponse> Create(CreateUsuarioDTO usuarioDTO)
    {
        try
        {
            Pais? pais = null;
            if (usuarioDTO.Pais != null)
            {
                pais = await _AppDbContext.Pais.AsNoTracking()
                    .FirstOrDefaultAsync(ciudad => ciudad.Id == usuarioDTO.Pais);

                if (pais == null)
                {
                    return new(
                        HttpStatusCode.BadRequest,
                        message: $"'{nameof(usuarioDTO.Pais)}' no valido."
                    );
                }
            }

            Rol? rol = await _AppDbContext.Rol.AsNoTracking()
                    .FirstOrDefaultAsync(rol => rol.Id == usuarioDTO.Rol && rol.Id != 1 ); //1 es Administrador

            if (rol == null)
            {
                return new(
                    HttpStatusCode.BadRequest,
                    message: $"'{nameof(usuarioDTO.Rol)}' no valido."
                );
            }

            if ((!string.IsNullOrEmpty(usuarioDTO.SegundoNombre)) & usuarioDTO.SegundoNombre.Length < 3)
            {
                return new(
                    HttpStatusCode.BadRequest,
                    message: $"'{nameof(usuarioDTO.SegundoNombre)}' debe estar entre 3 y 20 caracteres."
                );
            }

            if ((!string.IsNullOrEmpty(usuarioDTO.SegundoApellido)) & usuarioDTO.SegundoApellido.Length < 3)
            {
                return new(
                    HttpStatusCode.BadRequest,
                    message: $"'{nameof(usuarioDTO.SegundoApellido)}' debe estar entre 3 y 20 caracteres."
                );
            }

            usuarioDTO.Correo = usuarioDTO.Correo.ToLower();
            Usuario usuario = _Mapper.Map<Usuario>(usuarioDTO);

            _AppDbContext.Attach<Rol>(usuario.Rol);

            if (usuario.Pais != null && usuario.Pais?.Id != null)
                _AppDbContext.Attach<Pais>(usuario.Pais);

            var createdUser = await _AppDbContext.Usuario.AddAsync(usuario);
            await _AppDbContext.SaveChangesAsync();

            return new(
                statusCode: HttpStatusCode.Created,
                value: _Mapper.Map<GetUsuarioDTO>(createdUser.Entity)
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

    public async Task<ApiResponse> Update(long id, CreateUsuarioDTO usuarioDTO)
    {
        try
        {
            throw new NotImplementedException("Requiere validaciones adicionales.");

            Usuario? usuario = await _AppDbContext.Usuario.AsNoTracking()
                .FirstOrDefaultAsync(usuario => usuario.Id == id);

            if (usuario == null)
            {
                return new(
                    statusCode: HttpStatusCode.NotFound,
                    value: null //$"Error de actualización: Usuario no encontrado."
                );
            }

            Pais? ciudad = await _AppDbContext.Pais.AsNoTracking()
                .FirstOrDefaultAsync(ciudad => ciudad.Id == usuarioDTO.Pais);

            Rol? rol = await _AppDbContext.Rol.AsNoTracking()
                .FirstOrDefaultAsync(rol => rol.Id == usuarioDTO.Rol);

            usuario = _Mapper.Map<Usuario>(usuarioDTO);

            _AppDbContext.Update(usuario);
            await _AppDbContext.SaveChangesAsync();

            return new(
                statusCode: HttpStatusCode.NoContent,
                value: null
            );
        }
        catch (Exception ex)
        {
            string msg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            return new(
                statusCode: HttpStatusCode.InternalServerError,
                message: $"Error de actualización: {msg}."
            );
        }
    }

    public async Task<ApiResponse> Patch(long id, object usuarioDTO)
    {
        try
        {
            throw new NotImplementedException("Metodo no implementado.");
        }
        catch (Exception ex)
        {
            string msg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            return new(
                statusCode: HttpStatusCode.InternalServerError,
                message: $"Error de actualización: {msg}."
            );
        }
    }

    public async Task<ApiResponse> Delete(long id)
    {
        try
        {
            Usuario? usuario = await _AppDbContext.Usuario //.AsNoTracking()
                .FirstOrDefaultAsync(usuario => usuario.Id == id);

            if (usuario == null)
                return new(
                   statusCode: HttpStatusCode.NotFound,
                   value: null //$"Error de eliminación: Usuario no encontrado."
               );

            usuario.Activo = false;
            //_AppDbContext.Usuario.Update(usuario);
            await _AppDbContext.SaveChangesAsync();

            return new(
                statusCode: HttpStatusCode.NoContent,
                value: null
            );
        }
        catch (Exception ex)
        {
            string msg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            return new(
                statusCode: HttpStatusCode.InternalServerError,
                message: $"Error de eliminación: {msg}."
            );
        }
    }
}
