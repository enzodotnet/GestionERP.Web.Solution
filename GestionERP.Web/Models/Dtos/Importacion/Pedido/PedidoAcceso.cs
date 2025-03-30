namespace GestionERP.Web.Models.Dtos.Importacion;

public class PedidoAcceso
{
    public static string VerRegistros { get; } = "S106VIREPI";
    public static string Emitir { get; } = "S106EMREPI";
    public static string Anular { get; } = "S106ANREPI";
    public static string Editar { get; } = "S106EDREPI";
    public static string Eliminar { get; } = "S106ELREPI";
    public static string Excluir { get; } = "S106ECREPI";
    public static string EnviarIngreso { get; } = "S106ECREPI";
    public static string RevertirEnvioIngreso { get; } = "S106RREIPI";
    public static string VerReporte { get; } = "S106VIRPPI";
    public static string VerAuditoria { get; } = "S106VIAUPI";
    public static string Imprimir { get; } = "S106IMRPPI";
    public static string ExportarReportePDF { get; } = "S106EXRPPI";
	public static string Cerrar { get; } = "S106CRREPI";
	public static string Aperturar { get; } = "S106AUREPI";
	public static string Costear { get; } = "S106COREPI";
	public static string RetirarCosteo { get; } = "S106DCREPI";
}