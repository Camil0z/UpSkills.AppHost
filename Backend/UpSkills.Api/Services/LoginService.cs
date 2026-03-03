using Microsoft.EntityFrameworkCore;
using UpSkills.Api.Interfaces;
using UpSkills.Infrastructure;
using UpSkills.Models.DTO;

namespace UpSkills.Api.Services;

public class LoginService (AppDbContext _AppDbContext, IPasswordHasherService _PasswordHasherService, IAuthService _AuthService) : ILoginService
{
    public async Task<ApiResponse> Authenticate(AuthDTO auth)
    {
        string errorMessage = string.Empty;
        try
        {
            auth.Correo = auth.Correo.ToLower();

            string? hashedPass = (await _AppDbContext.Usuario.AsNoTracking()
                .Select(x => new { x.Correo, x.Contrasena })
                .FirstOrDefaultAsync(x => x.Correo == auth.Correo))?.Contrasena;

            if (hashedPass == null)
                return new(
                    statusCode: System.Net.HttpStatusCode.NotFound,
                    message: $"El 'Correo' '{auth.Correo}' no existe en el sistema."
                );

            if (_PasswordHasherService.Verify(auth.Contrasena, hashedPass))
                return new(
                    statusCode: System.Net.HttpStatusCode.OK,
                    value: new {
                        SessionId = _AuthService.GenerateToken(auth.Correo),
                        SesionTimeout = TimeSpan.FromHours(1)
                    }
                );

            return new(
                statusCode: System.Net.HttpStatusCode.BadRequest,
                message: "El 'Correo' y/o la 'Contraseña' no son válidos."
            );
        }
        catch (Exception ex)
        {
            string msg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            return new(
                statusCode: System.Net.HttpStatusCode.InternalServerError,
                message: $"Error de autenticación: {msg}"
            );
        }
    }
}
