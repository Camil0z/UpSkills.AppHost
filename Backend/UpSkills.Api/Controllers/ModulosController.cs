using Microsoft.AspNetCore.Mvc;
using UpSkills.Business.Interfaces;
using UpSkills.Models.DTO;

namespace UpSkills.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ModulosController(IModulosService _ModulosService) : ControllerBase
{
    [HttpGet("curso/{cursoId}")]
    public async Task<IActionResult> GetByCurso(long cursoId)
    {
        var response = await _ModulosService.GetByCurso(cursoId);
        return StatusCode((int)response.statusCode, response.statusCode == System.Net.HttpStatusCode.NoContent ? null : response.value);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateModuloDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _ModulosService.Create(dto);
        return StatusCode((int)response.statusCode, response.statusCode == System.Net.HttpStatusCode.Created ? response.value : response.message);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] CreateModuloDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _ModulosService.Update(id, dto);
        return StatusCode((int)response.statusCode, response.message);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var response = await _ModulosService.Delete(id);
        return StatusCode((int)response.statusCode, response.message);
    }
}
