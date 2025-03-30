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
using Telerik.SvgIcons;
using GestionERP.Web.Global;

namespace GestionERP.Web.Pages.Principal.Entidad;

public partial class Insert : IDisposable
{
    #region Propiedades
    private const string codigoServicio = "S002";
    public FluentValidationValidator validator;
    public EntidadInsertarDto EntidadInsertar { get; set; }
    public EntidadDireccionInsertarDto DireccionInsertar { get; set; }
    public EntidadRepresentanteInsertarDto RepresentanteInsertar { get; set; }
    public EntidadVehiculoInsertarDto VehiculoInsertar { get; set; }
    public List<EntidadDireccionObtenerDto> ListaDirecciones { get; set; }
    public List<EntidadRepresentanteObtenerDto> ListaRepresentantes { get; set; }
    public List<EntidadVehiculoObtenerDto> ListaVehiculos { get; set; }
    public EntidadObtenerDto EntidadObtener { get; set; }
    public EntidadDireccionObtenerDto DireccionObtener { get; set; }
    public EntidadDireccionObtenerEsPredeterminadoDto DireccionPredeterminadoObtener { get; set; }
    public EntidadRepresentanteObtenerDto RepresentanteObtener { get; set; }
    public EntidadVehiculoObtenerDto VehiculoObtener { get; set; }
    public bool EsVisibleListaDireccion { get; set; }
    public bool EsVisibleListaVehiculo { get; set; }
    public bool EsVisibleModalDireccion { get; set; }
    public bool EsVisibleModalRepresentante { get; set; }
    public bool EsVisibleModalVehiculo { get; set; }
    public TelerikNotification AlertDireccion { get; set; }
    public TelerikNotification AlertRepresentante { get; set; }
    public TelerikNotification AlertVehiculo { get; set; }
    public TelerikNotification Alert { get; set; }
    public string VerboAccionModal { get; set; }
    public string DescripcionAccionModal { get; set; }
    public string TipoAccionModal { get; set; }
    public string TipoAccionDialog { get; set; }
    public string OrigenDialog { get; set; }
    public string OrigenModal { get; set; }
    public bool EsVisibleTabFichaSunat { get; set; }
    public bool EsVisibleDialogDireccion { get; set; }
    public bool EsVisibleDialogTelefono { get; set; }
    public bool EsVisibleDialogRepresentante { get; set; }
    public bool EsVisibleDialogVehiculo { get; set; }
    private bool EsVisibleVolver { get; set; }
    public string CodigoItemAccion { get; set; }
    public ISvgIcon IconoAccionModal { get; set; }
    private IEnumerable<EntidadTipoPersonaType> TiposPersona { get; set; }
    private IEnumerable<EntidadFichaSunatCondicionContribuyenteType> CondicionesContribuyente { get; set; } 
    private string FlagTipoOrigen { get; set; }
    private string FlagTipoPersona
    {
        get
        {
            return EntidadInsertar.FlagTipoPersona;
        }
        set
        {
            EntidadInsertar.FlagTipoPersona = value;
            ObtenerTipoOrigenPorPersona(value);
        }
    }
    private TelerikGrid<EntidadDireccionObtenerDto> GridDireccionRef { get; set; }
    private TelerikGrid<EntidadRepresentanteObtenerDto> GridRepresentanteRef { get; set; }
    private TelerikGrid<EntidadVehiculoObtenerDto> GridVehiculoRef { get; set; }
    private EditContext EditContext { get; set; }
    private EditContext EditContextDireccion { get; set; }
    private EditContext EditContextRepresentante { get; set; }
    private EditContext EditContextVehiculo { get; set; }
    private bool IsLoadingAction { get; set; }
    private bool IsAuthUser { get; set; }
    private bool IsModified { get; set; }
    private bool IsModifiedDireccion { get; set; }  
    private bool IsModifiedRepresentante { get; set; }
    private bool IsModifiedVehiculo { get; set; }
    [CascadingParameter] public DialogFactory Dialog { get; set; }
    [Parameter][SupplyParameterFromQuery(Name = "returnpage")] public string ReturnPage { get; set; }
    private ClaimsPrincipal User { get; set; }
    [CascadingParameter] public NotifyComponent Notify { get; set; }
    #endregion

    [Inject] public IPrincipalEntidad IEntidad { get; set; }
    [Inject] public IPrincipalPermiso IPermiso { get; set; }
    [Inject] public UserService IUser { get; set; }
    [Inject] public NavigationManager INavigation { get; set; }
    [Inject] public IMapper IMapper { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        { 
            EntidadObtener = new();

            EntidadInsertar = new()
            {
                FichaSunat = new(),
                Direcciones = [],
                Representantes = [],
                Vehiculos = []
            };

            DireccionInsertar = new();
            RepresentanteInsertar = new();
            VehiculoInsertar = new();
            ListaDirecciones = [];
            ListaRepresentantes = [];
            ListaVehiculos = [];
            DireccionObtener = new();
            DireccionPredeterminadoObtener = new();
            RepresentanteObtener = new();
            VehiculoObtener = new();

            TiposPersona = EntidadTipoPersonaType.ObtenerTipos(); 
            CondicionesContribuyente = EntidadFichaSunatCondicionContribuyenteType.ObtenerTipos();

            EditContext = new EditContext(EntidadInsertar);

            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio);
            if (!IsAuthUser) return;
                
            EsVisibleVolver = !string.IsNullOrEmpty(ReturnPage) && ReturnPage is "index";

            if (!await IPermiso.ConsultaEsAsignadoPorSesion(EntidadAcceso.Insertar))
            {
                INavigation.NavigateTo("entidades");
                Notify.Show("No tiene permiso para insertar registros de [Entidades]", "error");
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

            Guid id = await IEntidad.Insertar(EntidadInsertar);
            
            IsModified = false;
            Notify.Show("La entidad ha sido insertada con éxito", "success");
            INavigation.NavigateTo($"entidades/{id}");
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

    private void IniciarAccionModal(string tipoAccion, string origenModal)
    {
        TipoAccionModal = tipoAccion;
        OrigenModal = origenModal;
        switch (tipoAccion)
        {
            case "I":
                IconoAccionModal = SvgIcon.TableAdd;
                VerboAccionModal = "Agregar";
                DescripcionAccionModal = "Agregando";
                break;
            case "M":
                IconoAccionModal = SvgIcon.TableCellProperties;
                VerboAccionModal = "Modificar";
                DescripcionAccionModal = "Modificando";
                break;
            case "V":
                DescripcionAccionModal = "Visualizando";
                break;
        }

        switch (origenModal)
        { 
            case "direccion":
                if (tipoAccion is "I" or "M") 
                    EditContextDireccion = new EditContext(DireccionInsertar);  
                EsVisibleModalDireccion = true;
                break;
            case "representante":
                if (tipoAccion is "I" or "M") 
                    EditContextRepresentante = new EditContext(RepresentanteInsertar);  
                EsVisibleModalRepresentante = true;
                break;
            case "vehiculo":
                if (tipoAccion is "I" or "M")
                    EditContextVehiculo = new EditContext(VehiculoInsertar);
                EsVisibleModalVehiculo = true;
                break;
        }
    }

    private void IniciarAccionDialog(string tipoAccion, string origenDialog)
    {
        TipoAccionDialog = tipoAccion;
        OrigenDialog = origenDialog; 
        switch (origenDialog)
        { 
            case "direccion":
                EsVisibleDialogDireccion = true;
                break;
            case "representante":
                EsVisibleDialogRepresentante = true;
                break;
            case "vehiculo":
                EsVisibleDialogVehiculo = true;
                break;
        }
        if(origenDialog is "direccion")
            EsVisibleDialogDireccion = true;
        else if(origenDialog is "representante")
            EsVisibleDialogRepresentante = true;
        else if(origenDialog is "vehiculo")
            EsVisibleDialogVehiculo = true;
    }

    public void CerrarModal()
    {
        if (OrigenModal is "direccion")
            IsModifiedDireccion = EsVisibleModalDireccion = false;
        else if (OrigenModal is "representante")
            IsModifiedRepresentante = EsVisibleModalRepresentante = false;
        else if (OrigenModal is "vehiculo")
            IsModifiedVehiculo = EsVisibleModalVehiculo = false;  
        VerboAccionModal = DescripcionAccionModal = TipoAccionModal = CodigoItemAccion = "";
        StateHasChanged();
    }

    private void Volver() => INavigation.NavigateTo("entidades");

    public void CerrarDialog()
    {
        if (OrigenDialog is "direccion")
            EsVisibleDialogDireccion = false;
        else if (OrigenDialog is "representante")
            EsVisibleDialogRepresentante = false;
        else if (OrigenDialog is "vehiculo")
            EsVisibleDialogVehiculo = false;
        TipoAccionDialog = OrigenDialog = "";
        StateHasChanged();
    }

    private void ObtenerTipoOrigenPorPersona(string flagTipoPersona)
    {
        FlagTipoOrigen = TiposPersona.Where(x => x.Codigo == (flagTipoPersona ?? "")).Select(x => x.Origen).FirstOrDefault();

        EntidadInsertar.CodigoTipoIdentificacion = null;
        EntidadObtener.SiglaTipoIdentificacion = "";
        EntidadObtener.NombreTipoIdentificacion = "";

        IsModified = true;
    }

    #region Direccion
    private void VisibleListaDireccionChangedHandler(bool esVisible) => EsVisibleListaDireccion = esVisible;

    private async Task CerrarDireccion()
    {
        if (TipoAccionModal is not "V" && IsModifiedDireccion && !await Dialog.ConfirmAsync("¿Está seguro de cerrar el formulario y que los datos no se carguen?", "Saliendo del formulario"))
            return;
        CerrarModal();
    }

    public void MostrarAgregarDireccion()
    {
        int numeroItem = (int)(ListaDirecciones.Count > 0 ? ListaDirecciones.Select(x => x.NumeroItem).Max() + 1 : 1);
        DireccionInsertar = new() 
        { 
            NumeroItem = numeroItem
        };
        DireccionObtener = new();
        IniciarAccionModal("I", "direccion");
    }

    public void MostrarModificarDireccion(EntidadDireccionObtenerDto item)
    {
        DireccionInsertar = IMapper.Map<EntidadDireccionInsertarDto>(item);
        DireccionObtener = item;
        CodigoItemAccion = item.NumeroItem.ToString();
        IniciarAccionModal("M", "direccion");
    }

    private void MostrarQuitarDireccion(string codigoItem)
    {
        CodigoItemAccion = codigoItem;
        IniciarAccionDialog("Q", "direccion");
    }

    public void VerItemDireccion(EntidadDireccionObtenerDto item)
    {
        DireccionObtener = item;
        CodigoItemAccion = item.NumeroItem.ToString();
        IniciarAccionModal("V", "direccion");
    }

    private bool ValidarDirecciones()
    {
        foreach (EntidadDireccionObtenerDto item in ListaDirecciones.Where(x => x.NumeroItem.ToString() != CodigoItemAccion))
        {
            if (item.Nombre.Trim() == DireccionInsertar.Nombre.Trim())
            {
                Fnc.MostrarAlerta(AlertDireccion, "El valor del campo [Nombre] ya existe en el listado de Direcciones", "warning");
                return false;
            }
        }
        return true;
    }

    private void ModificarDireccionPredeterminada()
    {
        if (ListaDirecciones.Count > 0)
        {
            ListaDirecciones.Where(x => x.EsPredeterminado && x.NumeroItem.ToString() != CodigoItemAccion).ToList().ForEach(x => x.EsPredeterminado = false);
        
            if (EntidadInsertar.Direcciones.Count > 0)
                EntidadInsertar.Direcciones.Where(x => x.EsPredeterminado && x.NumeroItem.ToString() != CodigoItemAccion).ToList().ForEach(x => x.EsPredeterminado = false);

            EntidadDireccionObtenerDto predeterminada = ListaDirecciones.Where(x => x.EsPredeterminado).FirstOrDefault();
            if (predeterminada is not null)
            {
                DireccionPredeterminadoObtener = new()
                {
                    CodigoRegion = predeterminada.CodigoRegion,
                    NombreRegion = predeterminada.NombreRegion,
                    CodigoProvincia = predeterminada.CodigoProvincia,
                    NombreProvincia = predeterminada.NombreProvincia,
                    CodigoDistrito = predeterminada.CodigoDistrito,
                    NombreDistrito = predeterminada.NombreDistrito,
                    Nombre = predeterminada.Nombre
                };
            }
            else
            {
                DireccionPredeterminadoObtener = new();        
            }
        }
    }

    private async Task AccionarDireccion(string tipoAccion)
    {
        GridState<EntidadDireccionObtenerDto> direccionState = GridDireccionRef.GetState();
        bool esModificablePredeterminada;
        int index = ListaDirecciones.FindIndex(i => i.NumeroItem.ToString() == CodigoItemAccion);

        switch (tipoAccion)
        {
            case "I" or "M":
                if (ValidarDirecciones())
                    return;
                esModificablePredeterminada = (!DireccionObtener.EsPredeterminado & DireccionInsertar.EsPredeterminado) || (DireccionObtener.EsPredeterminado & !DireccionInsertar.EsPredeterminado);
                IMapper.Map(DireccionInsertar, DireccionObtener);
                if (tipoAccion == "I")
                {
                    EntidadInsertar.Direcciones.Add(DireccionInsertar);
                    ListaDirecciones.Add(DireccionObtener);
                    direccionState.InsertedItem = DireccionObtener;
                    CodigoItemAccion = DireccionObtener.NumeroItem.ToString();
                }
                else
                {
                    EntidadInsertar.Direcciones[index] = DireccionInsertar;
                    ListaDirecciones[index] = DireccionObtener;
                }
                if (esModificablePredeterminada)
                    ModificarDireccionPredeterminada();
                break;
            case "Q":
                if (EntidadInsertar.Direcciones[index].EsPredeterminado) 
                    DireccionPredeterminadoObtener = new();
                EntidadInsertar.Direcciones.RemoveAt(index);
                ListaDirecciones.RemoveAt(index);
                CerrarDialog();
                break;
        };
        CerrarModal();
        IsModified = true;
        await GridDireccionRef.SetStateAsync(direccionState);
    }
    #endregion

    #region Representante
    private async Task CerrarRepresentante()
    {
        if (TipoAccionModal is not "V" && IsModifiedRepresentante && !await Dialog.ConfirmAsync("¿Está seguro de cerrar el formulario y que los datos no se carguen?", "Saliendo del formulario"))
            return;
        CerrarModal();
    }

    public void MostrarAgregarRepresentante()
    {
        RepresentanteInsertar = new();
        RepresentanteObtener = new();
        IniciarAccionModal("I", "representante");
    }

    public void MostrarModificarRepresentante(EntidadRepresentanteObtenerDto item)
    {
        RepresentanteInsertar = IMapper.Map<EntidadRepresentanteInsertarDto>(item); 
        RepresentanteObtener = item; 
        CodigoItemAccion = item.NumeroTipoIdentificacion.Trim();
        IniciarAccionModal("M", "representante");
    }

    private void MostrarQuitarRepresentante(string codigoItem)
    {
        CodigoItemAccion = codigoItem;
        IniciarAccionDialog("Q", "representante");
    }

    public void VerItemRepresentante(EntidadRepresentanteObtenerDto item)
    {
        RepresentanteObtener = item;
        CodigoItemAccion = item.NumeroTipoIdentificacion.Trim();
        IniciarAccionModal("V", "representante");
    }

    private bool ValidarRepresentantes()
    {
        foreach (EntidadRepresentanteInsertarDto item in EntidadInsertar.Representantes.Where(x => x.NumeroTipoIdentificacion.Trim() != CodigoItemAccion))
        {
            if (item.NumeroTipoIdentificacion.Trim() == RepresentanteInsertar.NumeroTipoIdentificacion.Trim())
            {
                Fnc.MostrarAlerta(AlertRepresentante, "El valor del campo [Número de identidad] ya existe en el listado de representantes", "warning");
                return false;
            }
        }
        return true;
    }

    private async Task AccionarRepresentante(string tipoAccion)
    {
        GridState<EntidadRepresentanteObtenerDto> representanteState = GridRepresentanteRef.GetState();
        int index = ListaRepresentantes.FindIndex(i => i.NumeroTipoIdentificacion.Trim() == CodigoItemAccion);

        switch (tipoAccion)
        {
            case "I" or "M":
                if (ValidarRepresentantes())
                    return;
                IMapper.Map(RepresentanteInsertar, RepresentanteObtener);
                if (tipoAccion == "I")
                {
                    EntidadInsertar.Representantes.Add(RepresentanteInsertar);
                    ListaRepresentantes.Add(RepresentanteObtener);
                    representanteState.InsertedItem = RepresentanteObtener;
                }
                else
                {
                    EntidadInsertar.Representantes[index] = RepresentanteInsertar;
                    ListaRepresentantes[index] = RepresentanteObtener;
                }
                break;
            case "Q":
                EntidadInsertar.Representantes.RemoveAt(index);
                ListaRepresentantes.RemoveAt(index);
                CerrarDialog();
                break;
        };
        CerrarModal();
        IsModified = true;
        await GridRepresentanteRef.SetStateAsync(representanteState);
    }
    #endregion

    #region Vehiculo
    private void VisibleListaVehiculoChangedHandler(bool esVisible) => EsVisibleListaVehiculo = esVisible;

    private async Task CerrarVehiculo()
    {
        if (TipoAccionModal is not "V" && IsModifiedVehiculo && !await Dialog.ConfirmAsync("¿Está seguro de cerrar el formulario y que los datos no se carguen?", "Saliendo del formulario"))
            return;
        CerrarModal();
    }

    public void MostrarAgregarVehiculo()
    {
        VehiculoInsertar = new();
        VehiculoObtener = new();
        IniciarAccionModal("I", "vehiculo");
    }

    public void MostrarModificarVehiculo(EntidadVehiculoObtenerDto item)
    {
        VehiculoInsertar = IMapper.Map<EntidadVehiculoInsertarDto>(item);
        VehiculoObtener = item;
        CodigoItemAccion = item.CodigoVehiculo.Trim();
        IniciarAccionModal("M", "vehiculo");
    }

    private void MostrarQuitarVehiculo(string codigoItem)
    {
        CodigoItemAccion = codigoItem;
        IniciarAccionDialog("Q", "vehiculo");
    }

    public void VerItemVehiculo(EntidadVehiculoObtenerDto item)
    {
        VehiculoObtener = item;
        CodigoItemAccion = item.CodigoVehiculo.Trim();
        IniciarAccionModal("V", "vehiculo");
    }

    private bool ValidarVehiculos()
    {
        foreach (EntidadVehiculoInsertarDto item in EntidadInsertar.Vehiculos.Where(x => x.CodigoVehiculo.Trim() != CodigoItemAccion))
        {
            if (item.CodigoVehiculo.Trim() == VehiculoInsertar.CodigoVehiculo.Trim())
            {
                Fnc.MostrarAlerta(AlertVehiculo, "El valor del campo [Código] ya existe en el listado de vehículos", "warning");
                return false;
            }
        }
        return true;
    }

    private async Task AccionarVehiculo(string tipoAccion)
    {
        GridState<EntidadVehiculoObtenerDto> vehiculoState = GridVehiculoRef.GetState();
        int index = ListaVehiculos.FindIndex(i => i.CodigoVehiculo.Trim() == CodigoItemAccion);

        switch (tipoAccion)
        {
            case "I" or "M":
                if (ValidarVehiculos())
                    return;
                IMapper.Map(VehiculoInsertar, VehiculoObtener);
                if (tipoAccion == "I")
                {
                    EntidadInsertar.Vehiculos.Add(VehiculoInsertar);
                    ListaVehiculos.Add(VehiculoObtener);
                    vehiculoState.InsertedItem = VehiculoObtener;
                }
                else
                {
                    EntidadInsertar.Vehiculos[index] = VehiculoInsertar;
                    ListaVehiculos[index] = VehiculoObtener;
                }
                break;
            case "Q":
                EntidadInsertar.Vehiculos.RemoveAt(index);
                ListaVehiculos.RemoveAt(index);
                CerrarDialog();
                break;
        };
        CerrarModal();
        IsModified = true;
        await GridVehiculoRef.SetStateAsync(vehiculoState);
    }
    #endregion

    #region Catalogos
    private void CargarItemCatalogoTipoIdentificacionPorTipoOrigen(TipoIdentificacionCatalogoPorTipoOrigenDto item)
    {
        EntidadInsertar.CodigoTipoIdentificacion = item.CodigoTipoIdentificacion;
        EntidadObtener.SiglaTipoIdentificacion = item.SiglaTipoIdentificacion;
        EntidadObtener.NombreTipoIdentificacion = item.NombreTipoIdentificacion;

        if (FlagTipoOrigen == "DO" & item.ManejaFichaSunat)
        {
            EsVisibleTabFichaSunat = true;
            EntidadInsertar.FichaSunat = new();
        }
        else
        {
            EsVisibleTabFichaSunat = false;
            EntidadInsertar.FichaSunat = null;
        }

        IsModified = true;
    }
      
    private void CargarItemCatalogoPais(PaisCatalogoDto item)
    {
        EntidadInsertar.CodigoPais = item.CodigoPais;
        EntidadObtener.NombrePais = item.NombrePais;
        EditContext.NotifyFieldChanged(EditContext.Field("CodigoPais"));
        IsModified = true;
    }

    private void CargarItemCatalogoRegion(RegionCatalogoDto item)
    {
        if (item.CodigoRegion.Trim() != DireccionObtener.CodigoRegion?.Trim())
        {
            DireccionObtener.CodigoProvincia = DireccionObtener.CodigoDistrito = null;
            DireccionObtener.NombreProvincia = DireccionObtener.NombreDistrito = "";
        }
        DireccionObtener.CodigoRegion = item.CodigoRegion;
        DireccionObtener.NombreRegion = item.NombreRegion;
        IsModifiedDireccion = true;
    }

    private void CargarItemCatalogoProvinciaPorRegion(ProvinciaCatalogoPorRegionDto item)
    {
        if (item.CodigoProvincia.Trim() != DireccionObtener.CodigoProvincia?.Trim())
        {
            DireccionInsertar.CodigoDistrito = null;
            DireccionObtener.NombreDistrito = "";
        }
        DireccionObtener.CodigoProvincia = item.CodigoProvincia;
        DireccionObtener.NombreProvincia = item.NombreProvincia;
        IsModifiedDireccion = true;
    }

    private void CargarItemCatalogoDistritoPorProvincia(DistritoCatalogoPorProvinciaDto item)
    {
        DireccionInsertar.CodigoDistrito = item.CodigoDistrito;
        DireccionObtener.NombreDistrito = item.NombreDistrito;
        EditContextDireccion.NotifyFieldChanged(EditContextDireccion.Field("CodigoDistrito"));
        IsModifiedDireccion = true;
    }

    private void CargarItemCatalogoTipoIdentificacion(TipoIdentificacionCatalogoDto item)
    {
        RepresentanteInsertar.CodigoTipoIdentificacion = item.CodigoTipoIdentificacion;
        RepresentanteObtener.SiglaTipoIdentificacion = item.SiglaTipoIdentificacion;
        RepresentanteObtener.NombreTipoIdentificacion = item.NombreTipoIdentificacion;
        EditContextRepresentante.NotifyFieldChanged(EditContextRepresentante.Field("CodigoTipoIdentificacion"));
        IsModifiedRepresentante = true;
    }

    private void CargarItemCatalogoVehiculo(VehiculoCatalogoDto item)
    {
        VehiculoInsertar.CodigoVehiculo = item.CodigoVehiculo;
        VehiculoObtener.MarcaVehiculo = item.MarcaVehiculo;
        EditContextVehiculo.NotifyFieldChanged(EditContextVehiculo.Field("CodigoVehiculo"));
        IsModifiedVehiculo = true;
    }
    #endregion

    public void Dispose() => GC.SuppressFinalize(this);
}