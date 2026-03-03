using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using UpSkills.Api.Interfaces;
using UpSkills.Models.DTO;

namespace UpSkills.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController(ILoginService _LoginService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Login([Required] [NotNull] AuthDTO auth)
    {
        ApiResponse response = await _LoginService.Authenticate(auth);
        return StatusCode(
            statusCode: (int)response.statusCode,
            value: response.value
        );
    }
}
