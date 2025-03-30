using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Models.Dtos.Principal.Types;
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Components;
using Microsoft.AspNetCore.Components;
using GestionERP.Web.Services; 

namespace GestionERP.Web.Pages.Principal.TipoDocumento;

public partial class View : IDisposable
{
    private const string codigoServicio = "S070";
    public TipoDocumentoObtenerDto TipoDocumentoObtener { get; set; }
    private IEnumerable<TipoDocumentoAtributoType> Atributos { get; set; }
    public bool EsAsignadoVerProcesoDocumento { get; set; }
    [Parameter] public Guid? Id { get; set; } 
    [CascadingParameter] public NotifyComponent Notify { get; set; }

    [Inject] public IPrincipalTipoDocumento ITipoDocumento { get; set; }
    [Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Notify.ShowLoading(mensaje: "Obteniendo registro"); 
            Atributos = TipoDocumentoAtributoType.ObtenerTipos();

			if (!(await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio)).esValido)
				return;

			EsAsignadoVerProcesoDocumento = await IPermiso.ConsultaEsAsignadoPorSesion(ProcesoDocumentoAcceso.VerRegistros);

            if (!await IPermiso.ConsultaEsAsignadoPorSesion(TipoDocumentoAcceso.VerRegistros))
            {
                INavigation.NavigateTo("inicio");
                Notify.Show("No tiene permiso para ver registros del servicio principal de [Tipos de documento]", "error");
                return;
            }

            TipoDocumentoObtener = await ITipoDocumento.Obtener((Guid) Id);

            if (TipoDocumentoObtener is null)
            {
                INavigation.NavigateTo("tipos-documento");
                Notify.Show("El registro del [Tipo de documento] consultado a visualizar no está disponible", "error");
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

    private void Volver() => INavigation.NavigateTo("tipos-documento");

    private void IrVerProcesosDocumento() => INavigation.NavigateTo(INavigation.GetUriWithQueryParameters("procesos-documento", new Dictionary<string, object> { ["codigoTipoDocumento"] = TipoDocumentoObtener.Codigo.Trim() }));

    public void Dispose() => GC.SuppressFinalize(this);
}