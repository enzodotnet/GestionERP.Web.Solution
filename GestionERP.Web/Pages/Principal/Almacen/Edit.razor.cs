 
using AutoMapper;
using Blazored.FluentValidation;
using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Models.Dtos.Principal.Types;
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Components;
using Microsoft.AspNetCore.Components;
using GestionERP.Web.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Telerik.Blazor;
using Telerik.Blazor.Components;
using GestionERP.Web.Global;

namespace GestionERP.Web.Pages.Principal.Almacen;

public partial class Edit : IDisposable
{
    #region Propiedades
    private const string codigoServicio = "S011";
    public FluentValidationValidator validator;
    public AlmacenEditarDto AlmacenEditar { get; set; }
    public AlmacenObtenerDto AlmacenObtener { get; set; }
    public bool EsVisibleAlert { get; set; }
    public string MensajeAlert { get; set; }
    private IEnumerable<AlmacenAtributoType> Atributos { get; set; }
    private EditContext EditContext { get; set; }
    private bool IsLoadingAction { get; set; }
    private bool IsAuthUser { get; set; }
    private bool IsModified { get; set; }
    private bool EsVisibleVolver { get; set; }
    private bool IsInitPage { get; set; }
    public TelerikNotification Alert { get; set; }
    [CascadingParameter] public DialogFactory Dialog { get; set; }
    [Parameter] public Guid? Id { get; set; }
    [Parameter][SupplyParameterFromQuery(Name = "returnpage")] public string ReturnPage { get; set; }
    private ClaimsPrincipal User { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }
    #endregion

    [Inject] public IPrincipalAlmacen IAlmacen { get; set; }
    [Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }
    [Inject] public IMapper IMapper { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Notify.ShowLoading(mensaje: "Obteniendo registro");  
            AlmacenEditar = new(); 

            Atributos = AlmacenAtributoType.ObtenerTipos();

            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio);
            if (!IsAuthUser) return;
                
            EsVisibleVolver = !string.IsNullOrEmpty(ReturnPage) && ReturnPage is "index" or "view";
                    
            if (!await IPermiso.ConsultaEsAsignadoPorSesion(AlmacenAcceso.Editar))
            {
                INavigation.NavigateTo("almacenes");
                Notify.Show("No tiene permiso para editar registros de [Almacenes]", "error");
                return;
            }

            AlmacenObtener = await IAlmacen.Obtener((Guid) Id);

            if (AlmacenObtener is null)
            {
                INavigation.NavigateTo("almacenes");
                Notify.Show("El registro del [Almacén] consultado a editar no existe", "error");
                return;
            }
            AlmacenEditar = IMapper.Map<AlmacenEditarDto>(AlmacenObtener);

            EditContext = new EditContext(AlmacenEditar);
            IsInitPage = true;
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

    private async Task Editar()
    {
        try
        {
            IsLoadingAction = true;

            IsAuthUser = (await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio, codigoUser: User.FindFirst("code").Value)).esValido;
            if (!IsAuthUser) return;
            if (!EditContext.Validate())
            {
                Fnc.MostrarAlerta(Alert, Cnf.MsgErrorInvalidEditContext, "error");
                return;
            }
             
            Notify.ShowLoading(mensaje: "Actualización en progreso");
 
            await IAlmacen.Editar((Guid) Id, AlmacenEditar);
            
            IsModified = false;
            Notify.Show("El almacén ha sido editado con éxito", "success");
            INavigation.NavigateTo($"almacenes/{Id}");
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
        if (IsAuthUser && IsModified && !await Dialog.ConfirmAsync("¿Está seguro de salir del formulario de editar sin haber actualizado?", "Saliendo del formulario"))
            context.PreventNavigation();
    }


    private void Volver() => INavigation.NavigateTo($"almacenes{(ReturnPage == "view" ? $"/{Id}" : "")}");

    #region Catalogos
    private void CargarItemCatalogoCuentaContable(CuentaContableCatalogoDto item)
    {
        if (string.IsNullOrEmpty(AlmacenEditar.CodigoCuentaContable) || AlmacenEditar.CodigoCuentaContable != item.CodigoCuentaContable)
        {
            AlmacenEditar.CodigoCuentaContable = item.CodigoCuentaContable;
            AlmacenObtener.NombreCuentaContable = item.NombreCuentaContable;
            IsModified = true;
        } 
    }

    private void CargarItemCatalogoTipoAlmacen(TipoAlmacenCatalogoDto item)
    {
        if (string.IsNullOrEmpty(AlmacenEditar.CodigoTipoAlmacen) || AlmacenEditar.CodigoTipoAlmacen != item.CodigoTipoAlmacen)
        {
            AlmacenEditar.CodigoTipoAlmacen = item.CodigoTipoAlmacen;
            AlmacenObtener.NombreTipoAlmacen = item.NombreTipoAlmacen;
            IsModified = true;
        } 
    }
    #endregion

    public void Dispose() => GC.SuppressFinalize(this);
}  