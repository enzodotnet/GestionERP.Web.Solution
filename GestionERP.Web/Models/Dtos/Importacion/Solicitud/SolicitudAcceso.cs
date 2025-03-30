namespace GestionERP.Web.Models.Dtos.Importacion;

public class SolicitudAcceso
{
    public static string VerRegistros { get; } = "S104VIRESI";
    public static string Emitir { get; } = "S104EMRESI";
    public static string Anular { get; } = "S104ANRESI";
    public static string Editar { get; } = "S104EDRESI";
    public static string Eliminar { get; } = "S104ELRESI";
    public static string Excluir { get; } = "S104ECRESI";
    public static string VerReporte { get; } = "S104VIRPSI";
    public static string VerAuditoria { get; } = "S104VIAUSI";
    public static string Imprimir { get; } = "S104IMRPSI";
    public static string ImprimirDirecto { get; } = "S104IMDRSI";
    public static string ExportarReportePDF { get; } = "S104EXRPSI";
    public static string VerFormularioAprobacion { get; } = "S104VIFASI";
    public static string Aprobar { get; } = "S104APRESI";
    public static string Desaprobar { get; } = "S104DSRESI";
    public static string Desestimar { get; } = "S104DTRESI";
    public static string VerFormularioRescindir { get; } = "S104VIFRSI";
    public static string Rescindir { get; } = "S104RSRESI";
    public static string VerFormularioCancelar { get; } = "S104VIFRSI";
    public static string Cancelar { get; } = "S104CARESI";
    public static string EnviarAprobacion { get; } = "S104ENAPSI";
    public static string RevertirEnvioAprobacion { get; } = "S104RRAPSI";
}