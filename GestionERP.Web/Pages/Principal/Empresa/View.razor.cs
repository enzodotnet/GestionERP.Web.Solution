using GestionERP.Web.Global;
using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Models.Dtos.Principal.Types;
using GestionERP.Web.Models.Dtos.Archivo;
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Components;
using Microsoft.AspNetCore.Components;
using GestionERP.Web.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Routing;
using Telerik.Blazor;
using Telerik.Blazor.Components;

namespace GestionERP.Web.Pages.Principal.Empresa;

public partial class View : IDisposable
{
    #region Propiedades
    private const string codigoServicio = "S006";
    public EmpresaObtenerDto EmpresaObtener { get; set; }
    private PrincipalEmpresaInsertarDto ArchivoInsertar { get; set; }
    private IEnumerable<PrincipalEmpresaListarDto> ListaArchivos { get; set; }
    private IEnumerable<EmpresaAtributoTipoRubroType> TiposRubro { get; set; }
    private bool EsVisibleDialogEliminar { get; set; }
    private bool EsVisibleDialogEliminarLogo { get; set; }
    private bool EsVisibleModalImagen { get; set; }
    private bool EsAsignadoEliminar { get; set; }
    private bool EsAsignadoEditar { get; set; }
    private bool AccesoActualizarLogo { get; set; }
    private bool IsLoadingAction { get; set; }
    private bool IsAuthUser { get; set; }
    public string Base64ImgLogo { get; set; }
    public FileSelectFileInfo FileLogo { get; set; }
    public Guid? ArchivoId { get; set; }
    public string UrlArchivoLogo { get; set; }
    public string UriArchivoLogoView { get; set; }
    public bool IsUploadingFile { get; set; }
    public bool IsSelectedFile { get; set; }
    public bool ItemFileSelectCargado { get; set; }
    public List<string> AllowedExtensions { get; set; } = [".jpeg", ".jpg", ".png"];
    public TelerikNotification Alert { get; set; }
    [Parameter] public Guid? Id { get; set; } 
    private ClaimsPrincipal User { get; set; }
    [CascadingParameter] public DialogFactory Dialog { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }
    #endregion

    [Inject] public IPrincipalEmpresa IEmpresa { get; set; }
    [Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public IArchivoPrincipalEmpresa IArchivo { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Notify.ShowLoading(mensaje: "Obteniendo registro");
            FileLogo = null;
            TiposRubro = EmpresaAtributoTipoRubroType.ObtenerTipos();

            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio);
            if (!IsAuthUser) return;

            EsAsignadoEditar = await IPermiso.ConsultaEsAsignadoPorSesion(EmpresaAcceso.Editar);
            EsAsignadoEliminar = await IPermiso.ConsultaEsAsignadoPorSesion(EmpresaAcceso.Eliminar);
            AccesoActualizarLogo = await IPermiso.ConsultaEsAsignadoPorSesion(EmpresaAcceso.ActualizarLogo);

            if (!await IPermiso.ConsultaEsAsignadoPorSesion(EmpresaAcceso.VerRegistros))
            {
                INavigation.NavigateTo("inicio");
                Notify.Show("No tiene permiso para ver registros del servicio principal de [Empresas]", "error");
                return;
            }

            EmpresaObtener = await IEmpresa.Obtener((Guid) Id);
            if (EmpresaObtener is null)
            {
                INavigation.NavigateTo("empresas");
                Notify.Show($"El registro de la [Empresa] consultado a visualizar no está disponible", "error");
                return;
            }

            await CargarArchivoLogo();

            ItemFileSelectCargado = true;
        }
        catch (Exception ex)
        {
            if (ex is HttpRequestException)
                Notify.ShowError("NC");
            else if (ex is HttpResponseException)
                Notify.ShowError((ex as HttpResponseException).Code, ex);
            else
                Notify.ShowError("FA", ex);
        }
        finally
        {
            Notify.ShowLoading(false);
        }
    }

    private async Task Eliminar()
    {
        try
        {
            EsVisibleDialogEliminar = false;
            IsLoadingAction = true;

            IsAuthUser = (await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio, codigoUser: User.FindFirst("code").Value)).esValido;
            if (!IsAuthUser) return;

            Notify.ShowLoading(mensaje: "Eliminación en progreso");
             
            await IEmpresa.Eliminar((Guid) Id);
            IsLoadingAction = false;
            INavigation.NavigateTo("empresas");
            Notify.Show($"La empresa {EmpresaObtener.Codigo.Trim()} ha sido eliminado con éxito", "success");
        }
        catch (Exception ex)
        {
            if (ex is HttpRequestException)
                Notify.ShowError("NC");
            else if (ex is HttpResponseException)
                Notify.ShowError((ex as HttpResponseException).Code, ex);
            else
                Notify.ShowError("FA", ex);
        }
        finally
        {
            IsLoadingAction = false;
            Notify.ShowLoading(false);
        }
    }

    private async Task Cerrar(LocationChangingContext context)
    {
        if (IsAuthUser && (IsSelectedFile | IsLoadingAction) && !await Dialog.ConfirmAsync("¿Está seguro de salir del formulario sin haber culminado la acción?", "Saliendo del formulario"))
            context.PreventNavigation();
    }

    private async Task CargarArchivoLogo()
    {  
        ListaArchivos = await IArchivo.Listar((Guid) Id) ?? [];
        foreach (PrincipalEmpresaListarDto item in ListaArchivos.Where(x => x.FlagTipoArchivo == "L"))
        {
            ArchivoId = item.Id;
            UrlArchivoLogo = UriArchivoLogoView = item.UrlArchivo;
        } 
    }

    private void LimpiarArchivoLogo()
    {
        ArchivoId = null;
        UrlArchivoLogo = UriArchivoLogoView = "";
    }

    private void DeshacerUploadFile()
    {
        FileLogo = null;
        IsSelectedFile = false;
        Base64ImgLogo = null; 
        IsUploadingFile = false; 
        UriArchivoLogoView = ArchivoId is null ? "" : UrlArchivoLogo; 
    }

    private void HandleRemoveFileLogo()
    {
        FileLogo = null;
        IsSelectedFile = false;
        Base64ImgLogo = null;
        UriArchivoLogoView = "";
    }

    private async Task HandleSelectedFileLogo(FileSelectEventArgs args)
    {
        foreach (FileSelectFileInfo file in args.Files)
        {
            if (!file.InvalidExtension)
            { 
                using MemoryStream ms = new();
                await file.Stream.CopyToAsync(ms);
                string base64File = Convert.ToBase64String(ms.ToArray());
                FileLogo = new();
                FileLogo = file;
                Base64ImgLogo = base64File;
                IsSelectedFile = true;
                UriArchivoLogoView = $"data:{Fnc.GetMimeType(file.Extension)};base64,{base64File}";
            }
        }
    }

    private async Task SubirArchivo()
    {
        try
        {
            IsLoadingAction = true;

            IsAuthUser = (await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio, codigoUser: User.FindFirst("code").Value)).esValido;
            if (!IsAuthUser) return;

            Notify.ShowLoading(mensaje: "Subiendo archivo al repositorio");

            if (!await IPermiso.ConsultaEsAsignadoPorSesion(EmpresaAcceso.ActualizarLogo))
            {
                Notify.Show("No tiene permiso para actualizar imagen del logo de [Empresas]", "error");
                return;
            }
            
            ArchivoInsertar = new()
            {
                OrigenArchivoId = (Guid) Id,
                FlagTipoArchivo = "L", //Logo
                NombreArchivo = string.Concat("logo_", Id, "_", DateTime.Now.ToString("ddMMyyhhmmssfff"), FileLogo.Extension),
                Base64Archivo = Base64ImgLogo,
                TipoContenido = Fnc.GetMimeType(FileLogo.Extension)
            };

            if (ArchivoId.HasValue) 
                await IArchivo.Eliminar((Guid) ArchivoId);

            await IArchivo.Insertar(EmpresaObtener.Codigo, ArchivoInsertar);

            Notify.Show($"El archivo de logo ha sido subido al repositorio con éxito", "success");

            await CargarArchivoLogo();
            DeshacerUploadFile();
        }
        catch (Exception ex)
        {
            if (ex is HttpRequestException)
                Notify.ShowError("NC");
            else if (ex is HttpResponseException)
                Notify.ShowError((ex as HttpResponseException).Code, ex);
            else
                Notify.ShowError("FA", ex);
        }
        finally
        {
            IsLoadingAction = false;
            Notify.ShowLoading(false);
        }
    } 

    private async Task EliminarArchivo()
    {
        try
        {
            IsLoadingAction = true;
            EsVisibleDialogEliminarLogo = false;

            IsAuthUser = (await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio, codigoUser: User.FindFirst("code").Value)).esValido;
            if (!IsAuthUser) return;
             
            Notify.ShowLoading(mensaje: "Eliminación de archivo del repositorio en progreso");

            if (!await IPermiso.ConsultaEsAsignadoPorSesion(EmpresaAcceso.ActualizarLogo))
            {
                Notify.Show("No tiene permiso para actualizar imagen del logo de [Empresas]", "error");
                return;
            }

            await IArchivo.Eliminar((Guid) ArchivoId);

            Notify.Show($"El archivo de logo ha sido eliminado del repositorio con éxito", "success");

            LimpiarArchivoLogo();
        }
        catch (Exception ex)
        {
            if (ex is HttpRequestException)
                Notify.ShowError("NC");
            else if (ex is HttpResponseException)
                Notify.ShowError((ex as HttpResponseException).Code, ex);
            else
                Notify.ShowError("FA", ex);
        }
        finally
        {
            IsLoadingAction = false;
            Notify.ShowLoading(false);
        }
    }

    private void Volver() => INavigation.NavigateTo("empresas");

    private void IrEditar() => INavigation.NavigateTo(INavigation.GetUriWithQueryParameters($"empresas/{Id}/editar", new Dictionary<string, object> { ["returnpage"] = "view" }));

    public void Dispose() => GC.SuppressFinalize(this);
} 