using Blazored.FluentValidation;
using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Dtos.Principal;
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

namespace GestionERP.Web.Pages.Principal.Permiso;

public partial class Insert : IDisposable
{
    #region Propiedades
    private const string codigoServicio = "S031";
    public FluentValidationValidator validator;
    public PermisoInsertarDto PermisoInsertar { get; set; }
    public PermisoObtenerDto PermisoObtener { get; set; }
    private EditContext EditContext { get; set; }
    private bool IsAuthUser { get; set; }
    private bool IsLoadingAction { get; set; }
    private bool EnabledCodigo { get; set; }
    private string MaskCodigo { get; set; }
    private bool IsModified { get; set; }
    private bool EsVisibleVolver { get; set; }
    public TelerikNotification Alert { get; set; }
    [CascadingParameter] public DialogFactory Dialog { get; set; }
    [Parameter][SupplyParameterFromQuery(Name = "returnpage")] public string ReturnPage { get; set; }
    private ClaimsPrincipal User { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }
    #endregion

    [Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            PermisoInsertar = new();
            PermisoObtener = new();

            EditContext = new EditContext(PermisoInsertar);

            MaskCodigo = "";

            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio);
            if (!IsAuthUser) return;
                
                EsVisibleVolver = !string.IsNullOrEmpty(ReturnPage) && ReturnPage is "index";

                if (!await IPermiso.ConsultaEsAsignadoPorSesion(PermisoAcceso.Insertar))
                {
                    INavigation.NavigateTo("permisos");
                    Notify.Show("No tiene permiso para insertar registros de [Permisos]", "error");
                }
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
    }

    private async Task Insertar()
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

            Notify.ShowLoading(mensaje: "Inserción en progreso");

            Guid id = await IPermiso.Insertar(PermisoInsertar);

            IsModified = false;
            Notify.Show("El permiso ha sido insertado con éxito", "success");
            INavigation.NavigateTo($"permisos/{id}");
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
        if (IsAuthUser && IsModified && !await Dialog.ConfirmAsync("¿Está seguro de salir del formulario de insertar sin haber guardado?", "Saliendo del formulario"))
            context.PreventNavigation();
    }


    private void ValueCodigoUpperChanged(object codigo) => PermisoInsertar.Codigo = codigo?.ToString().ToUpper().Trim();

    private void Volver() => INavigation.NavigateTo("permisos");

    private void CargarItemCatalogoServicio(ServicioCatalogoDto item)
    {
        PermisoInsertar.CodigoServicio = item.CodigoServicio;
        PermisoObtener.NombreServicio = item.NombreServicio;
        if(!string.IsNullOrEmpty(PermisoInsertar.CodigoEvento))
        {
            string preCodigo = string.Concat(PermisoInsertar.CodigoServicio, PermisoInsertar.CodigoEvento);
            MaskCodigo = @$"\{preCodigo[0]}\{preCodigo[1]}\{preCodigo[2]}\{preCodigo[3]}\{preCodigo[4]}\{preCodigo[5]}AAAA";
            EnabledCodigo = true;
            if (PermisoInsertar.Codigo is not null)
            {
                PermisoInsertar.Codigo = PermisoInsertar.Codigo.Replace(PermisoInsertar.Codigo[..6], preCodigo);
            }
        }
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoServicio"));
        IsModified = true;
    }

    private void CargarItemCatalogoEvento(EventoCatalogoDto item)
    {
        PermisoInsertar.CodigoEvento = item.CodigoEvento;
        PermisoObtener.NombreEvento = item.NombreEvento;
        if (!string.IsNullOrEmpty(PermisoInsertar.CodigoServicio))
        {
            string preCodigo = string.Concat(PermisoInsertar.CodigoServicio, PermisoInsertar.CodigoEvento);
            MaskCodigo = @$"\{preCodigo[0]}\{preCodigo[1]}\{preCodigo[2]}\{preCodigo[3]}\{preCodigo[4]}\{preCodigo[5]}AAAA";
            EnabledCodigo = true;
            if (PermisoInsertar.Codigo is not null)
            {
                PermisoInsertar.Codigo = PermisoInsertar.Codigo.Replace(PermisoInsertar.Codigo[..6], preCodigo);
            }
        }
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoEvento"));
        IsModified = true;
    }

    public void Dispose() => GC.SuppressFinalize(this);
}