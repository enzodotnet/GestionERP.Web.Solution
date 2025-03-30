using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Components;
using Microsoft.AspNetCore.Components;
using GestionERP.Web.Services;
using GestionERP.Web.Models.Dtos.Principal.Types;

namespace GestionERP.Web.Pages.Principal.ProcesoDocumento;

public partial class View : IDisposable
{
    private const string codigoServicio = "S071";
    public ProcesoDocumentoObtenerDto ProcesoDocumentoObtener { get; set; }
	public IEnumerable<ProcesoDocumentoTipoAccionType> TiposAccion { get; set; }
	[Parameter] public Guid? Id { get; set; } 
    [CascadingParameter] public NotifyComponent Notify { get; set; }

    [Inject] public IPrincipalProcesoDocumento IProcesoDocumento { get; set; }
    [Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Notify.ShowLoading(mensaje: "Obteniendo registro");
			TiposAccion = ProcesoDocumentoTipoAccionType.ObtenerTiposAccion();

            if (!(await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio)).esValido)
                return;

            if (!await IPermiso.ConsultaEsAsignadoPorSesion(ProcesoDocumentoAcceso.VerRegistros))
            {
                INavigation.NavigateTo("inicio");
                Notify.Show("No tiene permiso para ver registros del servicio principal de [Tipos de Procesos de Documentos]", "error");
                return;
            }

            ProcesoDocumentoObtener = await IProcesoDocumento.Obtener((Guid) Id);
            if (ProcesoDocumentoObtener is null)
            {
                INavigation.NavigateTo("procesos-documento");
                Notify.Show("El registro del [Tipo de proceso de documento] consultado a visualizar no está disponible", "error");
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

    private void Volver() => INavigation.NavigateTo("procesos-documento");

    public void Dispose() => GC.SuppressFinalize(this);
}