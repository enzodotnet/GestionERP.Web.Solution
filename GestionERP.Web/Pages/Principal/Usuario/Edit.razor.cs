using AutoMapper;
using Blazored.FluentValidation;
using GestionERP.Web.Handlers;
using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Models.Dtos.Principal.Types;
using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Components;
using Microsoft.AspNetCore.Components;
using GestionERP.Web.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Telerik.Blazor;
using Telerik.Blazor.Components;
using Telerik.DataSource;
using GestionERP.Web.Global;

namespace GestionERP.Web.Pages.Principal.Usuario;

public partial class Edit : IDisposable
{
    #region Propiedades
    private const string codigoServicio = "S001";
    public FluentValidationValidator validator;
    public UsuarioEditarDto UsuarioEditar { get; set; }
    public UsuarioObtenerDto UsuarioObtener { get; set; }
    public UsuarioModuloObtenerDto UsuarioModuloObtener { get; set; }
    public UsuarioServicioObtenerDto UsuarioServicioObtener { get; set; }
    public UsuarioPermisoObtenerDto UsuarioPermisoObtener { get; set; }
    public TelerikNotification Alert { get; set; }
    private IEnumerable<UsuarioTipoPerfilType> TiposPerfil { get; set; }  
    private EditContext EditContext { get; set; }
    private bool IsLoadingAction { get; set; }
    private bool IsAuthUser { get; set; }
    private bool IsModified { get; set; }
    private bool EsVisibleVolver { get; set; }
    private bool IsInitPage { get; set; }
    private int IndexTabAccesoActivo { get; set; }

    public bool SelectModulosAllValue
    {
        get
        {
            return UsuarioObtener.Modulos.All(x => x.EsAsignado);
        }
    } 
    public bool SelectModulosIndeterminate
    {
        get
        {
            return UsuarioObtener.Modulos.Any(x => x.EsAsignado) && !SelectModulosAllValue;
        }
    }

    public bool SelectServiciosAllValue
    {
        get
        {
            return UsuarioObtener.Servicios.All(x => x.EsAsignado);
        }
    }
    public bool SelectServiciosIndeterminate
    {
        get
        {
            return UsuarioObtener.Servicios.Any(x => x.EsAsignado) && !SelectServiciosAllValue;
        }
    }

    public bool SelectPermisosAllValue
    {
        get
        {
            return UsuarioObtener.Permisos.All(x => x.EsAsignado);
        }
    }
    public bool SelectPermisosIndeterminate
    {
        get
        {
            return UsuarioObtener.Permisos.Any(x => x.EsAsignado) && !SelectPermisosAllValue;
        }
    }

    [CascadingParameter] public DialogFactory Dialog { get; set; }
    [Parameter] public Guid? Id { get; set; }
    [Parameter][SupplyParameterFromQuery(Name = "returnpage")] public string ReturnPage { get; set; }
    private ClaimsPrincipal User { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }
    #endregion

    [Inject] public IPrincipalUsuario IUsuario { get; set; }
    [Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }
    [Inject] public IMapper IMapper { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Notify.ShowLoading(mensaje: "Obteniendo registro");

            UsuarioModuloObtener = new();
            UsuarioServicioObtener = new();
            UsuarioPermisoObtener = new();
            UsuarioEditar = new(); 

            TiposPerfil = UsuarioTipoPerfilType.ObtenerTipos();

            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio);
            if (!IsAuthUser) return;
                
            EsVisibleVolver = !string.IsNullOrEmpty(ReturnPage) && ReturnPage is "index" or "view";
                
            if (!await IPermiso.ConsultaEsAsignadoPorSesion(UsuarioAcceso.Editar))
            {
                INavigation.NavigateTo("usuarios");
                Notify.Show("No tiene permiso para editar registros de [Usuarios]", "error");
                return;
            }

            UsuarioObtener = await IUsuario.Obtener((Guid) Id);

            if (UsuarioObtener is null)
            {
                INavigation.NavigateTo("usuarios");
                Notify.Show("El registro del [Usuario] consultado a editar no existe", "error");
                return;
            }

            UsuarioEditar = IMapper.Map<UsuarioEditarDto>(UsuarioObtener);

            UsuarioObtener.Modulos.Where(x => x.EsAsignado).ToList().ForEach(x => x.EsAsignadoTemp = x.EsBloqueado = true);
            UsuarioObtener.Servicios.Where(x => x.EsAsignado).ToList().ForEach(x => x.EsAsignadoTemp = x.EsBloqueado = true);
            UsuarioObtener.Permisos.Where(x => x.EsAsignado).ToList().ForEach(x => x.EsAsignadoTemp = x.EsBloqueado = true);

            EditContext = new EditContext(UsuarioEditar);
            IndexTabAccesoActivo = 0;
            IsInitPage = true;
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

    private async Task Editar()
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
             
            Notify.ShowLoading(mensaje: "Actualización en progreso");
 
            AsignarAccesos();

            await IUsuario.Editar((Guid) Id, UsuarioEditar);

            IsModified = false;
            Notify.Show("El usuario ha sido editado con éxito", "success");
            INavigation.NavigateTo($"usuarios/{Id}");
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

    private void AsignarAccesos()
    {
        UsuarioEditar.ModulosInsertar = [];
        UsuarioEditar.ServiciosInsertar = [];
        UsuarioEditar.PermisosInsertar = [];
        UsuarioEditar.ModulosEditar = [];
        UsuarioEditar.ServiciosEditar = [];
        UsuarioEditar.PermisosEditar = [];

        foreach (UsuarioModuloObtenerDto itemModulo in UsuarioObtener.Modulos)
        {
            if (!itemModulo.Id.HasValue & itemModulo.EsAsignado)
            {
                UsuarioEditar.ModulosInsertar.Add(new UsuarioModuloInsertarDto() { CodigoModulo = itemModulo.CodigoModulo });

                foreach (UsuarioServicioObtenerDto itemServicio in UsuarioObtener.Servicios.Where(x => x.EsAsignado & !x.Id.HasValue & x.CodigoModulo == itemModulo.CodigoModulo))
                {
                    UsuarioEditar.ServiciosInsertar.Add(new UsuarioServicioInsertarDto() { CodigoServicio = itemServicio.CodigoServicio });

                    foreach (UsuarioPermisoObtenerDto itemPermiso in UsuarioObtener.Permisos.Where(x => x.EsAsignado & !x.Id.HasValue & x.CodigoServicio == itemServicio.CodigoServicio))
                        UsuarioEditar.PermisosInsertar.Add(new UsuarioPermisoInsertarDto() { CodigoPermiso = itemPermiso.CodigoPermiso });
                }
            }
            else if (itemModulo.Id.HasValue)
            {
                if (itemModulo.EsAsignado != itemModulo.EsAsignadoTemp)
                    UsuarioEditar.ModulosEditar.Add(new UsuarioModuloEditarDto() { Id = (Guid) itemModulo.Id, EsAsignado = itemModulo.EsAsignado }); 

                if (!itemModulo.EsAsignado)
                {
                    foreach (UsuarioServicioObtenerDto itemServicio in UsuarioObtener.Servicios.Where(x => x.Id.HasValue & x.CodigoModulo == itemModulo.CodigoModulo))
                    {
                        UsuarioEditar.ServiciosEditar.Add(new UsuarioServicioEditarDto() { Id = (Guid) itemServicio.Id, EsAsignado = false });

                        foreach (UsuarioPermisoObtenerDto itemPermiso in UsuarioObtener.Permisos.Where(x => x.Id.HasValue & x.CodigoServicio == itemServicio.CodigoServicio))
                            UsuarioEditar.PermisosEditar.Add(new UsuarioPermisoEditarDto() { Id = (Guid) itemPermiso.Id, EsAsignado = false });
                    }
                }
                else
                {
                    foreach (UsuarioServicioObtenerDto itemServicio in UsuarioObtener.Servicios.Where(x => x.CodigoModulo == itemModulo.CodigoModulo))
                    {
                        if(!itemServicio.Id.HasValue & itemServicio.EsAsignado)
                        {
                            UsuarioEditar.ServiciosInsertar.Add(new UsuarioServicioInsertarDto() { CodigoServicio = itemServicio.CodigoServicio });

                            foreach (UsuarioPermisoObtenerDto itemPermiso in UsuarioObtener.Permisos.Where(x => x.EsAsignado & !x.Id.HasValue & x.CodigoServicio == itemServicio.CodigoServicio))
                                UsuarioEditar.PermisosInsertar.Add(new UsuarioPermisoInsertarDto() { CodigoPermiso = itemPermiso.CodigoPermiso });
                        }
                        else if(itemServicio.Id.HasValue)
                        {
                            if(itemServicio.EsAsignado != itemServicio.EsAsignadoTemp)
                                UsuarioEditar.ServiciosEditar.Add(new UsuarioServicioEditarDto() { Id = (Guid) itemServicio.Id, EsAsignado = itemServicio.EsAsignado }); 

                            if (!itemServicio.EsAsignado)
                            {
                                foreach (UsuarioPermisoObtenerDto itemPermiso in UsuarioObtener.Permisos.Where(x => x.Id.HasValue & x.CodigoServicio == itemServicio.CodigoServicio))
                                    UsuarioEditar.PermisosEditar.Add(new UsuarioPermisoEditarDto() { Id = (Guid) itemPermiso.Id, EsAsignado = false });
                            }
                            else
                            {
                                foreach (UsuarioPermisoObtenerDto itemPermiso in UsuarioObtener.Permisos.Where(x => x.CodigoServicio == itemServicio.CodigoServicio))
                                {
                                    if (itemPermiso.Id.HasValue & itemPermiso.EsAsignado != itemPermiso.EsAsignadoTemp)
                                        UsuarioEditar.PermisosEditar.Add(new UsuarioPermisoEditarDto() { Id = (Guid) itemPermiso.Id, EsAsignado = itemPermiso.EsAsignado });
                                    else if (!itemPermiso.Id.HasValue & itemPermiso.EsAsignado)
                                        UsuarioEditar.PermisosInsertar.Add(new UsuarioPermisoInsertarDto() { CodigoPermiso = itemPermiso.CodigoPermiso });
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void OnRowModulosRenderHandler(GridRowRenderEventArgs args)
    {
        UsuarioModuloObtenerDto item = args.Item as UsuarioModuloObtenerDto;

        if (!item.Id.HasValue & item.EsAsignado)
        {
            args.Class = "new-row";
            IsModified = true;
        }
        else if (item.Id.HasValue)
        {
            if (item.EsAsignado != item.EsAsignadoTemp)
            {
                if (item.EsAsignado) 
                    args.Class = "new-row"; 
                else 
                    args.Class = "delete-row";
                IsModified = true;
            }
        }
    }

    private void OnRowServiciosRenderHandler(GridRowRenderEventArgs args)
    {
        UsuarioServicioObtenerDto item = args.Item as UsuarioServicioObtenerDto;

        if (!item.Id.HasValue & item.EsAsignado)
        {
            args.Class = "new-row";
            IsModified = true;
        }
        else if (item.Id.HasValue)
        {
            if(item.EsAsignado != item.EsAsignadoTemp)
            {
                if(item.EsAsignado) 
                    args.Class = "new-row"; 
                else 
                    args.Class = "delete-row";
                IsModified = true; 
            }
        }
    }

    private void OnRowPermisosRenderHandler(GridRowRenderEventArgs args)
    {
        UsuarioPermisoObtenerDto item = args.Item as UsuarioPermisoObtenerDto;

        if (!item.Id.HasValue & item.EsAsignado)
        {
            args.Class = "new-row";
            IsModified = true;
        }
        else if (item.Id.HasValue)
        {
            if (item.EsAsignado != item.EsAsignadoTemp)
            {
                if (item.EsAsignado) 
                    args.Class = "new-row";
                else 
                    args.Class = "delete-row";
                IsModified = true;
            }
        }
    }

    private void ValueModulosChanged(bool value) => UsuarioObtener.Modulos.ForEach(x => x.EsAsignado = value);

    private void ValueServiciosChanged(bool value) => UsuarioObtener.Servicios.ForEach(x => x.EsAsignado = value);

    private void ValuePermisosChanged(bool value) => UsuarioObtener.Permisos.ForEach(x => x.EsAsignado = value);

    private void TabChangedHandler(int newIndex)
    {
        switch (newIndex)
        {
            case 1:
                foreach (UsuarioModuloObtenerDto item in UsuarioObtener.Modulos)
                {
                    if (item.EsAsignado & !item.EsBloqueado)
                    {
                        item.EsBloqueado = true;
                        UsuarioObtener.Servicios.Where(x => x.CodigoModulo == item.CodigoModulo).ToList().ForEach(x => x.EsAsignadoModulo = true);
                    }
                    else if (!item.EsAsignado & item.EsBloqueado)
                    {
                        item.EsBloqueado = false;
                        UsuarioObtener.Servicios.Where(x => x.CodigoModulo == item.CodigoModulo).ToList().ForEach(x => x.EsAsignadoModulo = false);

                        foreach (UsuarioServicioObtenerDto itemServicio in UsuarioObtener.Servicios.Where(x => x.CodigoModulo == item.CodigoModulo & x.EsAsignado))
                            UsuarioObtener.Permisos.Where(x => x.CodigoServicio == itemServicio.CodigoServicio).ToList().ForEach(x => x.EsAsignadoServicio = false);
                    }
                }
                break;
            case 2:
                foreach (UsuarioServicioObtenerDto item in UsuarioObtener.Servicios)
                {
                    if (item.EsAsignado & !item.EsBloqueado)
                    {
                        item.EsBloqueado = true;
                        UsuarioObtener.Permisos.Where(x => x.CodigoServicio == item.CodigoServicio).ToList().ForEach(x => x.EsAsignadoServicio = true);
                    }
                    else if (!item.EsAsignado & item.EsBloqueado)
                    {
                        item.EsBloqueado = false;
                        UsuarioObtener.Permisos.Where(x => x.CodigoServicio == item.CodigoServicio).ToList().ForEach(x => x.EsAsignadoServicio = false);
                    }
                }
                break;
        }
        IndexTabAccesoActivo = newIndex;
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

    private async Task Cerrar(LocationChangingContext context)
    {
        if (IsAuthUser && IsModified && !await Dialog.ConfirmAsync("¿Está seguro de salir del formulario de editar sin haber actualizado?", "Saliendo del formulario"))
            context.PreventNavigation();
    }


    private void Volver() => INavigation.NavigateTo($"usuarios{(ReturnPage == "view" ? $"/{Id}" : "")}");

    public void Dispose() => GC.SuppressFinalize(this);
}