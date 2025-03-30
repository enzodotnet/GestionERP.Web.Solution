namespace GestionERP.Web.Models.Dtos.Produccion;

public class OrdenAcceso
{
    public static string VerRegistros { get; } = "S112VIREOP";
    public static string EmitirDirecta { get; } = "S112EMRDOP";
    public static string EmitirPorSolicitud { get; } = "S112EMRSOP";
    public static string Anular { get; } = "S112ANREOP";
    public static string Editar { get; } = "S112EDREOP";
    public static string Eliminar { get; } = "S112ELREOP";
    public static string Excluir { get; } = "S112ECREOP";
    public static string VerReporte { get; } = "S112VIRPOP";
    public static string VerAuditoria { get; } = "S112VIAUOP";
    public static string Imprimir { get; } = "S112IMRPOP";
    public static string ExportarReportePDF { get; } = "S112EXRPOP";
    public static string VerFormularioAprobacion { get; } = "S112VIFAOP";
    public static string Aprobar { get; } = "S112APREOP";
    public static string Desaprobar { get; } = "S112DSREOP";
    public static string Desestimar { get; } = "S112DTREOP";
    public static string VerFormularioCancelar { get; } = "S112VIFROP";
    public static string Cancelar { get; } = "S112CAREOP";
    public static string EnviarAprobacion { get; } = "S112ENAPOP";
    public static string RevertirEnvioAprobacion { get; } = "S112RRAPOP";
    public static string EnviarProceso { get; } = "S112ENPROP";
    public static string RevertirEnvioProceso { get; } = "S112RREPOP";
    public static string EnviarIngreso { get; } = "S112ENIAOP";
    public static string RevertirEnvioIngreso { get; } = "S112RREIOP";
    public static string Cerrar { get; } = "S112CRREOP";
    public static string Aperturar { get; } = "S112AUREOP";
    public static string Costear { get; } = "S112COREOP";
    public static string RetirarCosteo { get; } = "S112DCREOP";
}