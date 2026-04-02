using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using RestSharp;
using UpSkills.BlazorClient.Interfaces;
using UpSkills.Models.DTO;

namespace UpSkills.BlazorClient.Pages;

public partial class MisInscripciones(IInternalApiRepository _Api, NavigationManager _Nav)
{
    List<GetInscripcionDTO> Inscripciones = new();
    bool Cargando = true;

    protected override async Task OnInitializedAsync()
    {
        var userIdStr = await JS.InvokeAsync<string?>("localStorage.getItem", "upskills_userId");
        if (!long.TryParse(userIdStr, out long uid))
        {
            _Nav.NavigateTo("/login");
            return;
        }

        RestResponse resp = await _Api.Request($"Inscripciones/usuario/{uid}");
        if (resp.IsSuccessStatusCode && !string.IsNullOrEmpty(resp.Content))
            Inscripciones = JsonConvert.DeserializeObject<List<GetInscripcionDTO>>(resp.Content) ?? new();

        Cargando = false;
    }
}
