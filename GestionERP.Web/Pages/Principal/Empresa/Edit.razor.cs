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

namespace GestionERP.Web.Pages.Principal.Empresa;

public partial class Edit : IDisposable
{
    #region Propiedades
    private const string codigoServicio = "S006";
    public FluentValidationValidator validator;
    public EmpresaEditarDto EmpresaEditar { get; set; }
    public EmpresaObtenerDto EmpresaObtener { get; set; }
    public TelerikNotification Alert { get; set; }
    private IEnumerable<EmpresaAtributoTipoRubroType> TiposRubro { get; set; }
    private EditContext EditContext { get; set; }
    private bool IsLoadingAction { get; set; }
    private bool IsAuthUser { get; set; }
    private bool IsModified { get; set; }
    private bool EsVisibleVolver { get; set; }
    private bool IsInitPage { get; set; }
    [CascadingParameter] public DialogFactory Dialog { get; set; }
    [Parameter] public Guid? Id { get; set; }
    [Parameter][SupplyParameterFromQuery(Name = "returnpage")] public string ReturnPage { get; set; }
    private ClaimsPrincipal User { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }
    #endregion

    [Inject] public IPrincipalEmpresa IEmpresa { get; set; } 
    [Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }
    [Inject] public IMapper IMapper { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Notify.ShowLoading(mensaje: "Obteniendo registro"); 

            EmpresaEditar = new()
            {
                FacturacionEditar = new(),
                AtributoEditar = new()
            }; 

            TiposRubro = EmpresaAtributoTipoRubroType.ObtenerTipos();

            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio);
            if (!IsAuthUser) return;
                
            EsVisibleVolver = !string.IsNullOrEmpty(ReturnPage) && ReturnPage is "index" or "view";

            if (!await IPermiso.ConsultaEsAsignadoPorSesion(EmpresaAcceso.Editar))
            {
                INavigation.NavigateTo("empresas");
                Notify.Show("No tiene permiso para editar registros de [Empresas]", "error");
                return;
            }

            EmpresaObtener = await IEmpresa.Obtener((Guid) Id); 
            if (EmpresaObtener is null)
            {
                INavigation.NavigateTo("empresas");
                Notify.Show($"El registro de la [Empresa] consultado a editar no existe", "error");
                return;
            }
            EmpresaEditar = IMapper.Map<EmpresaEditarDto>(EmpresaObtener);
            EmpresaEditar.AtributoEditar = IMapper.Map<EmpresaAtributoEditarDto>(EmpresaObtener.Atributo);
            EmpresaEditar.FacturacionEditar = IMapper.Map<EmpresaFacturacionEditarDto>(EmpresaObtener.Facturacion);

            EditContext = new EditContext(EmpresaEditar);
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
            
            await IEmpresa.Editar((Guid) Id, EmpresaEditar);
            IsModified = false;
            Notify.Show("La empresa ha sido editada con éxito", "success");
            INavigation.NavigateTo($"empresas/{Id}");
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


    private void Volver() => INavigation.NavigateTo($"empresas{(ReturnPage == "view" ? $"/{Id}" : "")}");
 
    #region Catalogos

    private void CargarItemCatalogoCuentaContableOrdenDeudor(CuentaContableCatalogoDto item)
    {
        EmpresaEditar.AtributoEditar.CodigoCuentaContableOrdenDeudor = item.CodigoCuentaContable;
        EmpresaObtener.Atributo.NombreCuentaContableOrdenDeudor = item.NombreCuentaContable; 
        IsModified = true;
    }

    private void CargarItemCatalogoCuentaContableOrdenAcreedor(CuentaContableCatalogoDto item)
    {
        EmpresaEditar.AtributoEditar.CodigoCuentaContableOrdenAcreedor = item.CodigoCuentaContable;
        EmpresaObtener.Atributo.NombreCuentaContableOrdenAcreedor = item.NombreCuentaContable; 
        IsModified = true;
    }

    private void CargarItemCatalogoCuentaContableImpuestoExtorno(CuentaContableCatalogoDto item)
    {
        if (string.IsNullOrEmpty(EmpresaEditar.AtributoEditar.CodigoCuentaContableImpuestoExtorno) || EmpresaEditar.AtributoEditar.CodigoCuentaContableImpuestoExtorno != item.CodigoCuentaContable)
        {
            EmpresaEditar.AtributoEditar.CodigoCuentaContableImpuestoExtorno = item.CodigoCuentaContable;
            EmpresaObtener.Atributo.NombreCuentaContableImpuestoExtorno = item.NombreCuentaContable;
            IsModified = true;
        } 
    }
    #endregion

    public void Dispose() => GC.SuppressFinalize(this);
}