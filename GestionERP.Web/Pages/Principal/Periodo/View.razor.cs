﻿using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Components;
using Microsoft.AspNetCore.Components;
using GestionERP.Web.Services; 

namespace GestionERP.Web.Pages.Principal.Periodo;

public partial class View : IDisposable
{ 
    private const string codigoServicio = "S055";
    public PeriodoObtenerDto PeriodoObtener { get; set; }  
    [Parameter] public Guid? Id { get; set; } 
    [CascadingParameter] public NotifyComponent Notify { get; set; }

    [Inject] public IPrincipalPeriodo IPeriodo { get; set; }
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

            if (!await IPermiso.ConsultaEsAsignadoPorSesion(PeriodoAcceso.VerRegistros))
            {
                INavigation.NavigateTo("inicio");
                Notify.Show("No tiene permiso para ver registros del servicio principal de [Periodos]", "error");
                return;
            }

            PeriodoObtener = await IPeriodo.Obtener((Guid) Id);

            if (PeriodoObtener is null)
            {
                INavigation.NavigateTo("periodos");
                Notify.Show("El registro del [Periodo] consultado a visualizar no está disponible", "error");
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

    private void Volver() => INavigation.NavigateTo("periodos");

    public void Dispose() => GC.SuppressFinalize(this);
}