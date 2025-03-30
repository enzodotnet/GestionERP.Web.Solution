namespace GestionERP.Web.Models.Dtos.Principal;

public class OperacionLogisticaObtenerDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public string CodigoTipoMovimiento { get; set; }
    public string NombreTipoMovimiento { get; set; }
    public string CodigoTipoOperacionAlmacen { get; set; }
    public string NombreTipoOperacionAlmacen { get; set; }
    public string CodigoOperacionLogisticaDestino { get; set; }
    public string NombreOperacionLogisticaDestino { get; set; }
    public bool Activo { get; set; }
}