using Microsoft.EntityFrameworkCore;
using UpSkills.Api.Interfaces;
using UpSkills.Infrastructure;
using UpSkills.Models.DTO;

namespace UpSkills.Api.Services;

public class LoginService(AppDbContext _AppDbContext, IPasswordHasherService _PasswordHasherService, IAuthService _AuthService) : ILoginService
{
    public async Task<ApiResponse> Authenticate(AuthDTO auth)
    {
        try
        {
            auth.Correo = auth.Correo.ToLower();

            var usuario = await _AppDbContext.Usuario.AsNoTracking()
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(x => x.Correo == auth.Correo && x.Activo);

            if (usuario == null)
                return new(
                    statusCode: System.Net.HttpStatusCode.NotFound,
                    message: $"El correo '{auth.Correo}' no existe o la cuenta está inactiva."
                );

            if (_PasswordHasherService.Verify(auth.Contrasena, usuario.Contrasena))
                return new(
                    statusCode: System.Net.HttpStatusCode.OK,
                    value: new LoginResponseDTO
                    {
                        Token = _AuthService.GenerateToken(auth.Correo),
                        UserId = usuario.Id,
                        Rol = usuario.Rol.Nombre,
                        NombreCompleto = $"{usuario.PrimerNombre} {usuario.PrimerApellido}",
                        SesionTimeout = TimeSpan.FromHours(1)
                    }
                );

            return new(
                statusCode: System.Net.HttpStatusCode.BadRequest,
                message: "El correo y/o la contraseña no son válidos."
            );
        }
        catch (Exception ex)
        {
            string msg = ex.InnerException?.Message ?? ex.Message;
            return new(
                statusCode: System.Net.HttpStatusCode.InternalServerError,
                message: $"Error de autenticación: {msg}"
            );
        }
    }
}
