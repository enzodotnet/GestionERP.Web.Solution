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

public partial class Edit : IDisposable
{
    #region Propiedades
    private const string codigoServicio = "S002";
    public FluentValidationValidator validator;
    public EntidadEditarDto EntidadEditar { get; set; }
    public EntidadDireccionInsertarDto DireccionInsertar { get; set; }
    public EntidadRepresentanteInsertarDto RepresentanteInsertar { get; set; }
    public EntidadVehiculoInsertarDto VehiculoInsertar { get; set; }
    public EntidadDireccionEditarDto DireccionEditar { get; set; }
    public EntidadRepresentanteEditarDto RepresentanteEditar { get; set; }
    public EntidadVehiculoEditarDto VehiculoEditar { get; set; }
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
    public ISvgIcon IconoAccionModal { get; set; }
    private IEnumerable<EntidadTipoPersonaType> TiposPersona { get; set; }
    private IEnumerable<EntidadFichaSunatCondicionContribuyenteType> CondicionesContribuyente { get; set; }
    private TelerikGrid<EntidadDireccionObtenerDto> GridDireccionRef { get; set; }
    private TelerikGrid<EntidadRepresentanteObtenerDto> GridRepresentanteRef { get; set; }
    private TelerikGrid<EntidadVehiculoObtenerDto> GridVehiculoRef { get; set; }
    private EditContext EditContext { get; set; }
    private EditContext EditContextDireccionInsertar { get; set; }
    private EditContext EditContextRepresentanteInsertar { get; set; }
    private EditContext EditContextVehiculoInsertar { get; set; }
    private EditContext EditContextDireccionEditar { get; set; }
    private EditContext EditContextRepresentanteEditar { get; set; }
    private EditContext EditContextVehiculoEditar { get; set; }
    private bool IsLoadingAction { get; set; }
    private bool IsAuthUser { get; set; }
    private bool IsModified { get; set; }
    private bool IsModifiedDireccion { get; set; }
    private bool IsModifiedRepresentante { get; set; }
    private bool IsModifiedVehiculo { get; set; }
    private bool EsVisibleVolver { get; set; }
    private bool IsInitPage { get; set; }
    [CascadingParameter] public DialogFactory Dialog { get; set; }
    [Parameter] public Guid? Id { get; set; }
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
            Notify.ShowLoading(mensaje: "Obteniendo registro");
            EntidadEditar = new();

            DireccionInsertar = new();
            RepresentanteInsertar = new();
            VehiculoInsertar = new();
            DireccionEditar = new();
            RepresentanteEditar = new();
            VehiculoEditar = new();
            DireccionObtener = new();
            DireccionPredeterminadoObtener = new();
            RepresentanteObtener = new();
            VehiculoObtener = new(); 

			TiposPersona = EntidadTipoPersonaType.ObtenerTipos();
            CondicionesContribuyente = EntidadFichaSunatCondicionContribuyenteType.ObtenerTipos();

            (IsAuthUser, User) = await IUser.VerificarAccesoEsValido(Notify, codigoServicio: codigoServicio);
            if (!IsAuthUser) return;
                
            EsVisibleVolver = !string.IsNullOrEmpty(ReturnPage) && ReturnPage is "index" or "view";

            if (!await IPermiso.ConsultaEsAsignadoPorSesion(EntidadAcceso.Editar))
            {
                INavigation.NavigateTo("entidades");
                Notify.Show("No tiene permiso para editar registros de [Entidades]", "error");
                return;
            }

            EntidadObtener = await IEntidad.Obtener((Guid) Id); 
            if (EntidadObtener is null)
            {
                INavigation.NavigateTo("entidades");
                Notify.Show($"El registro de la [Entidad] consultado a editar no existe", "error");
                return;
            }
            EntidadEditar = IMapper.Map<EntidadEditarDto>(EntidadObtener);
            EntidadEditar.FichaSunatEditar = IMapper.Map<EntidadFichaSunatEditarDto>(EntidadObtener.FichaSunat);

            EditContext = new EditContext(EntidadEditar);

            DireccionPredeterminadoObtener = await IEntidad.ObtenerDireccionEsPredeterminado(EntidadObtener.Codigo) ?? new();
            EsVisibleTabFichaSunat = EntidadObtener.FlagTipoPersona == "JU";
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

            await IEntidad.Editar((Guid) Id, EntidadEditar);
            
            IsModified = false;
            Notify.Show("La entidad ha sido editada con éxito", "success");
            INavigation.NavigateTo($"entidades/{Id}");
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
        if (IsAuthUser && IsModified && !await Dialog.ConfirmAsync("¿Está seguro de salir del formulario de editar sin haber actualizado?", "Saliendo del formulario"))
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
            case "M" or "E":
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
                    EditContextDireccionInsertar = new EditContext(DireccionInsertar);  
                else if (tipoAccion is "E") 
                    EditContextDireccionEditar = new EditContext(DireccionEditar);
                EsVisibleModalDireccion = true;
                break;
            case "representante":
                if (tipoAccion is "I" or "M") 
                    EditContextRepresentanteInsertar = new EditContext(RepresentanteInsertar);
                else if (tipoAccion is "E") 
                    EditContextRepresentanteEditar = new EditContext(RepresentanteEditar);
                EsVisibleModalRepresentante = true;
                break;
            case "vehiculo":
                if (tipoAccion is "I" or "M") 
                    EditContextVehiculoInsertar = new EditContext(VehiculoInsertar);
                else if (tipoAccion is "E") 
                    EditContextVehiculoEditar = new EditContext(VehiculoEditar);
                EsVisibleModalVehiculo = true;
                break;
        }
    }

    private void IniciarAccionDialog(string tipoAccion, string origenDialog)
    {
        TipoAccionDialog = tipoAccion;
        OrigenDialog = origenDialog; 
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
        VerboAccionModal = DescripcionAccionModal = TipoAccionModal = "";
        StateHasChanged();
    }

    private void Volver() => INavigation.NavigateTo($"entidades{(ReturnPage == "view" ? $"/{Id}" : "")}");

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

    #region Direccion
    private void VisibleListaDireccionChangedHandler(bool esVisible) => EsVisibleListaDireccion = esVisible;

    private async Task CerrarDireccion()
    {
        if (TipoAccionModal is not "V" && IsModifiedDireccion && !await Dialog.ConfirmAsync("¿Está seguro de cerrar el formulario y que los datos no se carguen?", "Saliendo del formulario"))
            return;
        CerrarModal();
    }

    public static void OnRowDireccionesRenderHandler(GridRowRenderEventArgs args) => args.Class = !(args.Item as EntidadDireccionObtenerDto).Id.HasValue ? "new-row" : "";
    
    public void MostrarAgregarDireccion()
    {
        int numeroItem = EntidadObtener.Direcciones.Count > 0 ? EntidadObtener.Direcciones.Select(x => x.NumeroItem).Max() + 1 : 1;
        DireccionInsertar = new()
        {
            NumeroItem = numeroItem
        };
        DireccionObtener = new()
        {
            Activo = true
        };
        IniciarAccionModal("I", "direccion");
    }

    public void MostrarModificarDireccion(EntidadDireccionObtenerDto item)
    {
        string tipoAccion = !item.Id.HasValue ? "M" : "E";
        if (tipoAccion is "M")
            DireccionEditar = IMapper.Map<EntidadDireccionEditarDto>(item); 
        else 
            DireccionInsertar = IMapper.Map<EntidadDireccionInsertarDto>(item);
        
        DireccionObtener = item;
        IniciarAccionModal(tipoAccion, "direccion");
    }

    private bool ValidarDirecciones()
    {
        foreach (EntidadDireccionObtenerDto item in EntidadObtener.Direcciones.Where(x => x.NumeroItem.ToString() != DireccionInsertar.NumeroItem.ToString()))
        {
            if (item.Nombre.Trim() == DireccionInsertar.Nombre.Trim())
            {
                Fnc.MostrarAlerta(AlertDireccion, "El valor del campo [Nombre] ya existe en el listado de Direcciones", "warning");
                return false;
            }
        }
        return true;
    }

    public void VerItemDireccion(EntidadDireccionObtenerDto item)
    {
        DireccionObtener = item; 
        IniciarAccionModal("V", "direccion");
    }

    private void MostrarQuitarDireccion(EntidadDireccionObtenerDto item)
    {
        DireccionObtener = item;
        IniciarAccionDialog(!item.Id.HasValue ? "Q" : "X", "direccion");
    }

    private void ModificarDireccionPredeterminada()
    {
        if (EntidadObtener.Direcciones.Count > 0)
        {
            EntidadObtener.Direcciones.Where(x => x.EsPredeterminado && x.NumeroItem != DireccionObtener.NumeroItem).ToList().ForEach(x => x.EsPredeterminado = false);
        
            if (EntidadEditar.DireccionesInsertar.Count > 0)
                EntidadEditar.DireccionesInsertar.Where(x => x.EsPredeterminado && x.NumeroItem != DireccionObtener.NumeroItem).ToList().ForEach(x => x.EsPredeterminado = false);

            if (EntidadEditar.DireccionesEditar.Count > 0)
                EntidadEditar.DireccionesEditar.Where(x => x.EsPredeterminado && x.NumeroItem != DireccionObtener.NumeroItem).ToList().ForEach(x => x.EsPredeterminado = false);

            EntidadDireccionObtenerDto predeterminada = EntidadObtener.Direcciones.Where(x => x.EsPredeterminado).FirstOrDefault();
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
        bool esModificablePredeterminada = false;
        int indexGrid = EntidadObtener.Direcciones.FindIndex(i => i.NumeroItem == DireccionObtener.NumeroItem);
        int indexEdit = tipoAccion is "E" or "X" ? EntidadEditar.DireccionesEditar.FindIndex(i => i.Id == DireccionObtener.Id) : EntidadEditar.DireccionesInsertar.FindIndex(i => i.NumeroItem == DireccionObtener.NumeroItem);
        
        switch (tipoAccion)
        {
            case "I" or "M":
                if (ValidarDirecciones())
                    return;
                esModificablePredeterminada = (!DireccionObtener.EsPredeterminado & DireccionInsertar.EsPredeterminado) || (DireccionObtener.EsPredeterminado & !DireccionInsertar.EsPredeterminado);
                IMapper.Map(DireccionInsertar, DireccionObtener);
                if (tipoAccion == "I")
                {
                    EntidadEditar.DireccionesInsertar.Add(DireccionInsertar);
                    EntidadObtener.Direcciones.Add(DireccionObtener);
                    direccionState.InsertedItem = DireccionObtener;
                }
                else
                {
                    EntidadEditar.DireccionesInsertar[indexEdit] = DireccionInsertar;
                    EntidadObtener.Direcciones[indexGrid] = DireccionObtener;
                }
                break;
            case "E":
                esModificablePredeterminada = (!DireccionObtener.EsPredeterminado & DireccionEditar.EsPredeterminado) || (DireccionObtener.EsPredeterminado & !DireccionEditar.EsPredeterminado);
                if (indexEdit > -1)
                    EntidadEditar.DireccionesEditar[indexEdit] = DireccionEditar;
                else
                    EntidadEditar.DireccionesEditar.Add(DireccionEditar);
                EntidadObtener.Direcciones[indexGrid] = IMapper.Map(DireccionEditar, DireccionObtener);
                break;
            case "Q" or "X":
                if (tipoAccion == "Q")
                {
                    if (EntidadEditar.DireccionesInsertar[indexEdit].EsPredeterminado)
                        DireccionPredeterminadoObtener = new();

                    EntidadEditar.DireccionesInsertar.RemoveAt(indexEdit);
                }
                else
                {
                    EntidadEditar.DireccionesEliminar.Add(new() { Id = (Guid) DireccionObtener.Id });
                    if (indexEdit > -1)
                    {
                        if (EntidadEditar.DireccionesEditar[indexEdit].EsPredeterminado)
                            DireccionPredeterminadoObtener = new();
                        
                        EntidadEditar.DireccionesEditar.RemoveAt(indexEdit);
                    }
                }
                EntidadObtener.Direcciones.RemoveAt(indexGrid);
                CerrarDialog();
                break;
        };
        CerrarModal();
        if(esModificablePredeterminada)
            ModificarDireccionPredeterminada();
        IsModified = true;
        await GridDireccionRef.SetStateAsync(direccionState);
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

    public static void OnRowVehiculosRenderHandler(GridRowRenderEventArgs args) => args.Class = !(args.Item as EntidadVehiculoObtenerDto).Id.HasValue ? "new-row" : "";
    
    public void MostrarAgregarVehiculo()
    {
        VehiculoInsertar = new();
        VehiculoObtener = new()
        {
            Activo = true
        };
        IniciarAccionModal("I", "vehiculo");
    }

    public void MostrarModificarVehiculo(EntidadVehiculoObtenerDto item)
    {
        string tipoAccion = !item.Id.HasValue ? "M" : "E";
        if (tipoAccion is "M")
            VehiculoEditar = IMapper.Map<EntidadVehiculoEditarDto>(item); 
        else 
            VehiculoInsertar = IMapper.Map<EntidadVehiculoInsertarDto>(item);
        
        VehiculoObtener = item;
        IniciarAccionModal(tipoAccion, "vehiculo");
    }

    private void MostrarQuitarVehiculo(EntidadVehiculoObtenerDto item)
    {
        VehiculoObtener = item;
        IniciarAccionDialog(!item.Id.HasValue ? "Q" : "X", "vehiculo");
    }

    public void VerItemVehiculo(EntidadVehiculoObtenerDto item)
    {
        VehiculoObtener = item;
        IniciarAccionModal("V", "vehiculo");
    }

    private bool ValidarVehiculos()
    {
        foreach (EntidadVehiculoObtenerDto item in EntidadObtener.Vehiculos.Where(x => string.Concat(x.Id.HasValue ? 1 : 0, x.CodigoVehiculo) != string.Concat(0, VehiculoInsertar.CodigoVehiculo)))
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
        int indexGrid = EntidadObtener.Vehiculos.FindIndex(i => i.CodigoVehiculo.Trim() == VehiculoObtener.CodigoVehiculo.Trim());
        int indexEdit = tipoAccion is "E" or "X" ? EntidadEditar.VehiculosEditar.FindIndex(i => i.Id == VehiculoObtener.Id) : EntidadEditar.VehiculosInsertar.FindIndex(i => i.CodigoVehiculo.Trim() == VehiculoObtener.CodigoVehiculo.Trim());
        
        switch (tipoAccion)
        {
            case "I" or "M":
                if (ValidarVehiculos())
                    return;
                IMapper.Map(VehiculoInsertar, VehiculoObtener);
                if (tipoAccion == "I")
                {
                    EntidadEditar.VehiculosInsertar.Add(VehiculoInsertar);
                    EntidadObtener.Vehiculos.Add(VehiculoObtener);
                    vehiculoState.InsertedItem = VehiculoObtener;
                }
                else
                {
                    EntidadEditar.VehiculosInsertar[indexEdit] = VehiculoInsertar;
                    EntidadObtener.Vehiculos[indexGrid] = VehiculoObtener;
                }
                break;
            case "E":
                if (indexEdit > -1)
                    EntidadEditar.VehiculosEditar[indexEdit] = VehiculoEditar;
                else
                    EntidadEditar.VehiculosEditar.Add(VehiculoEditar);
                EntidadObtener.Vehiculos[indexGrid] = IMapper.Map(VehiculoEditar, VehiculoObtener);
                break;
            case "Q" or "X":
                if (tipoAccion == "Q")
                    EntidadEditar.VehiculosInsertar.RemoveAt(indexEdit);
                else
                {
                    EntidadEditar.VehiculosEliminar.Add(new() { Id = (Guid) VehiculoObtener.Id });
                    if (indexEdit > -1)
                        EntidadEditar.VehiculosEditar.RemoveAt(indexEdit);
                }
                EntidadObtener.Vehiculos.RemoveAt(indexGrid);
                CerrarDialog();
                break;
        };
        CerrarModal();
        IsModified = true;
        await GridVehiculoRef.SetStateAsync(vehiculoState);
    }
    #endregion

    #region Representante
    private async Task CerrarRepresentante()
    {
        if (TipoAccionModal is not "V" && IsModifiedRepresentante && !await Dialog.ConfirmAsync("¿Está seguro de cerrar el formulario y que los datos no se carguen?", "Saliendo del formulario"))
            return;
        CerrarModal();
    }

    public static void OnRowRepresentantesRenderHandler(GridRowRenderEventArgs args) => args.Class = !(args.Item as EntidadRepresentanteObtenerDto).Id.HasValue ? "new-row" : "";
    
    public void MostrarAgregarRepresentante()
    {
        RepresentanteInsertar = new();
        RepresentanteObtener = new()
        {
            Activo = true
        };
        IniciarAccionModal("I", "representante");
    }

    public void MostrarModificarRepresentante(EntidadRepresentanteObtenerDto item)
    {
        string tipoAccion = !item.Id.HasValue ? "M" : "E";
        if (tipoAccion is "M")
            RepresentanteEditar = IMapper.Map<EntidadRepresentanteEditarDto>(item);
        else
            RepresentanteInsertar = IMapper.Map<EntidadRepresentanteInsertarDto>(item);
        
        RepresentanteObtener = item;
        IniciarAccionModal(tipoAccion, "representante");
    }

    private void MostrarQuitarRepresentante(EntidadRepresentanteObtenerDto item)
    {
        RepresentanteObtener = item;
        IniciarAccionDialog(!item.Id.HasValue ? "Q" : "X", "representante");
    }

    public void VerItemRepresentante(EntidadRepresentanteObtenerDto item)
    {
        RepresentanteObtener = item; 
        IniciarAccionModal("V", "representante");
    }

    private bool ValidarRepresentantes()
    {
        foreach (EntidadRepresentanteObtenerDto item in EntidadObtener.Representantes.Where(x => string.Concat(x.Id.HasValue ? 1 : 0, x.NumeroTipoIdentificacion.Trim()) != string.Concat(0, RepresentanteInsertar.NumeroTipoIdentificacion.Trim())))
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
        int indexGrid = EntidadObtener.Representantes.FindIndex(i => i.NumeroTipoIdentificacion.Trim() == RepresentanteObtener.NumeroTipoIdentificacion.Trim());
        int indexEdit = tipoAccion is "E" or "X" ? EntidadEditar.RepresentantesEditar.FindIndex(i => i.Id == RepresentanteObtener.Id) : EntidadEditar.RepresentantesInsertar.FindIndex(i => i.NumeroTipoIdentificacion.Trim() == RepresentanteObtener.NumeroTipoIdentificacion.Trim());
        
        switch (tipoAccion)
        {
            case "I" or "M":
                if (ValidarRepresentantes())
                    return;
                IMapper.Map(RepresentanteInsertar, RepresentanteObtener);
                if (tipoAccion == "I")
                {
                    EntidadEditar.RepresentantesInsertar.Add(RepresentanteInsertar);
                    EntidadObtener.Representantes.Add(RepresentanteObtener);
                    representanteState.InsertedItem = RepresentanteObtener;
                }
                else
                {
                    EntidadEditar.RepresentantesInsertar[indexEdit] = RepresentanteInsertar;
                    EntidadObtener.Representantes[indexGrid] = RepresentanteObtener;
                }
                break;
            case "E":
                if (indexEdit > -1)
                    EntidadEditar.RepresentantesEditar[indexEdit] = RepresentanteEditar;
                else
                    EntidadEditar.RepresentantesEditar.Add(RepresentanteEditar);
                EntidadObtener.Representantes[indexGrid] = IMapper.Map(RepresentanteEditar, RepresentanteObtener);
                break;
            case "Q" or "X":
                if (tipoAccion == "Q")
                    EntidadEditar.RepresentantesInsertar.RemoveAt(indexEdit);
                else
                { 
                    EntidadEditar.RepresentantesEliminar.Add(new() { Id = (Guid) RepresentanteObtener.Id });
                    if (indexEdit > -1) 
                        EntidadEditar.RepresentantesEditar.RemoveAt(indexEdit);
                }
                EntidadObtener.Representantes.RemoveAt(indexGrid);
                CerrarDialog();
                break;
        };
        CerrarModal();
        IsModified = true;
        await GridRepresentanteRef.SetStateAsync(representanteState);
    }
    #endregion

    #region Catalogos

    private void CargarItemCatalogoRegion(RegionCatalogoDto item)
    {
        if (string.IsNullOrEmpty(DireccionObtener.CodigoRegion) || DireccionObtener.CodigoRegion != item.CodigoRegion)
        {
            if (item.CodigoRegion.Trim() != DireccionObtener.CodigoRegion?.Trim())
            {
                if (TipoAccionModal is "I" or "M")
                    DireccionInsertar.CodigoDistrito = null;
                else if (TipoAccionModal is "E")
                    DireccionEditar.CodigoDistrito = null;

                DireccionObtener.CodigoProvincia = null;
                DireccionObtener.NombreProvincia = DireccionObtener.NombreDistrito = "";
            }

            DireccionObtener.CodigoRegion = item.CodigoRegion;
            DireccionObtener.NombreRegion = item.NombreRegion;
            IsModifiedDireccion = true;
        } 
    }

    private void CargarItemCatalogoProvinciaPorRegion(ProvinciaCatalogoPorRegionDto item)
    {
        if (string.IsNullOrEmpty(DireccionObtener.CodigoProvincia) || DireccionObtener.CodigoProvincia != item.CodigoProvincia)
        {
            if (item.CodigoProvincia.Trim() != DireccionObtener.CodigoProvincia?.Trim())
            {
                if (TipoAccionModal is "I" or "M")
                    DireccionInsertar.CodigoDistrito = null;
                else if (TipoAccionModal is "E")
                    DireccionEditar.CodigoDistrito = null;
                
                DireccionObtener.NombreDistrito = "";
            }

            DireccionObtener.CodigoProvincia = item.CodigoProvincia;
            DireccionObtener.NombreProvincia = item.NombreProvincia;
            IsModifiedDireccion = true;
        } 
    }

    private void CargarItemCatalogoDistritoPorProvincia(DistritoCatalogoPorProvinciaDto item)
    {
        if (string.IsNullOrEmpty(DireccionObtener.CodigoDistrito) || DireccionObtener.CodigoDistrito != item.CodigoDistrito)
        {
            if (TipoAccionModal is "I" or "M")
            {
                DireccionInsertar.CodigoDistrito = item.CodigoDistrito;
                EditContextDireccionInsertar.NotifyFieldChanged(EditContextDireccionInsertar.Field("CodigoDistrito"));
            }
            else if (TipoAccionModal is "E")
            {
                DireccionEditar.CodigoDistrito = item.CodigoDistrito;
                EditContextDireccionEditar.NotifyFieldChanged(EditContextDireccionEditar.Field("CodigoDistrito"));
            }

            DireccionObtener.NombreDistrito = item.NombreDistrito;
            IsModifiedDireccion = true;    
        } 
    }

    private void CargarItemCatalogoTipoIdentificacion(TipoIdentificacionCatalogoDto item)
    {
        if (string.IsNullOrEmpty(RepresentanteObtener.CodigoTipoIdentificacion) || RepresentanteObtener.CodigoTipoIdentificacion != item.CodigoTipoIdentificacion)
        {
            RepresentanteInsertar.CodigoTipoIdentificacion = item.CodigoTipoIdentificacion;
            RepresentanteObtener.SiglaTipoIdentificacion = item.SiglaTipoIdentificacion;
            RepresentanteObtener.NombreTipoIdentificacion = item.NombreTipoIdentificacion;
            EditContextRepresentanteInsertar.NotifyFieldChanged(EditContextRepresentanteInsertar.Field("CodigoTipoIdentificacion"));
            IsModifiedRepresentante = true;
        } 
    }

    private void CargarItemCatalogoVehiculo(VehiculoCatalogoDto item)
    {
        if (string.IsNullOrEmpty(VehiculoObtener.CodigoVehiculo ) || VehiculoObtener.CodigoVehiculo != item.CodigoVehiculo)
        {
            VehiculoInsertar.CodigoVehiculo = item.CodigoVehiculo;
            VehiculoObtener.MarcaVehiculo = item.MarcaVehiculo;
            EditContextVehiculoInsertar.NotifyFieldChanged(EditContextVehiculoInsertar.Field("CodigoVehiculo"));
            IsModifiedVehiculo = true;
        } 
    }
    #endregion

    public void Dispose() => GC.SuppressFinalize(this);
}