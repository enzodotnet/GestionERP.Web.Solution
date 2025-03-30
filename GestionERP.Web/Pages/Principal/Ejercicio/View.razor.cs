using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Components;
using Microsoft.AspNetCore.Components;
using GestionERP.Web.Services; 

namespace GestionERP.Web.Pages.Principal.Ejercicio;

public partial class View : IDisposable
{
    private const string codigoServicio = "S054";
    public EjercicioObtenerDto EjercicioObtener { get; set; }
    public bool EsAsignadoVerRegistrosPeriodo { get; set; }
    [Parameter] public Guid? Id { get; set; } 
    [CascadingParameter] public NotifyComponent Notify { get; set; }

    [Inject] public IPrincipalEjercicio IEjercicio { get; set; }
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

			EsAsignadoVerRegistrosPeriodo = await IPermiso.ConsultaEsAsignadoPorSesion(PeriodoAcceso.VerRegistros);

            if (!await IPermiso.ConsultaEsAsignadoPorSesion(EjercicioAcceso.VerRegistros))
            {
                INavigation.NavigateTo("inicio");
                Notify.Show("No tiene permiso para ver registros del servicio principal de [Ejercicios]", "error");
                return;
            }

            EjercicioObtener = await IEjercicio.Obtener((Guid) Id);

            if (EjercicioObtener is null)
            {
                INavigation.NavigateTo("ejercicios");
                Notify.Show("El registro del [Ejercicio] consultado a visualizar no está disponible", "error");
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

    private void Volver() => INavigation.NavigateTo("ejercicios");

    private void IrVerPeriodos() => INavigation.NavigateTo(INavigation.GetUriWithQueryParameters("periodos", new Dictionary<string, object> { ["codigoEjercicio"] = EjercicioObtener.Codigo.Trim() }));

    public void Dispose() => GC.SuppressFinalize(this);
} 