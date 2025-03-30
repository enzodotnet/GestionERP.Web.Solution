namespace GestionERP.Web.Models.Dtos.Servicio;

public class SolicitudDetalleCatalogoActualizarEstadoDto
{ 
    public string CodigoArticulo { get; set; }
    public string NombreArticulo { get; set; }
    public string CodigoUnidadMedida { get; set; }
    public string NombreUnidadMedida { get; set; }
    public int Cantidad { get; set; }
    public int CantidadAtendida { get; set; }
    public string Observacion { get; set; }
}