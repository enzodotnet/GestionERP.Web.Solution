namespace GestionERP.Web.Global;

/// <summary>
/// Clase global <c>Configuración</c> de atributos constantes compartidos.
/// </summary>
public class Cnf
{
    public static string ServerApiURI { get; } = "https://localhost:7113/api/";
    public static string ServerReportURI { get; } = "https://localhost:7218/api/reports";

	public const string MsgErrorExpiredToken = "Lo sentimos, su token de autorización al sistema ya ha expirado. Para generar uno nuevo deberá volver a loguearse.";
	public const string MsgErrorNoAuthenticated = "Lo sentimos, la sesión de su usuario ya ha sido cerrada. Si necesita seguir en el sistema deberá volver a loguearse.";
	public const string MsgErrorFuncAppWeb = "Error funcional en el aplicativo web. Por favor contacte al administrador del sistema.";
	public const string MsgErrorNotConnectApi = "Error de conexión hacia el servidor de APIs. Por favor contacte al administrador del sistema.";
	public const string MsgErrorNotFoundAPi = "Error de API no encontrado en el servidor. Por favor contacte al administrador del sistema.";
	public const string MsgErrorSesionAltered = "Lo sentimos, la sesión anterior ha sido alterada desde otra ventana del navegador, el usuario actual es de [{cu}]. Por lo que deberá refrescar la página.";
	public const string MsgErrorInvalidEditContext = "Existen campos en el formulario pendientes de completar, verificar por favor.";
	public const string MsgErrorNoConfigMontoIGV = "El tipo de impuesto predeterminado para operaciones comerciales no está configurado. Por favor contacte al administrador del sistema.";
	public const string MsgErrorNoConfigPrinterDoc = "El numerador del documento no tiene configurado una impresora predeterminada. Por favor contacte al administrador del sistema.";
}