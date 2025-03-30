using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Components;
using Microsoft.AspNetCore.Components;
using GestionERP.Web.Services; 

namespace GestionERP.Web.Pages.Principal.MotivoTraslado;

public partial class View : IDisposable
{
    private const string codigoServicio = "S044";
    public MotivoTrasladoObtenerDto MotivoTrasladoObtener { get; set; }
    [Parameter] public Guid? Id { get; set; } 
    [CascadingParameter] public NotifyComponent Notify { get; set; }

    [Inject] public IPrincipalMotivoTraslado IMotivoTraslado { get; set; }
    [Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Notify.ShowLoading(mensaje: "Obteniendo registro");

            if (!(await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio)).esValido)
                return;

            if (!await IPermiso.ConsultaEsAsignadoPorSesion(MotivoTrasladoAcceso.VerRegistros))
            {
                INavigation.NavigateTo("inicio");
                Notify.Show("No tiene permiso para ver registros del servicio principal de [Motivos de Traslado]", "error");
                return;
            }

            MotivoTrasladoObtener = await IMotivoTraslado.Obtener((Guid) Id);

            if (MotivoTrasladoObtener is null)
            {
                INavigation.NavigateTo("motivos-traslado");
                Notify.Show($"El registro de la [Motivo de traslado] consultado a visualizar no está disponible", "error");
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

    private void Volver() => INavigation.NavigateTo("motivos-traslado");

    public void Dispose() => GC.SuppressFinalize(this);
}