namespace GestionERP.Web.Models.Dtos.Servicio;

public class ContratoAcceso
{
    public static string VerRegistros { get; } = "S110VIRECN";
    public static string Emitir { get; } = "S110EMRECN";
    public static string Anular { get; } = "S110ANRECN";
    public static string Editar { get; } = "S110EDRECN";
    public static string Eliminar { get; } = "S110ELRECN";
    public static string Excluir { get; } = "S110ECRECN"; 
	public static string VerReporte { get; } = "S110VIRPCN";
    public static string VerAuditoria { get; } = "S110VIAUCN";
    public static string Imprimir { get; } = "S110IMRPCN";
    public static string ImprimirDirecto { get; } = "S110IMDRCN";
    public static string ExportarReportePDF { get; } = "S110EXRPCN";
    public static string VerFormularioAprobacion { get; } = "S110VIFACN";    
    public static string Aprobar { get; } = "S110APRECN";
    public static string Desaprobar { get; } = "S110DSRECN";
    public static string Desestimar { get; } = "S110DTRECN";
    public static string VerFormularioRescindir { get; } = "S110VIFRCN";
    public static string Rescindir { get; } = "S110RSRECN";
    public static string VerFormularioCancelar { get; } = "S110VIFRCN";
    public static string Cancelar { get; } = "S110CARECN";
    public static string EnviarAprobacion { get; } = "S110ENAPCN";
    public static string RevertirEnvioAprobacion { get; } = "S110RRAPCN";
}