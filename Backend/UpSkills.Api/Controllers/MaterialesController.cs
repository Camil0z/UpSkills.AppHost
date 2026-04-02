using Microsoft.AspNetCore.Mvc;
using UpSkills.Business.Interfaces;
using UpSkills.Models.DTO;

namespace UpSkills.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MaterialesController(IMaterialesService _MaterialesService) : ControllerBase
{
    [HttpGet("modulo/{moduloId}")]
    public async Task<IActionResult> GetByModulo(long moduloId)
    {
        var response = await _MaterialesService.GetByModulo(moduloId);
        return StatusCode((int)response.statusCode, response.statusCode == System.Net.HttpStatusCode.NoContent ? null : response.value);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMaterialDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _MaterialesService.Create(dto);
        return StatusCode((int)response.statusCode, response.statusCode == System.Net.HttpStatusCode.Created ? response.value : response.message);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(long id, [FromBody] CreateMaterialDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var response = await _MaterialesService.Update(id, dto);
        return StatusCode((int)response.statusCode, response.message);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var response = await _MaterialesService.Delete(id);
        return StatusCode((int)response.statusCode, response.message);
    }
}
