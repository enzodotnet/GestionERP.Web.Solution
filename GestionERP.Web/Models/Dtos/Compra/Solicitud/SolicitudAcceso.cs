namespace GestionERP.Web.Models.Dtos.Compra;

public class SolicitudAcceso
{
    public static string VerRegistros { get; } = "S102VIRESC";
    public static string Emitir { get; } = "S102EMRESC";
    public static string Anular { get; } = "S102ANRESC";
    public static string Editar { get; } = "S102EDRESC";
    public static string Eliminar { get; } = "S102ELRESC";
    public static string Excluir { get; } = "S102ECRESC";
    public static string VerReporte { get; } = "S102VIRPSC";
    public static string VerAuditoria { get; } = "S102VIAUSC";
    public static string RequerirRevision { get; } = "S102RQRVSC";
    public static string Imprimir { get; } = "S102IMRPSC";
    public static string ImprimirDirecto { get; } = "S102IMDRSC";
    public static string ExportarReportePDF { get; } = "S102EXRPSC";
    public static string VerFormularioRevision { get; } = "S102VIFVSC";
    public static string Revisar { get; } = "S102RVRESC";
    public static string RetirarRevision { get; } = "S102REVRSC";
    public static string VerFormularioAprobacion { get; } = "S102VIFASC";
    public static string Aprobar { get; } = "S102APRESC";
    public static string Desaprobar { get; } = "S102DSRESC";
    public static string Desestimar { get; } = "S102DTRESC";
    public static string VerFormularioRescindir { get; } = "S102VIFRSC";
    public static string Rescindir { get; } = "S102RSRESC";
    public static string VerFormularioCancelar { get; } = "S102VIFRSC";
    public static string Cancelar { get; } = "S102CARESC";
    public static string EnviarRevision { get; } = "S102ENRVSC";
    public static string RevertirEnvioRevision { get; } = "S102RRRVSC";
    public static string EnviarAprobacion { get; } = "S102ENAPSC";
    public static string RevertirEnvioAprobacion { get; } = "S102RRAPSC";
}