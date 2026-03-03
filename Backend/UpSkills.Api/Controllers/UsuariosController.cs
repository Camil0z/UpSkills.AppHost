using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using UpSkills.Api.Interfaces;
using UpSkills.Business.Interfaces;
using UpSkills.Models.DTO;

namespace UpSkills.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsuariosController(IPasswordHasherService _PasswordHasherService, IUsuariosService _UsuariosService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Usuarios()
    {
        ApiResponse response = await _UsuariosService.GetAll();

        return StatusCode(
            statusCode: (int)response!.statusCode,
            value: response.value
        );
    }

    [HttpGet("({id})")]
    public async Task<IActionResult> Usuarios([NotNull][Required] long id)
    {
        ApiResponse response = await _UsuariosService.GetById(id);

        return StatusCode(
            statusCode: (int)response!.statusCode,
            value: response.value
        );
    }

    [HttpGet("rolId={rolId}")]
    public async Task<IActionResult> UsuariosPorRol([NotNull][Required] long rolId)
    {
        ApiResponse response = await _UsuariosService.GetByRol(rolId);

        return StatusCode(
            statusCode: (int)response!.statusCode,
            value: response.value
        );
    }

    [HttpGet("active={active}")]
    public async Task<IActionResult> UsuariosPorEstado([NotNull][Required] bool active)
    {
        ApiResponse response = await _UsuariosService.GetByState(active);

        return StatusCode(
            statusCode: (int)response!.statusCode,
            value: response.value
        );
    }

    // POST: Usuarios/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    public async Task<IActionResult> Usuarios([NotNull][Required][FromBody] CreateUsuarioDTO usuarioDTO)
    {
        if (!string.IsNullOrEmpty(usuarioDTO.Contrasena) && usuarioDTO.Contrasena != null)
        {
            usuarioDTO.Contrasena = _PasswordHasherService.Hash(usuarioDTO.Contrasena);
            ApiResponse response = await _UsuariosService.Create(usuarioDTO);

            return StatusCode(
                statusCode: (int)response!.statusCode,
                value: response.value
            );
        }

        ApiResponse errorResponse = new(
            statusCode: System.Net.HttpStatusCode.BadRequest,
            message: $"Error de creación: La '{nameof(usuarioDTO.Contrasena)}' no puede estar vacia."
        );

        return StatusCode(
            statusCode: (int)errorResponse.statusCode,
            value: errorResponse.value
        );
    }

    [HttpPut("({id})")]
    public async Task<IActionResult> Usuarios(
        [NotNull] [Required] long id,
        [NotNull] [Required] [FromBody] CreateUsuarioDTO usuarioDTO
        )
    {
        ApiResponse response = await _UsuariosService.Update(id, usuarioDTO);

        return StatusCode(
            statusCode: (int)response!.statusCode,
            value: response.value
        );
    }

    [HttpPatch("({id})")]
    public async Task<IActionResult> Usuarios(
        [NotNull] [Required] long id,
        [NotNull] [Required] [FromBody] object usuarioDTO
        )
    {
        ApiResponse response = await _UsuariosService.Patch(id, usuarioDTO);

        return StatusCode(
            statusCode: (int)response!.statusCode,
            value: response.value
        );
    }

    [HttpDelete("({id})")]
    public async Task<IActionResult> Delete(long id)
    {
        ApiResponse response = await _UsuariosService.Delete(id);

        return StatusCode(
            statusCode: (int)response!.statusCode,
            value: response.value
        );
    }
}
