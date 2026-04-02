using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using RestSharp;
using UpSkills.BlazorClient.Interfaces;
using UpSkills.Models.DTO;

namespace UpSkills.BlazorClient.Pages;

public partial class InstructorCursos(IInternalApiRepository _Api, NavigationManager _Nav)
{
    List<GetCursoDTO> Cursos = new();
    List<CategoriaDTO> Categorias = new();
    List<GetModuloDTO> Modulos = new();

    CreateCursoDTO Form = new();
    CreateModuloDTO FormModulo = new();
    CreateMaterialDTO FormMaterial = new();

    long? EditandoId = null;
    GetCursoDTO? CursoSeleccionado = null;
    long? ModuloParaMaterial = null;

    bool Cargando = true;
    bool MostrarFormulario = false;
    bool MostrarFormularioModulo = false;
    bool Guardando = false;
    bool GuardandoModulo = false;
    bool GuardandoMaterial = false;

    string MensajeExito = string.Empty;
    string MensajeError = string.Empty;
    long InstructorId = 0;

    protected override async Task OnInitializedAsync()
    {
        var userIdStr = await JS.InvokeAsync<string?>("localStorage.getItem", "upskills_userId");
        var rol = await JS.InvokeAsync<string?>("localStorage.getItem", "upskills_rol");

        if (!long.TryParse(userIdStr, out long uid) || rol != "Instructor")
        {
            _Nav.NavigateTo("/login");
            return;
        }

        InstructorId = uid;
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
        RestResponse resp = await _Api.Request($"Cursos/instructor/{InstructorId}");
        if (resp.IsSuccessStatusCode && !string.IsNullOrEmpty(resp.Content))
            Cursos = JsonConvert.DeserializeObject<List<GetCursoDTO>>(resp.Content) ?? new();
        else
            Cursos = new();
        Cargando = false;
    }

    private void AbrirFormulario()
    {
        EditandoId = null;
        Form = new CreateCursoDTO { InstructorId = InstructorId, Estado = 1 };
        MostrarFormulario = true;
    }

    private void EditarCurso(GetCursoDTO curso)
    {
        EditandoId = curso.Id;
        Form = new CreateCursoDTO
        {
            Titulo = curso.Titulo,
            Imagen = curso.Imagen,
            Descripcion = curso.Descripcion,
            Duracion = curso.Duracion,
            Precio = curso.Precio,
            Estado = curso.EstadoId,
            CategoriaId = curso.CategoriaId,
            InstructorId = InstructorId
        };
        MostrarFormulario = true;
    }

    private void CerrarFormulario()
    {
        MostrarFormulario = false;
        EditandoId = null;
    }

    private async Task GuardarCurso()
    {
        if (string.IsNullOrWhiteSpace(Form.Titulo) || Form.CategoriaId == 0)
        {
            MensajeError = "Título y categoría son obligatorios.";
            return;
        }

        MensajeError = string.Empty;
        Guardando = true;

        RestResponse resp;
        if (EditandoId.HasValue)
            resp = await _Api.Request($"Cursos/{EditandoId}", Method.Put, Form);
        else
            resp = await _Api.Request("Cursos", Method.Post, Form);

        if (resp.IsSuccessStatusCode)
        {
            MensajeExito = EditandoId.HasValue ? "Curso actualizado." : "Curso creado exitosamente.";
            MostrarFormulario = false;
            EditandoId = null;
            await CargarCursos();
        }
        else
        {
            MensajeError = "Error al guardar el curso. Verifica los datos.";
        }

        Guardando = false;
    }

    private async Task CambiarEstado(long cursoId, int estado)
    {
        RestResponse resp = await _Api.Request($"Cursos/{cursoId}/estado/{estado}", Method.Patch);
        if (resp.IsSuccessStatusCode)
        {
            MensajeExito = estado == 2 ? "Curso publicado." : "Curso movido a borrador.";
            await CargarCursos();
        }
    }

    private async Task GestionarModulos(GetCursoDTO curso)
    {
        CursoSeleccionado = curso;
        await CargarModulos(curso.Id);
    }

    private async Task CargarModulos(long cursoId)
    {
        RestResponse resp = await _Api.Request($"Modulos/curso/{cursoId}");
        if (resp.IsSuccessStatusCode && !string.IsNullOrEmpty(resp.Content))
            Modulos = JsonConvert.DeserializeObject<List<GetModuloDTO>>(resp.Content) ?? new();
        else
            Modulos = new();
    }

    private void AbrirFormularioModulo()
    {
        FormModulo = new CreateModuloDTO { CursoId = CursoSeleccionado!.Id, Orden = Modulos.Count + 1 };
        MostrarFormularioModulo = true;
    }

    private async Task GuardarModulo()
    {
        if (string.IsNullOrWhiteSpace(FormModulo.Titulo)) return;
        GuardandoModulo = true;
        RestResponse resp = await _Api.Request("Modulos", Method.Post, FormModulo);
        if (resp.IsSuccessStatusCode)
        {
            MostrarFormularioModulo = false;
            await CargarModulos(CursoSeleccionado!.Id);
        }
        GuardandoModulo = false;
    }

    private async Task EliminarModulo(long moduloId)
    {
        bool confirmar = await JS.InvokeAsync<bool>("confirm", "¿Eliminar este módulo y todos sus materiales?");
        if (!confirmar) return;
        RestResponse resp = await _Api.Request($"Modulos/{moduloId}", Method.Delete);
        if (resp.IsSuccessStatusCode)
            await CargarModulos(CursoSeleccionado!.Id);
    }

    private void AbrirFormularioMaterial(long moduloId)
    {
        FormMaterial = new CreateMaterialDTO { ModuloId = moduloId, Tipo = 2, Orden = 1 };
        ModuloParaMaterial = moduloId;
    }

    private async Task GuardarMaterial()
    {
        if (string.IsNullOrWhiteSpace(FormMaterial.Titulo) || string.IsNullOrWhiteSpace(FormMaterial.Url)) return;
        GuardandoMaterial = true;
        RestResponse resp = await _Api.Request("Materiales", Method.Post, FormMaterial);
        if (resp.IsSuccessStatusCode)
        {
            ModuloParaMaterial = null;
            await CargarModulos(CursoSeleccionado!.Id);
        }
        GuardandoMaterial = false;
    }

    private async Task EliminarMaterial(long materialId)
    {
        bool confirmar = await JS.InvokeAsync<bool>("confirm", "¿Eliminar este material?");
        if (!confirmar) return;
        RestResponse resp = await _Api.Request($"Materiales/{materialId}", Method.Delete);
        if (resp.IsSuccessStatusCode)
            await CargarModulos(CursoSeleccionado!.Id);
    }
}
