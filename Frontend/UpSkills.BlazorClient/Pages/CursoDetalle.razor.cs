using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using RestSharp;
using UpSkills.BlazorClient.Interfaces;
using UpSkills.Models.DTO;

namespace UpSkills.BlazorClient.Pages;

public partial class CursoDetalle(IInternalApiRepository _Api, NavigationManager _Nav)
{
    [Parameter] public long Id { get; set; }

    GetCursoDTO? Curso;
    List<GetModuloDTO> Modulos = new();
    HashSet<long> ModulosAbiertos = new();

    bool Cargando = true;
    bool YaInscrito = false;
    bool SesionActiva = false;
    bool InscribiendoSe = false;
    string MensajeError = string.Empty;
    string MensajeExito = string.Empty;
    long UsuarioId = 0;

    protected override async Task OnInitializedAsync()
    {
        var userIdStr = await JS.InvokeAsync<string?>("localStorage.getItem", "upskills_userId");
        if (long.TryParse(userIdStr, out long uid))
        {
            UsuarioId = uid;
            SesionActiva = true;
        }

        await CargarCurso();
    }

    private async Task CargarCurso()
    {
        Cargando = true;

        RestResponse resp = await _Api.Request($"Cursos/{Id}");
        if (resp.IsSuccessStatusCode && !string.IsNullOrEmpty(resp.Content))
        {
            Curso = JsonConvert.DeserializeObject<GetCursoDTO>(resp.Content);
        }

        if (Curso != null)
        {
            RestResponse modResp = await _Api.Request($"Modulos/curso/{Id}");
            if (modResp.IsSuccessStatusCode && !string.IsNullOrEmpty(modResp.Content))
                Modulos = JsonConvert.DeserializeObject<List<GetModuloDTO>>(modResp.Content) ?? new();

            if (SesionActiva)
                await VerificarInscripcion();
        }

        Cargando = false;
    }

    private async Task VerificarInscripcion()
    {
        RestResponse resp = await _Api.Request($"Inscripciones/usuario/{UsuarioId}");
        if (resp.IsSuccessStatusCode && !string.IsNullOrEmpty(resp.Content))
        {
            var inscripciones = JsonConvert.DeserializeObject<List<GetInscripcionDTO>>(resp.Content) ?? new();
            YaInscrito = inscripciones.Any(i => i.CursoId == Id && i.EstadoId != 3); // 3=Rechazada
        }
    }

    private async Task Inscribirse()
    {
        if (!SesionActiva)
        {
            _Nav.NavigateTo("/login");
            return;
        }

        MensajeError = string.Empty;
        MensajeExito = string.Empty;
        InscribiendoSe = true;

        try
        {
            var dto = new CreateInscripcionDTO { UsuarioId = UsuarioId, CursoId = Id };
            RestResponse resp = await _Api.Request("Inscripciones", Method.Post, dto);

            if (resp.IsSuccessStatusCode)
            {
                YaInscrito = true;
                MensajeExito = "¡Te has inscrito exitosamente! Tu inscripción está pendiente de aprobación.";
            }
            else if (resp.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                MensajeError = "Ya tienes una inscripción activa para este curso.";
            }
            else
            {
                MensajeError = "No se pudo procesar la inscripción. Intenta nuevamente.";
            }
        }
        catch
        {
            MensajeError = "Error de conexión. Intenta nuevamente.";
        }
        finally
        {
            InscribiendoSe = false;
        }
    }

    private void ToggleModulo(long moduloId)
    {
        if (ModulosAbiertos.Contains(moduloId))
            ModulosAbiertos.Remove(moduloId);
        else
            ModulosAbiertos.Add(moduloId);
    }
}
