using Microsoft.EntityFrameworkCore;
using System.Net;
using UpSkills.Business.Interfaces;
using UpSkills.Infrastructure;
using UpSkills.Infrastructure.Entidades;
using UpSkills.Models.DTO;

namespace UpSkills.Business.Services;

public class InscripcionesService(AppDbContext _AppDbContext) : IInscripcionesService
{
    public async Task<ApiResponse> GetByUsuario(long usuarioId)
    {
        try
        {
            var inscripciones = await _AppDbContext.Inscripcion.AsNoTracking()
                .Include(i => i.Usuario)
                .Include(i => i.Curso)
                    .ThenInclude(c => c.Categoria)
                .Where(i => i.Usuario.Id == usuarioId)
                .OrderByDescending(i => i.FechaInscripcion)
                .ToListAsync();

            return new(
                statusCode: inscripciones.Any() ? HttpStatusCode.OK : HttpStatusCode.NoContent,
                value: inscripciones.Select(MapToDto).ToList()
            );
        }
        catch (Exception ex)
        {
            string msg = ex.InnerException?.Message ?? ex.Message;
            return new(HttpStatusCode.InternalServerError, message: $"Error de consulta: {msg}.");
        }
    }

    public async Task<ApiResponse> GetByCurso(long cursoId)
    {
        try
        {
            var inscripciones = await _AppDbContext.Inscripcion.AsNoTracking()
                .Include(i => i.Usuario)
                .Include(i => i.Curso)
                .Where(i => i.Curso.Id == cursoId)
                .OrderByDescending(i => i.FechaInscripcion)
                .ToListAsync();

            return new(
                statusCode: inscripciones.Any() ? HttpStatusCode.OK : HttpStatusCode.NoContent,
                value: inscripciones.Select(MapToDto).ToList()
            );
        }
        catch (Exception ex)
        {
            string msg = ex.InnerException?.Message ?? ex.Message;
            return new(HttpStatusCode.InternalServerError, message: $"Error de consulta: {msg}.");
        }
    }

    public async Task<ApiResponse> Create(CreateInscripcionDTO dto)
    {
        try
        {
            var usuario = await _AppDbContext.Usuario.FindAsync(dto.UsuarioId);
            if (usuario == null)
                return new(HttpStatusCode.BadRequest, message: "Usuario no válido.");

            var curso = await _AppDbContext.Curso
                .Include(c => c.Inscripciones)
                .FirstOrDefaultAsync(c => c.Id == dto.CursoId && c.Estado == EstadoCurso.Publicado);
            if (curso == null)
                return new(HttpStatusCode.BadRequest, message: "Curso no válido o no publicado.");

            bool yaInscrito = await _AppDbContext.Inscripcion.AnyAsync(i =>
                i.Usuario.Id == dto.UsuarioId &&
                i.Curso.Id == dto.CursoId &&
                i.Estado != EstadoInscripcion.Rechazada);
            if (yaInscrito)
                return new(HttpStatusCode.Conflict, message: "Ya existe una inscripción activa para este curso.");

            var inscripcion = new Inscripcion
            {
                Usuario = usuario,
                Curso = curso,
                Estado = EstadoInscripcion.Pendiente,
                Referencia = dto.Referencia,
                Valor = curso.Precio,
                FechaInscripcion = DateTime.Now
            };

            _AppDbContext.Attach(usuario);
            _AppDbContext.Attach(curso);
            await _AppDbContext.Inscripcion.AddAsync(inscripcion);
            await _AppDbContext.SaveChangesAsync();

            return new(HttpStatusCode.Created, value: MapToDto(inscripcion));
        }
        catch (Exception ex)
        {
            string msg = ex.InnerException?.Message ?? ex.Message;
            return new(HttpStatusCode.InternalServerError, message: $"Error de creación: {msg}.");
        }
    }

    public async Task<ApiResponse> CambiarEstado(long id, int estado)
    {
        try
        {
            var inscripcion = await _AppDbContext.Inscripcion.FindAsync(id);
            if (inscripcion == null)
                return new(HttpStatusCode.NotFound, message: "Inscripción no encontrada.");

            if (!Enum.IsDefined(typeof(EstadoInscripcion), estado))
                return new(HttpStatusCode.BadRequest, message: "Estado no válido.");

            inscripcion.Estado = (EstadoInscripcion)estado;
            await _AppDbContext.SaveChangesAsync();
            return new(HttpStatusCode.NoContent, value: null);
        }
        catch (Exception ex)
        {
            string msg = ex.InnerException?.Message ?? ex.Message;
            return new(HttpStatusCode.InternalServerError, message: $"Error de actualización: {msg}.");
        }
    }

    private static GetInscripcionDTO MapToDto(Inscripcion i) => new()
    {
        Id = i.Id,
        UsuarioId = i.Usuario?.Id ?? 0,
        UsuarioNombre = i.Usuario != null ? $"{i.Usuario.PrimerNombre} {i.Usuario.PrimerApellido}" : "",
        CursoId = i.Curso?.Id ?? 0,
        CursoTitulo = i.Curso?.Titulo ?? "",
        CursoImagen = i.Curso?.Imagen,
        Estado = i.Estado.ToString(),
        EstadoId = (int)i.Estado,
        Referencia = i.Referencia,
        Valor = i.Valor,
        FechaInscripcion = i.FechaInscripcion
    };
}
