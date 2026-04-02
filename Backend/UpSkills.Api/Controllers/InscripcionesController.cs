using Microsoft.AspNetCore.Mvc;
using UpSkills.Business.Interfaces;
using UpSkills.Models.DTO;

namespace UpSkills.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InscripcionesController(IInscripcionesService _InscripcionesService) : ControllerBase
{
    /// <summary>Inscripciones del usuario (M4 - mis-inscripciones)</summary>
    [HttpGet("usuario/{usuarioId}")]
    public async Task<IActionResult> GetByUsuario(long usuarioId)
    {
        var response = await _InscripcionesService.GetByUsuario(usuarioId);
        return StatusCode((int)response.statusCode, response.statusCode == System.Net.HttpStatusCode.NoContent ? null : response.value);
    }

    /// <summary>Inscripciones por curso (para instructor)</summary>
    [HttpGet("curso/{cursoId}")]
    public async Task<IActionResult> GetByCurso(long cursoId)
    {
        var response = await _InscripcionesService.GetByCurso(cursoId);
        return StatusCode((int)response.statusCode, response.statusCode == System.Net.HttpStatusCode.NoContent ? null : response.value);
    }

    /// <summary>Crear inscripción (M4-RF1, M4-RF3)</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateInscripcionDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _InscripcionesService.Create(dto);
        return StatusCode((int)response.statusCode, response.statusCode == System.Net.HttpStatusCode.Created ? response.value : response.message);
    }

    /// <summary>Cambiar estado: 1=Pendiente, 2=Aprobada, 3=Rechazada (M4-RF2)</summary>
    [HttpPatch("{id}/estado/{estado}")]
    public async Task<IActionResult> CambiarEstado(long id, int estado)
    {
        var response = await _InscripcionesService.CambiarEstado(id, estado);
        return StatusCode((int)response.statusCode, response.message);
    }
}
