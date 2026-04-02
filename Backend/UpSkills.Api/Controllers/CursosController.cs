using Microsoft.AspNetCore.Mvc;
using UpSkills.Business.Interfaces;
using UpSkills.Models.DTO;

namespace UpSkills.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CursosController(ICursosService _CursosService) : ControllerBase
{
    /// <summary>
    /// Catálogo paginado con filtros (M3-RF1, M3-RF2)
    /// GET /api/cursos?pagina=1&tamano=9&busqueda=&categoriaId=&precioMin=&precioMax=&duracionMax=
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetPaginado(
        [FromQuery] int pagina = 1,
        [FromQuery] int tamano = 9,
        [FromQuery] string? busqueda = null,
        [FromQuery] long? categoriaId = null,
        [FromQuery] decimal? precioMin = null,
        [FromQuery] decimal? precioMax = null,
        [FromQuery] int? duracionMax = null)
    {
        var response = await _CursosService.GetPaginado(pagina, tamano, busqueda, categoriaId, precioMin, precioMax, duracionMax);
        return StatusCode((int)response.statusCode, response.value);
    }

    /// <summary>Ficha de curso (M3-RF3)</summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var response = await _CursosService.GetById(id);
        return StatusCode((int)response.statusCode, response.statusCode == System.Net.HttpStatusCode.OK ? response.value : response.message);
    }

    /// <summary>Cursos por instructor (M2)</summary>
    [HttpGet("instructor/{instructorId}")]
    public async Task<IActionResult> GetByInstructor(long instructorId)
    {
        var response = await _CursosService.GetByInstructor(instructorId);
        return StatusCode((int)response.statusCode, response.statusCode == System.Net.HttpStatusCode.NoContent ? null : response.value);
    }

    /// <summary>Crear curso (M2-RF1)</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCursoDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _CursosService.Create(dto);
        return StatusCode((int)response.statusCode, response.statusCode == System.Net.HttpStatusCode.Created ? response.value : response.message);
    }

    /// <summary>Actualizar curso (M2-RF1)</summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] CreateCursoDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _CursosService.Update(id, dto);
        return StatusCode((int)response.statusCode, response.statusCode == System.Net.HttpStatusCode.OK ? response.value : response.message);
    }

    /// <summary>Cambiar estado: 1=Borrador, 2=Publicado, 3=Inactivo (M2-RF2)</summary>
    [HttpPatch("{id}/estado/{estado}")]
    public async Task<IActionResult> CambiarEstado(long id, int estado)
    {
        var response = await _CursosService.CambiarEstado(id, estado);
        return StatusCode((int)response.statusCode, response.message);
    }

    /// <summary>Eliminar (pasa a Inactivo) un curso</summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var response = await _CursosService.Delete(id);
        return StatusCode((int)response.statusCode, response.message);
    }
}
