﻿using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Dtos.Principal;    
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Components;
using Microsoft.AspNetCore.Components;
using GestionERP.Web.Services;
using System.Security.Claims;  
using Telerik.Blazor;
using Microsoft.AspNetCore.Components.Routing;

namespace GestionERP.Web.Pages.Principal.TransporteImportacion;

public partial class View : IDisposable
{
    #region Propiedades
    private const string codigoServicio = "S036";
    public TransporteImportacionObtenerDto TransporteObtener { get; set; } 
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

    [Inject] public IPrincipalTransporteImportacion ITransporte { get; set; }
    [Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Notify.ShowLoading(mensaje: "Obteniendo registro"); 

            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio);
            if (!IsAuthUser) return;
                  
            EsAsignadoEditar = await IPermiso.ConsultaEsAsignadoPorSesion(TransporteImportacionAcceso.Editar);
            EsAsignadoEliminar = await IPermiso.ConsultaEsAsignadoPorSesion(TransporteImportacionAcceso.Eliminar);

            if (!await IPermiso.ConsultaEsAsignadoPorSesion(TransporteImportacionAcceso.VerRegistros))
            {
                INavigation.NavigateTo("inicio");
                Notify.Show("No tiene permiso para ver registros del servicio principal de [Transportes]", "error");
                return;
            }

            TransporteObtener = await ITransporte.Obtener((Guid) Id);
            if (TransporteObtener is null)
            {
                INavigation.NavigateTo("transportes-importacion");
                Notify.Show($"El registro de la [Transporte] consultado a visualizar no está disponible", "error");
                return;
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

            await ITransporte.Eliminar((Guid) Id);
            IsLoadingAction = false;
            INavigation.NavigateTo("transportes-importacion");
            Notify.Show($"El transporte {TransporteObtener.Codigo.Trim()} ha sido eliminado con éxito", "success");
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

    public async Task Cerrar(LocationChangingContext context)
    {
        if (IsAuthUser && IsLoadingAction && !await Dialog.ConfirmAsync("¿Está seguro de salir del formulario sin haber culminado la acción?", "Saliendo del formulario"))
            context.PreventNavigation();
    }

    private void Volver() => INavigation.NavigateTo("transportes-importacion");

    private void IrEditar() => INavigation.NavigateTo(INavigation.GetUriWithQueryParameters($"transportes-importacion/{Id}/editar", new Dictionary<string, object> { ["returnpage"] = "view" }));

    public void Dispose() => GC.SuppressFinalize(this);
}