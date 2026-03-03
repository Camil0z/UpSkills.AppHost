using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using RestSharp;
using UpSkills.BlazorClient.Interfaces;
using UpSkills.Models.DTO;

namespace UpSkills.BlazorClient.Pages;

public partial class Register(IInternalApiRepository _InternalApiRepository)
{
    CreateUsuarioDTO FormData = new CreateUsuarioDTO();
    IEnumerable<PaisDTO> IEPaises = [];
    IEnumerable<RolDTO> IERoles = [];

    protected override async Task OnInitializedAsync()
    {
        RestResponse response = await _InternalApiRepository.Request("Paises");
        if (response.IsSuccessStatusCode)
        {
            IEPaises = JsonConvert.DeserializeObject<IEnumerable<PaisDTO>>(response.Content ?? "{}") ?? [];
        }

        response = await _InternalApiRepository.Request("Roles");
        if (response.IsSuccessStatusCode)
        {
            IERoles = JsonConvert.DeserializeObject<IEnumerable<RolDTO>>(response.Content ?? "{}") ?? [];
        }
    }

    private async Task<EventCallback> RegistrateUser() //(object receiver, Action ActionCallBack)
    {
        RestResponse response = await _InternalApiRepository.Request("Usuarios", Method.Post, FormData);
        return new();
    }
}
