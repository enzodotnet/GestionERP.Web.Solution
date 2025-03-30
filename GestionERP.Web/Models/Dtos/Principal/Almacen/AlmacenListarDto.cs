namespace GestionERP.Web.Models.Dtos.Principal;

public class AlmacenListarDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public string FlagAtributo { get; set; } 
    public string CodigoCuentaContable { get; set; }
    public string NombreCuentaContable { get; set; }
    public string CodigoTipoAlmacen { get; set; }
    public string NombreTipoAlmacen { get; set; } 
    public bool EsDestinadoVenta { get; set; }
    public bool EsDestinadoCompra { get; set; }
    public bool EsDestinadoProduccion { get; set; }
    public bool EsDestinadoTraslado { get; set; }
    public bool EsDestinadoControl { get; set; }
    public bool Activo { get; set; }
}