using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Models.Dtos.Principal.Types;
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Components;
using Microsoft.AspNetCore.Components;
using GestionERP.Web.Services; 

namespace GestionERP.Web.Pages.Principal.TipoAdjuntoCertificado;

public partial class View : IDisposable
{
    private const string codigoServicio = "S086";
    public TipoAdjuntoCertificadoObtenerDto TipoAdjuntoCertificadoObtener { get; set; }
    private IEnumerable<TipoAdjuntoCertificadoOrigenType> Origenes { get; set; }
    [Parameter] public Guid? Id { get; set; } 
    [CascadingParameter] public NotifyComponent Notify { get; set; }

    [Inject] public IPrincipalTipoAdjuntoCertificado ITipoAdjuntoCertificado { get; set; }
    [Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Notify.ShowLoading(mensaje: "Obteniendo registro");
            Origenes = TipoAdjuntoCertificadoOrigenType.ObtenerTipos();

			if (!(await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio)).esValido)
				return;

			if (!await IPermiso.ConsultaEsAsignadoPorSesion(TipoAdjuntoCertificadoAcceso.VerRegistros))
            {
                INavigation.NavigateTo("inicio");
                Notify.Show("No tiene permiso para ver registros del servicio principal de [Tipos de Adjuntos de Certificado]", "error");
                return;
            }

            TipoAdjuntoCertificadoObtener = await ITipoAdjuntoCertificado.Obtener((Guid)Id);

            if (TipoAdjuntoCertificadoObtener is null)
            {
                INavigation.NavigateTo("tipos-adjunto-certificado");
                Notify.Show("El registro del [Tipo de Adjunto de Certificado] consultado a visualizar no está disponible", "error");
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

    private void Volver() => INavigation.NavigateTo("tipos-adjunto-certificado");

    public void Dispose() => GC.SuppressFinalize(this);
}