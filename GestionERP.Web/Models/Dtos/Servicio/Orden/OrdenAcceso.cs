namespace GestionERP.Web.Models.Dtos.Servicio;

public class OrdenAcceso
{
    public static string VerRegistros { get; } = "S109VIREOS";
    public static string EmitirDirecta { get; } = "S109EMRDOS";
    public static string EmitirPorSolicitud { get; } = "S109EMRSOS";
    public static string Anular { get; } = "S109ANREOS";
    public static string Editar { get; } = "S109EDREOS";
    public static string Eliminar { get; } = "S109ELREOS";
    public static string Excluir { get; } = "S109ECREOS"; 
	public static string VerReporte { get; } = "S109VIRPOS";
    public static string VerAuditoria { get; } = "S109VIAUOS";
    public static string Imprimir { get; } = "S109IMRPOS";
    public static string ImprimirDirecto { get; } = "S109IMDROS";
    public static string ExportarReportePDF { get; } = "S109EXRPOS";
    public static string VerFormularioAprobacion { get; } = "S109VIFAOS";    
    public static string Aprobar { get; } = "S109APREOS";
    public static string Desaprobar { get; } = "S109DSREOS";
    public static string VerFormularioAceptacion { get; } = "S109VIFTOS";    
    public static string Aceptar { get; } = "S109ACRAOS";
    public static string RetirarAceptacion { get; } = "S109RERCOS";
    public static string Desestimar { get; } = "S109DTREOS";
    public static string VerFormularioRescindir { get; } = "S109VIFROS";
    public static string Rescindir { get; } = "S109RSREOS";
    public static string VerFormularioCancelar { get; } = "S109VIFROS";
    public static string Cancelar { get; } = "S109CAREOS";
    public static string EnviarAprobacion { get; } = "S109ENAPOS";
    public static string RevertirEnvioAprobacion { get; } = "S109RRAPOS";
}