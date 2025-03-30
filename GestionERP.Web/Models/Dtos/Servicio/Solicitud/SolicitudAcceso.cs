namespace GestionERP.Web.Models.Dtos.Servicio;

public class SolicitudAcceso
{
    public static string VerRegistros { get; } = "S108VIRESS";
    public static string Emitir { get; } = "S108EMRESS";
    public static string Anular { get; } = "S108ANRESS";
    public static string Editar { get; } = "S108EDRESS";
    public static string Eliminar { get; } = "S108ELRESS";
    public static string Excluir { get; } = "S108ECRESS";
    public static string VerReporte { get; } = "S108VIRPSS";
    public static string VerAuditoria { get; } = "S108VIAUSS";
    public static string RequerirRevision { get; } = "S108RQRVSS";
    public static string Imprimir { get; } = "S108IMRPSS";
    public static string ImprimirDirecto { get; } = "S108IMDRSS";
    public static string ExportarReportePDF { get; } = "S108EXRPSS";
    public static string VerFormularioRevision { get; } = "S108VIFVSS";
    public static string Revisar { get; } = "S108RVRESS";
    public static string RetirarRevision { get; } = "S108REVRSS";
    public static string VerFormularioAprobacion { get; } = "S108VIFASS";
    public static string Aprobar { get; } = "S108APRESS";
    public static string Desaprobar { get; } = "S108DSRESS";
    public static string Desestimar { get; } = "S108DTRESS";
    public static string VerFormularioRescindir { get; } = "S108VIFRSS";
    public static string Rescindir { get; } = "S108RSRESS";
    public static string VerFormularioCancelar { get; } = "S108VIFRSS";
    public static string Cancelar { get; } = "S108CARESS";
    public static string EnviarRevision { get; } = "S108ENRVSS";
    public static string RevertirEnvioRevision { get; } = "S108RRRVSS";
    public static string EnviarAprobacion { get; } = "S108ENAPSS";
    public static string RevertirEnvioAprobacion { get; } = "S108RRAPSS";
}