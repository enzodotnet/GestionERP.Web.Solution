namespace GestionERP.Web.Models.Dtos.Compra;

public class OrdenAcceso
{
    public static string VerRegistros { get; } = "S103VIREOC";
    public static string EmitirDirecta { get; } = "S103EMRDOC";
    public static string EmitirPorSolicitud { get; } = "S103EMRSOC";
    public static string Anular { get; } = "S103ANREOC";
    public static string Editar { get; } = "S103EDREOC";
    public static string Eliminar { get; } = "S103ELREOC";
    public static string Excluir { get; } = "S103ECREOC";
	public static string EnviarIngreso { get; } = "S103ECREOC";
    public static string RevertirEnvioIngreso { get; } = "S103RREIOC";
    public static string VerReporte { get; } = "S103VIRPOC";
    public static string VerAuditoria { get; } = "S103VIAUOC";
    public static string Imprimir { get; } = "S103IMRPOC";
    public static string ImprimirDirecto { get; } = "S103IMDROC";
    public static string ExportarReportePDF { get; } = "S103EXRPOC";
    public static string VerFormularioAprobacion { get; } = "S103VIFAOC";
    public static string Aprobar { get; } = "S103APREOC";
    public static string Desaprobar { get; } = "S103DSREOC";
    public static string Desestimar { get; } = "S103DTREOC";
    public static string VerFormularioRescindir { get; } = "S103VIFROC";
    public static string Rescindir { get; } = "S103RSREOC";
    public static string VerFormularioCancelar { get; } = "S103VIFROC";
    public static string Cancelar { get; } = "S103CAREOC";
    public static string EnviarAprobacion { get; } = "S103ENAPOC";
    public static string RevertirEnvioAprobacion { get; } = "S103RRAPOC";
}