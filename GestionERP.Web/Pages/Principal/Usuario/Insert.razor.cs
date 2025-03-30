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

public partial class Insert : IDisposable
{
    #region Propiedades
    private const string codigoServicio = "S001";
    public FluentValidationValidator validator;
    public UsuarioInsertarDto UsuarioInsertar { get; set; }  
    public UsuarioObtenerDto UsuarioObtener { get; set; }
    public UsuarioModuloObtenerDto UsuarioModuloObtener { get; set; }
    public UsuarioServicioObtenerDto UsuarioServicioObtener { get; set; }
    public UsuarioPermisoObtenerDto UsuarioPermisoObtener { get; set; }
    public List<ModuloCatalogoDto> CatalogoModulos { get; set; }
    public List<ServicioCatalogoDto> CatalogoServicios { get; set; }
    public List<PermisoCatalogoDto> CatalogoPermisos { get; set; }

    public bool SelectModulosAllValue
    {
        get
        {
            return CatalogoModulos.All(x => x.EsAsignado);
        }
    }
    public bool SelectModulosIndeterminate
    {
        get
        {
            return CatalogoModulos.Any(x => x.EsAsignado) && !SelectModulosAllValue;
        }
    }

    public bool SelectServiciosAllValue
    {
        get
        {
            return CatalogoServicios.All(x => x.EsAsignado);
        }
    }
    public bool SelectServiciosIndeterminate
    {
        get
        {
            return CatalogoServicios.Any(x => x.EsAsignado) && !SelectServiciosAllValue;
        }
    }

    public bool SelectPermisosAllValue
    {
        get
        {
            return CatalogoPermisos.All(x => x.EsAsignado);
        }
    }
    public bool SelectPermisosIndeterminate
    {
        get
        {
            return CatalogoPermisos.Any(x => x.EsAsignado) && !SelectPermisosAllValue;
        }
    }

    public TelerikNotification Alert { get; set; }
    private IEnumerable<UsuarioTipoPerfilType> TiposPerfil { get; set; } 
    private EditContext EditContext { get; set; } 
    private bool IsLoadingAction { get; set; }
    private bool IsAuthUser { get; set; }
    private bool IsModified { get; set; }
    private bool EsVisibleVolver { get; set; }
    private int IndexTabAccesoActivo { get; set; }  
    [CascadingParameter] public DialogFactory Dialog { get; set; }
    [Parameter][SupplyParameterFromQuery(Name = "returnpage")] public string ReturnPage { get; set; }
    private ClaimsPrincipal User { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }
    #endregion

    [Inject] public IPrincipalUsuario IUsuario { get; set; }
    [Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public IPrincipalModulo IModulo { get; set; }
    [Inject] public IPrincipalServicio IServicio { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }
    [Inject] public IMapper IMapper { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            UsuarioObtener = new();
            UsuarioModuloObtener = new();
            UsuarioServicioObtener = new();
            UsuarioPermisoObtener = new();
            UsuarioInsertar = new()
            {
                Modulos = [],
                Servicios = [],
                Permisos = []
            };

            CatalogoModulos = [];
            CatalogoServicios = [];
            CatalogoPermisos = []; 
            TiposPerfil = UsuarioTipoPerfilType.ObtenerTipos(); 

            EditContext = new EditContext(UsuarioInsertar);

            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio);
            if (!IsAuthUser) return;
                
            EsVisibleVolver = !string.IsNullOrEmpty(ReturnPage) && ReturnPage is "index";

            if (!await IPermiso.ConsultaEsAsignadoPorSesion(UsuarioAcceso.Insertar))
            {
                INavigation.NavigateTo("usuarios");
                Notify.Show("No tiene permiso para insertar registros de [Usuarios]", "error");
                return;
            }

            CatalogoModulos = (List<ModuloCatalogoDto>) await IModulo.Catalogo(flagTipoAcceso: "P") ?? [];
            CatalogoServicios = (List<ServicioCatalogoDto>) await IServicio.Catalogo() ?? [];
            CatalogoPermisos = (List<PermisoCatalogoDto>) await IPermiso.Catalogo() ?? [];
            IndexTabAccesoActivo = 0;
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

            AsignarAccesos();

            Guid id = await IUsuario.Insertar(UsuarioInsertar);
            
            IsModified = false;
            Notify.Show("El usuario ha sido insertado con éxito", "success");
            INavigation.NavigateTo($"usuarios/{id}");
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
        UsuarioInsertar.Modulos = [];
        UsuarioInsertar.Servicios = [];
        UsuarioInsertar.Permisos = [];

        foreach (ModuloCatalogoDto itemModulo in CatalogoModulos.Where(x => x.EsAsignado))
        {
            UsuarioInsertar.Modulos.Add(new UsuarioModuloInsertarDto() { CodigoModulo = itemModulo.CodigoModulo });

            foreach (ServicioCatalogoDto itemServicio in CatalogoServicios.Where(x => x.EsAsignado & x.CodigoModulo == itemModulo.CodigoModulo))
            {
                UsuarioInsertar.Servicios.Add(new UsuarioServicioInsertarDto() { CodigoServicio = itemServicio.CodigoServicio });

                foreach (PermisoCatalogoDto itemPermiso in CatalogoPermisos.Where(x => x.EsAsignado & x.CodigoServicio == itemServicio.CodigoServicio))
                {
                    UsuarioInsertar.Permisos.Add(new UsuarioPermisoInsertarDto() { CodigoPermiso = itemPermiso.CodigoPermiso });
                }
            }
        }
    }

    private async Task Cerrar(LocationChangingContext context)
    {
        if (IsAuthUser && IsModified && !await Dialog.ConfirmAsync("¿Está seguro de salir del formulario de insertar sin haber guardado?", "Saliendo del formulario"))
            context.PreventNavigation();
    }
     
    private void TabChangedHandler(int newIndex)
    { 
        switch (newIndex)
        {
            case 1: 
                foreach (ModuloCatalogoDto item in CatalogoModulos)
                {
                    if (item.EsAsignado & !item.EsBloqueado)
                    {
                        item.EsBloqueado = true;
                        CatalogoServicios.Where(x => x.CodigoModulo == item.CodigoModulo).ToList().ForEach(x => x.EsAsignadoModulo = true);
                    }
                    else if (!item.EsAsignado & item.EsBloqueado)
                    {
                        item.EsBloqueado = false;
                        CatalogoServicios.Where(x => x.CodigoModulo == item.CodigoModulo).ToList().ForEach(x => x.EsAsignadoModulo = false);

                        foreach (ServicioCatalogoDto itemServicio in CatalogoServicios.Where(x => x.CodigoModulo == item.CodigoModulo & x.EsAsignado)) 
                            CatalogoPermisos.Where(x => x.CodigoServicio == itemServicio.CodigoServicio).ToList().ForEach(x => x.EsAsignadoServicio = false); 
                    } 
                } 
                break;
            case 2:
                foreach (ServicioCatalogoDto item in CatalogoServicios)
                {
                    if (item.EsAsignado & !item.EsBloqueado)
                    {
                        item.EsBloqueado = true;
                        CatalogoPermisos.Where(x => x.CodigoServicio == item.CodigoServicio).ToList().ForEach(x => x.EsAsignadoServicio = true);
                    }
                    else if (!item.EsAsignado & item.EsBloqueado)
                    {
                        item.EsBloqueado = false;
                        CatalogoPermisos.Where(x => x.CodigoServicio == item.CodigoServicio).ToList().ForEach(x => x.EsAsignadoServicio = false);
                    }
                } 
                break;
        }
        IndexTabAccesoActivo = newIndex;
    }

    protected static void OnStateInitServiciosHandler(GridStateEventArgs<ServicioCatalogoDto> args)
    { 
        GridState<ServicioCatalogoDto> desiredState = new()
        {
            GroupDescriptors =
            [
                new()
                {
                    Member = nameof(ServicioCatalogoDto.NombreModulo),
                    MemberType = typeof(string)
                }
            ]
        }; 
        args.GridState = desiredState; 
    }

    protected static void OnStateInitPermisosHandler(GridStateEventArgs<PermisoCatalogoDto> args)
    {
        GridState<PermisoCatalogoDto> desiredState = new()
        {
            GroupDescriptors =
            [
                new()
                {
                    Member = nameof(PermisoCatalogoDto.NombreModulo),
                    MemberType = typeof(string)
                },
                new()
                {
                    Member = nameof(PermisoCatalogoDto.NombreServicio),
                    MemberType = typeof(string)
                }
            ]
        };
        args.GridState = desiredState;
    }

    private void ValueModulosChanged(bool value) => CatalogoModulos.ForEach(x => x.EsAsignado = value);

    private void ValueServiciosChanged(bool value) => CatalogoServicios.ForEach(x => x.EsAsignado = value);

    private void ValuePermisosChanged(bool value)
    {
        CatalogoPermisos.ForEach(x => x.EsAsignado = value);
    }

    private void CargarItemCatalogoTipoIdentificacion(TipoIdentificacionCatalogoDto item)
    {
        UsuarioInsertar.CodigoTipoIdentificacion = item.CodigoTipoIdentificacion;
        UsuarioObtener.SiglaTipoIdentificacion = item.SiglaTipoIdentificacion;
        UsuarioObtener.NombreTipoIdentificacion = item.NombreTipoIdentificacion;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoTipoIdentificacion"));
        IsModified = true;
    }
     

    private void Volver() => INavigation.NavigateTo("usuarios");

    public void Dispose() => GC.SuppressFinalize(this);
}

