namespace GestionERP.Web.Models.Dtos.Importacion;

public class OrdenAcceso
{
    public static string VerRegistros { get; } = "S105VIREOI";
    public static string EmitirDirecta { get; } = "S105EMRDOI";
    public static string EmitirPorSolicitud { get; } = "S105EMRSOI";
    public static string Anular { get; } = "S105ANREOI";
    public static string Editar { get; } = "S105EDREOI";
    public static string Eliminar { get; } = "S105ELREOI";
    public static string Excluir { get; } = "S105ECREOI";
	public static string VerReporte { get; } = "S105VIRPOI";
    public static string VerAuditoria { get; } = "S105VIAUOI";
    public static string Imprimir { get; } = "S105IMRPOI";
    public static string ImprimirDirecto { get; } = "S105IMDROI";
    public static string ExportarReportePDF { get; } = "S105EXRPOI";
    public static string VerFormularioAprobacion { get; } = "S105VIFAOI";
    public static string Aprobar { get; } = "S105APREOI";
    public static string Desaprobar { get; } = "S105DSREOI";
    public static string Desestimar { get; } = "S105DTREOI";
    public static string VerFormularioRescindir { get; } = "S105VIFROI";
    public static string Rescindir { get; } = "S105RSREOI";
    public static string VerFormularioCancelar { get; } = "S105VIFROI";
    public static string Cancelar { get; } = "S105CAREOI";
    public static string EnviarAprobacion { get; } = "S105ENAPOI";
    public static string RevertirEnvioAprobacion { get; } = "S105RRAPOI";
}