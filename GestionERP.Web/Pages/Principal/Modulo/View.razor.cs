﻿using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Models.Dtos.Principal.Types;
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Components;
using Microsoft.AspNetCore.Components;
using GestionERP.Web.Services;
using System.Security.Claims;
using Telerik.Blazor;
using Microsoft.AspNetCore.Components.Routing;

namespace GestionERP.Web.Pages.Principal.Modulo;

public partial class View : IDisposable
{
    private const string codigoServicio = "S029";
    public ModuloObtenerDto ModuloObtener { get; set; }
    private IEnumerable<ModuloTipoAccesoType> ModuloTiposAccesos { get; set; }
    private bool EsVisibleDialogEliminar { get; set; }
    private bool EsAsignadoEliminar { get; set; }
    private bool EsAsignadoEditar { get; set; }
    private bool EsAsignadoVerServicio { get; set; }
    private bool IsLoadingAction { get; set; }
    private bool IsAuthUser { get; set; }
    [Parameter] public Guid? Id { get; set; }
    private ClaimsPrincipal User { get; set; }
    [CascadingParameter] public DialogFactory Dialog { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }

    [Inject] public IPrincipalModulo IModulo { get; set; }
    [Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        { 
            Notify.ShowLoading(mensaje: "Obteniendo registro");

            ModuloTiposAccesos = ModuloTipoAccesoType.ObtenerTipos();

            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio);
            if (!IsAuthUser) return;
                
            EsAsignadoEditar = await IPermiso.ConsultaEsAsignadoPorSesion(ModuloAcceso.Editar);
            EsAsignadoEliminar = await IPermiso.ConsultaEsAsignadoPorSesion(ModuloAcceso.Eliminar);
            EsAsignadoVerServicio = await IPermiso.ConsultaEsAsignadoPorSesion(ServicioAcceso.VerRegistros);

            if (!await IPermiso.ConsultaEsAsignadoPorSesion(ModuloAcceso.VerRegistros))
            {
                INavigation.NavigateTo("inicio");
                Notify.Show("No tiene permiso para ver registros del servicio principal de [Módulos]", "error");
                return;
            }

            ModuloObtener = await IModulo.Obtener((Guid) Id);
            if (ModuloObtener is null)
            {
                INavigation.NavigateTo("modulos");
                Notify.Show("El registro del [Módulo] consultado a visualizar no está disponible", "error");
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
 
            await IModulo.Eliminar((Guid) Id);
            IsLoadingAction = false;
            INavigation.NavigateTo("modulos");
            Notify.Show($"El módulo {ModuloObtener.Codigo.Trim()} ha sido eliminado con éxito", "success");
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

    private void Volver() => INavigation.NavigateTo("modulos");

    private void IrEditar() => INavigation.NavigateTo(INavigation.GetUriWithQueryParameters($"modulos/{Id}/editar", new Dictionary<string, object> { ["returnpage"] = "view" }));

    private void IrVerServicios() => INavigation.NavigateTo(INavigation.GetUriWithQueryParameters("servicios", new Dictionary<string, object> { ["codigoModulo"] = ModuloObtener.Codigo.Trim() }));

    public void Dispose() => GC.SuppressFinalize(this);
}