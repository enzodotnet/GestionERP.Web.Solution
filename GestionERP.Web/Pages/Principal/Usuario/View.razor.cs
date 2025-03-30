using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Models.Dtos.Principal.Types;
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Components;
using Microsoft.AspNetCore.Components;
using GestionERP.Web.Services;
using System.Security.Claims;
using Telerik.Blazor.Components;
using Telerik.Blazor;
using Microsoft.AspNetCore.Components.Routing;

namespace GestionERP.Web.Pages.Principal.Usuario;

public partial class View : IDisposable
{
    #region Propiedades
    private const string codigoServicio = "S001";
    public UsuarioObtenerDto UsuarioObtener { get; set; } 
    private IEnumerable<UsuarioTipoPerfilType> TiposPerfil { get; set; } 
    private bool EsVisibleDialogEliminar { get; set; }
    private bool EsAsignadoEliminar { get; set; }
    private bool EsAsignadoEditar { get; set; }
    private bool IsLoadingAction { get; set; }
    private bool IsAuthUser { get; set; }
    [Parameter] public Guid? Id { get; set; }
    private ClaimsPrincipal User { get; set; }
    [CascadingParameter] public DialogFactory Dialog { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }
    #endregion

    [Inject] public IPrincipalUsuario IUsuario { get; set; }
    [Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Notify.ShowLoading(mensaje: "Obteniendo registro"); 

            TiposPerfil = UsuarioTipoPerfilType.ObtenerTipos();

            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio);
            if (!IsAuthUser) return;
                 
            EsAsignadoEditar = await IPermiso.ConsultaEsAsignadoPorSesion(UsuarioAcceso.Editar);
            EsAsignadoEliminar = await IPermiso.ConsultaEsAsignadoPorSesion(UsuarioAcceso.Eliminar);

            if (!await IPermiso.ConsultaEsAsignadoPorSesion(UsuarioAcceso.VerRegistros))
            {
                INavigation.NavigateTo("inicio");
                Notify.Show("No tiene permiso para ver registros del servicio principal de [Usuarios]", "error");
                return;
            }

            UsuarioObtener = await IUsuario.Obtener((Guid) Id);
            if (UsuarioObtener is null)
            {
                INavigation.NavigateTo("usuarios");
                Notify.Show("El registro del [Usuario] consultado a visualizar no está disponible", "error");
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

            await IUsuario.Eliminar((Guid) Id);
            IsLoadingAction = false;
            INavigation.NavigateTo("usuarios");
            Notify.Show($"El usuario {UsuarioObtener.Codigo.Trim()} ha sido eliminado con éxito", "success");
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

    protected static void OnStateInitServiciosHandler(GridStateEventArgs<UsuarioServicioObtenerDto> args)
    {
        GridState<UsuarioServicioObtenerDto> desiredState = new()
        {
            GroupDescriptors =
            [
                new()
                {
                    Member = nameof(UsuarioServicioObtenerDto.NombreModulo),
                    MemberType = typeof(string)
                }
            ]
        };
        args.GridState = desiredState;
    }

    protected static void OnStateInitPermisosHandler(GridStateEventArgs<UsuarioPermisoObtenerDto> args)
    {
        GridState<UsuarioPermisoObtenerDto> desiredState = new()
        {
            GroupDescriptors =
            [
                new()
                {
                    Member = nameof(UsuarioPermisoObtenerDto.NombreModulo),
                    MemberType = typeof(string)
                },
                new()
                {
                    Member = nameof(UsuarioPermisoObtenerDto.NombreServicio),
                    MemberType = typeof(string)
                }
            ]
        };
        args.GridState = desiredState;
    }

    public async Task Cerrar(LocationChangingContext context)
    {
        if (IsAuthUser && IsLoadingAction && !await Dialog.ConfirmAsync("¿Está seguro de salir del formulario sin haber culminado la acción?", "Saliendo del formulario"))
            context.PreventNavigation();
    }

    private void Volver() => INavigation.NavigateTo("usuarios");

    private void IrEditar() => INavigation.NavigateTo(INavigation.GetUriWithQueryParameters($"/usuarios/{Id}/editar", new Dictionary<string, object> { ["returnpage"] = "view" }));

    public void Dispose() => GC.SuppressFinalize(this);
}