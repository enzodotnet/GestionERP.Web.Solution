namespace GestionERP.Web.Models.Dtos.Principal;

public class CierreContableDetalleObtenerDto
{
    public Guid Id { get; set; }
    public int NumeroItem { get; set; }
    public string NumeroCuentaInicio { get; set; }
    public string NumeroCuentaFinal { get; set; }
    public int CuentaContableDestinoId { get; set; }
    public string CodigoCuentaContable { get; set; }
    public string NombreCuentaContable { get; set; }
    public int NumeroOrden { get; set; }
}