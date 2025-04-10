﻿using Blazored.FluentValidation;     
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

namespace GestionERP.Web.Pages.Principal.TransporteImportacion;

public partial class Insert : IDisposable
{
    #region Propiedades
    private const string codigoServicio = "S036";
    public FluentValidationValidator validator;
    public TransporteImportacionInsertarDto TransporteInsertar { get; set; }
    public TransporteImportacionObtenerDto TransporteObtener { get; set; }
    public TelerikNotification Alert { get; set; }             
    private EditContext EditContext { get; set; }
    private bool IsLoadingAction { get; set; }
    private bool IsAuthUser { get; set; }
    private bool IsModified { get; set; }
    private bool EsVisibleVolver { get; set; }
    [CascadingParameter] public DialogFactory Dialog { get; set; }
    [Parameter][SupplyParameterFromQuery(Name = "returnpage")] public string ReturnPage { get; set; }
    private ClaimsPrincipal User { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }
    #endregion

    [Inject] public IPrincipalTransporteImportacion ITransporte { get; set; }
    [Inject] public IPrincipalMoneda IMoneda{ get; set; }
    [Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        { 
            TransporteObtener = new(); 
            TransporteInsertar = new();
             
            EditContext = new EditContext(TransporteInsertar);

            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio);
            if (!IsAuthUser) return;
                
            EsVisibleVolver = !string.IsNullOrEmpty(ReturnPage) && ReturnPage is "index";

            if (!await IPermiso.ConsultaEsAsignadoPorSesion(TransporteImportacionAcceso.Insertar))
            {
                INavigation.NavigateTo("transportes-importacion");
                Notify.Show("No tiene permiso para insertar registros de [Transportes]", "error");
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

            Guid id = await ITransporte.Insertar(TransporteInsertar);
             
            IsModified = false;
            Notify.Show("El transporte ha sido insertado con éxito", "success");
            INavigation.NavigateTo($"transportes-importacion/{id}");
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


    private void Volver() => INavigation.NavigateTo("transportes-importacion");

    public void Dispose() => GC.SuppressFinalize(this);
}