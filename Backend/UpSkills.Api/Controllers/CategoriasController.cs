using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using UpSkills.Business.Interfaces;
using UpSkills.Models.DTO;

namespace UpSkills.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriasController(ICategoriasService _CategoriasService, IMemoryCache _MemoryCache) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        string key = nameof(CategoriasController) + "_GetAll";
        if (!_MemoryCache.TryGetValue(key, out ApiResponse? response))
        {
            response = await _CategoriasService.GetAll();
            _MemoryCache.Set(key, response,
                response!.statusCode == System.Net.HttpStatusCode.NoContent
                    ? TimeSpan.FromSeconds(30)
                    : TimeSpan.FromMinutes(30));
        }
        return StatusCode((int)response!.statusCode, response.value);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CategoriaDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        _MemoryCache.Remove(nameof(CategoriasController) + "_GetAll");
        var response = await _CategoriasService.Create(dto);
        return StatusCode((int)response.statusCode, response.statusCode == System.Net.HttpStatusCode.Created ? response.value : response.message);
    }
}
