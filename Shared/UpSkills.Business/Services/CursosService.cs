using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Net;
using UpSkills.Business.Interfaces;
using UpSkills.Infrastructure;
using UpSkills.Infrastructure.Entidades;
using UpSkills.Models.DTO;

namespace UpSkills.Business.Services;

public class CursosService(AppDbContext _AppDbContext, IMapper _Mapper) : ICursosService
{
    public async Task<ApiResponse> GetPaginado(int pagina, int tamano, string? busqueda, long? categoriaId,
        decimal? precioMin, decimal? precioMax, int? duracionMax)
    {
        try
        {
            if (pagina < 1) pagina = 1;
            if (tamano < 1 || tamano > 50) tamano = 9;

            var query = _AppDbContext.Curso.AsNoTracking()
                .Include(c => c.Categoria)
                .Include(c => c.Instructor)
                .Include(c => c.Modulos)
                .Include(c => c.Inscripciones)
                .Where(c => c.Estado == EstadoCurso.Publicado)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(busqueda))
                query = query.Where(c => c.Titulo.ToLower().Contains(busqueda.ToLower())
                    || c.Descripcion.ToLower().Contains(busqueda.ToLower()));

            if (categoriaId.HasValue)
                query = query.Where(c => c.Categoria.Id == categoriaId.Value);

            if (precioMin.HasValue)
                query = query.Where(c => c.Precio >= precioMin.Value);

            if (precioMax.HasValue)
                query = query.Where(c => c.Precio <= precioMax.Value);

            if (duracionMax.HasValue)
                query = query.Where(c => c.Duracion <= duracionMax.Value);

            int totalItems = await query.CountAsync();
            int totalPaginas = (int)Math.Ceiling(totalItems / (double)tamano);

            var cursos = await query
                .OrderByDescending(c => c.CreadoEn)
                .Skip((pagina - 1) * tamano)
                .Take(tamano)
                .ToListAsync();

            var resultado = new CursoPaginadoDTO
            {
                Items = cursos.Select(MapCursoToDto).ToList(),
                TotalItems = totalItems,
                PaginaActual = pagina,
                TotalPaginas = totalPaginas == 0 ? 1 : totalPaginas,
                TamañoPagina = tamano
            };

            return new(HttpStatusCode.OK, value: resultado);
        }
        catch (Exception ex)
        {
            string msg = ex.InnerException?.Message ?? ex.Message;
            return new(HttpStatusCode.InternalServerError, message: $"Error de consulta: {msg}.");
        }
    }

    public async Task<ApiResponse> GetById(long id)
    {
        try
        {
            var curso = await _AppDbContext.Curso.AsNoTracking()
                .Include(c => c.Categoria)
                .Include(c => c.Instructor)
                .Include(c => c.Modulos.OrderBy(m => m.Orden))
                    .ThenInclude(m => m.Materiales.OrderBy(mat => mat.Orden))
                .Include(c => c.Inscripciones)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (curso == null)
                return new(HttpStatusCode.NotFound, message: "Curso no encontrado.");

            return new(HttpStatusCode.OK, value: MapCursoToDto(curso));
        }
        catch (Exception ex)
        {
            string msg = ex.InnerException?.Message ?? ex.Message;
            return new(HttpStatusCode.InternalServerError, message: $"Error de consulta: {msg}.");
        }
    }

    public async Task<ApiResponse> GetByInstructor(long instructorId)
    {
        try
        {
            var cursos = await _AppDbContext.Curso.AsNoTracking()
                .Include(c => c.Categoria)
                .Include(c => c.Instructor)
                .Include(c => c.Modulos)
                .Include(c => c.Inscripciones)
                .Where(c => c.Instructor.Id == instructorId)
                .OrderByDescending(c => c.CreadoEn)
                .ToListAsync();

            return new(
                statusCode: cursos.Any() ? HttpStatusCode.OK : HttpStatusCode.NoContent,
                value: cursos.Select(MapCursoToDto).ToList()
            );
        }
        catch (Exception ex)
        {
            string msg = ex.InnerException?.Message ?? ex.Message;
            return new(HttpStatusCode.InternalServerError, message: $"Error de consulta: {msg}.");
        }
    }

    public async Task<ApiResponse> Create(CreateCursoDTO dto)
    {
        try
        {
            var categoria = await _AppDbContext.Categoria.FindAsync(dto.CategoriaId);
            if (categoria == null)
                return new(HttpStatusCode.BadRequest, message: "Categoría no válida.");

            var instructor = await _AppDbContext.Usuario.FindAsync(dto.InstructorId);
            if (instructor == null)
                return new(HttpStatusCode.BadRequest, message: "Instructor no válido.");

            var curso = new Curso
            {
                Titulo = dto.Titulo,
                Imagen = dto.Imagen,
                Descripcion = dto.Descripcion,
                Duracion = dto.Duracion,
                Precio = dto.Precio,
                Estado = (EstadoCurso)dto.Estado,
                Categoria = categoria,
                Instructor = instructor,
                CreadoEn = DateTime.Now
            };

            _AppDbContext.Attach(categoria);
            _AppDbContext.Attach(instructor);
            await _AppDbContext.Curso.AddAsync(curso);
            await _AppDbContext.SaveChangesAsync();

            return new(HttpStatusCode.Created, value: MapCursoToDto(curso));
        }
        catch (Exception ex)
        {
            string msg = ex.InnerException?.Message ?? ex.Message;
            return new(HttpStatusCode.InternalServerError, message: $"Error de creación: {msg}.");
        }
    }

    public async Task<ApiResponse> Update(long id, CreateCursoDTO dto)
    {
        try
        {
            var curso = await _AppDbContext.Curso
                .Include(c => c.Categoria)
                .Include(c => c.Instructor)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (curso == null)
                return new(HttpStatusCode.NotFound, message: "Curso no encontrado.");

            var categoria = await _AppDbContext.Categoria.FindAsync(dto.CategoriaId);
            if (categoria == null)
                return new(HttpStatusCode.BadRequest, message: "Categoría no válida.");

            curso.Titulo = dto.Titulo;
            curso.Imagen = dto.Imagen;
            curso.Descripcion = dto.Descripcion;
            curso.Duracion = dto.Duracion;
            curso.Precio = dto.Precio;
            curso.Estado = (EstadoCurso)dto.Estado;
            curso.Categoria = categoria;

            await _AppDbContext.SaveChangesAsync();
            return new(HttpStatusCode.OK, value: MapCursoToDto(curso));
        }
        catch (Exception ex)
        {
            string msg = ex.InnerException?.Message ?? ex.Message;
            return new(HttpStatusCode.InternalServerError, message: $"Error de actualización: {msg}.");
        }
    }

    public async Task<ApiResponse> CambiarEstado(long id, int estado)
    {
        try
        {
            var curso = await _AppDbContext.Curso.FindAsync(id);
            if (curso == null)
                return new(HttpStatusCode.NotFound, message: "Curso no encontrado.");

            if (!Enum.IsDefined(typeof(EstadoCurso), estado))
                return new(HttpStatusCode.BadRequest, message: "Estado no válido.");

            curso.Estado = (EstadoCurso)estado;
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
            var curso = await _AppDbContext.Curso.FindAsync(id);
            if (curso == null)
                return new(HttpStatusCode.NotFound, message: "Curso no encontrado.");

            curso.Estado = EstadoCurso.Inactivo;
            await _AppDbContext.SaveChangesAsync();
            return new(HttpStatusCode.NoContent, value: null);
        }
        catch (Exception ex)
        {
            string msg = ex.InnerException?.Message ?? ex.Message;
            return new(HttpStatusCode.InternalServerError, message: $"Error de eliminación: {msg}.");
        }
    }

    private static GetCursoDTO MapCursoToDto(Curso c) => new()
    {
        Id = c.Id,
        Titulo = c.Titulo,
        Imagen = c.Imagen,
        Descripcion = c.Descripcion,
        Duracion = c.Duracion,
        Precio = c.Precio,
        Estado = c.Estado.ToString(),
        EstadoId = (int)c.Estado,
        CategoriaId = c.Categoria?.Id ?? 0,
        CategoriaNombre = c.Categoria?.Nombre ?? "",
        InstructorId = c.Instructor?.Id ?? 0,
        InstructorNombre = c.Instructor != null
            ? $"{c.Instructor.PrimerNombre} {c.Instructor.PrimerApellido}"
            : "",
        CreadoEn = c.CreadoEn,
        TotalModulos = c.Modulos?.Count ?? 0,
        TotalInscritos = c.Inscripciones?.Count ?? 0
    };
}
