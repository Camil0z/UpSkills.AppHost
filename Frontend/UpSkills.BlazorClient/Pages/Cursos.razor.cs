using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using RestSharp;
using UpSkills.BlazorClient.Interfaces;
using UpSkills.Models.DTO;

namespace UpSkills.BlazorClient.Pages;

public partial class Cursos(IInternalApiRepository _Api)
{
    List<GetCursoDTO> Cursos = new();
    List<CategoriaDTO> Categorias = new();

    string Busqueda = string.Empty;
    string CategoriaSeleccionada = string.Empty;
    decimal? PrecioMax = null;
    int? DuracionMax = null;

    int PaginaActual = 1;
    int TotalPaginas = 1;
    int TotalItems = 0;
    const int TamañoPagina = 9;

    bool Cargando = true;

    protected override async Task OnInitializedAsync()
    {
        await CargarCategorias();
        await CargarCursos();
    }

    private async Task CargarCategorias()
    {
        RestResponse resp = await _Api.Request("Categorias");
        if (resp.IsSuccessStatusCode && !string.IsNullOrEmpty(resp.Content))
            Categorias = JsonConvert.DeserializeObject<List<CategoriaDTO>>(resp.Content) ?? new();
    }

    private async Task CargarCursos()
    {
        Cargando = true;
        StateHasChanged();

        string query = $"Cursos?pagina={PaginaActual}&tamano={TamañoPagina}";
        if (!string.IsNullOrWhiteSpace(Busqueda)) query += $"&busqueda={Uri.EscapeDataString(Busqueda)}";
        if (!string.IsNullOrEmpty(CategoriaSeleccionada)) query += $"&categoriaId={CategoriaSeleccionada}";
        if (PrecioMax.HasValue) query += $"&precioMax={PrecioMax}";
        if (DuracionMax.HasValue) query += $"&duracionMax={DuracionMax}";

        RestResponse resp = await _Api.Request(query);
        if (resp.IsSuccessStatusCode && !string.IsNullOrEmpty(resp.Content))
        {
            var resultado = JsonConvert.DeserializeObject<CursoPaginadoDTO>(resp.Content);
            if (resultado != null)
            {
                Cursos = resultado.Items;
                TotalItems = resultado.TotalItems;
                PaginaActual = resultado.PaginaActual;
                TotalPaginas = resultado.TotalPaginas;
            }
        }
        else
        {
            Cursos = new();
        }

        Cargando = false;
    }

    private async Task BuscarCursos()
    {
        PaginaActual = 1;
        await CargarCursos();
    }

    private async Task OnFiltroChange(ChangeEventArgs _)
    {
        PaginaActual = 1;
        await CargarCursos();
    }

    private async Task OnBusquedaKeyUp(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
            await BuscarCursos();
    }

    private async Task CambiarPagina(int pagina)
    {
        if (pagina < 1 || pagina > TotalPaginas) return;
        PaginaActual = pagina;
        await CargarCursos();
    }
}
