namespace GestionERP.Web.Models.Dtos.Almacen;

public class MovimientoAcceso
{
    public static string VerRegistros { get; } = "S130VIREMV";
    public static string RegistrarIngreso { get; } = "S130RGRIMV";
    public static string RegistrarSalida { get; } = "S130RGRSMV";
    public static string RegistrarTransferencia { get; } = "S130RGRTMV";
    public static string Anular { get; } = "S130ANREMV"; 
    public static string Eliminar { get; } = "S130ELREMV";
    public static string Excluir { get; } = "S130ECREMV"; 
    public static string VerReporte { get; } = "S130VIRPMV";
    public static string VerAuditoria { get; } = "S130VIAUMV";
    public static string Imprimir { get; } = "S130IMRPMV"; 
    public static string ExportarReportePDF { get; } = "S130EXRPMV"; 
}