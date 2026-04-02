using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using RestSharp;
using UpSkills.BlazorClient.Interfaces;
using UpSkills.Models.DTO;

namespace UpSkills.BlazorClient.Pages;

public partial class Login(IInternalApiRepository _Api, NavigationManager _Nav)
{
    string Correo = string.Empty;
    string Contrasena = string.Empty;
    string MensajeError = string.Empty;
    string MensajeExito = string.Empty;
    bool Cargando = false;

    private async Task HandleLogin()
    {
        MensajeError = string.Empty;
        MensajeExito = string.Empty;
        Cargando = true;

        try
        {
            var dto = new AuthDTO { Correo = Correo, Contrasena = Contrasena };
            RestResponse response = await _Api.Request("Login", Method.Post, dto);

            if (response.IsSuccessStatusCode)
            {
                var loginResult = JsonConvert.DeserializeObject<LoginResponseDTO>(response.Content ?? "{}");
                if (loginResult != null)
                {
                    await JS.InvokeVoidAsync("localStorage.setItem", "upskills_token", loginResult.Token);
                    await JS.InvokeVoidAsync("localStorage.setItem", "upskills_userId", loginResult.UserId.ToString());
                    await JS.InvokeVoidAsync("localStorage.setItem", "upskills_rol", loginResult.Rol);
                    await JS.InvokeVoidAsync("localStorage.setItem", "upskills_nombre", loginResult.NombreCompleto);
                    MensajeExito = $"¡Bienvenido, {loginResult.NombreCompleto}! Redirigiendo...";
                    await Task.Delay(800);
                    _Nav.NavigateTo(loginResult.Rol == "Instructor" ? "/instructor/cursos" : "/cursos");
                }
            }
            else
            {
                MensajeError = "Correo o contraseña incorrectos.";
            }
        }
        catch
        {
            MensajeError = "Error de conexión con el servidor.";
        }
        finally
        {
            Cargando = false;
        }
    }
}
