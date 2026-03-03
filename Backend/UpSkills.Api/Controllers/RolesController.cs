using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using UpSkills.Business.Interfaces;
using UpSkills.Models.DTO;

namespace UpSkills.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolesController(IRolesService _RolesService, IMemoryCache _MemoryCache) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Roles()
    {
        string key = nameof(RolesController) + "_" + nameof(Roles) + "_" + "Get";
        if (!_MemoryCache.TryGetValue(key, out ApiResponse? response))
        {
            response = await _RolesService.GetAll();
            _MemoryCache.Set(
                key,
                response,
                response!.statusCode == System.Net.HttpStatusCode.OK ? TimeSpan.FromDays(7) : TimeSpan.FromSeconds(10)
            );
        }

        return StatusCode(
            statusCode: (int)response!.statusCode,
            value: response!.value
        );
    }

    [HttpPost]
    public async Task<IActionResult> Roles([Required] [NotNull] [FromBody] RolDTO rolDTO)
    {
        ApiResponse response = await _RolesService.Create(rolDTO);
        return StatusCode(
            statusCode: (int)response.statusCode,
            value: response.value
        );
    }
}