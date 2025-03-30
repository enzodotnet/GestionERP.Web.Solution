namespace GestionERP.Web.Models.Dtos.Produccion;

public class SolicitudAcceso
{
    public static string VerRegistros { get; } = "S111VIRESP";
    public static string Emitir { get; } = "S111EMRESP";
    public static string Anular { get; } = "S111ANRESP";
    public static string Editar { get; } = "S111EDRESP";
    public static string Eliminar { get; } = "S111ELRESP";
    public static string Excluir { get; } = "S111ECRESP";
    public static string VerReporte { get; } = "S111VIRPSP";
    public static string VerAuditoria { get; } = "S111VIAUSP";
    public static string Imprimir { get; } = "S111IMRPSP";
    public static string ExportarReportePDF { get; } = "S111EXRPSP";
    public static string VerFormularioAprobacion { get; } = "S111VIFASP";
    public static string Aprobar { get; } = "S111APRESP";
    public static string Desaprobar { get; } = "S111DSRESP";
    public static string Desestimar { get; } = "S111DTRESP";
    public static string VerFormularioRescindir { get; } = "S111VIFRSP";
    public static string Rescindir { get; } = "S111RSRESP";
    public static string VerFormularioCancelar { get; } = "S111VIFRSP";
    public static string Cancelar { get; } = "S111CARESP";
    public static string EnviarAprobacion { get; } = "S111ENAPSP";
    public static string RevertirEnvioAprobacion { get; } = "S111RRAPSP";
}