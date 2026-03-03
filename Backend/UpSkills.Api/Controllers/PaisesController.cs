using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using UpSkills.Business.Interfaces;
using UpSkills.Models.DTO;

namespace UpSkills.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaisesController (IPaisesService _PaisesService, IMemoryCache _MemoryCache) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Ciudades()
    {
        string key = nameof(PaisesController) + "_" + nameof(Ciudades) + "_" + "Get";
        if (!_MemoryCache.TryGetValue(key, out ApiResponse? response))
        {
            response = await _PaisesService.GetAll();
            _MemoryCache.Set(
                key,
                response,
                response!.statusCode == System.Net.HttpStatusCode.NoContent ? TimeSpan.FromSeconds(10) : TimeSpan.FromDays(7)
            );
        }

        return StatusCode(
            statusCode: (int)response!.statusCode,
            value: response.value
        );
    }
}