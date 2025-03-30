namespace GestionERP.Web.Models.Dtos.Principal;

public class TipoFacturaNegociableListarDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public int CuentaBancariaId { get; set; }
    public string CodigoCuentaBancaria { get; set; }
    public string NombreCuentaBancaria { get; set; }
    public string NumeroProductoBanco { get; set; }
    public int? DocumentoGeneraId { get; set; }
    public string CodigoDocumentoGenera { get; set; }
    public string NombreDocumentoGenera { get; set; }
    public string Descripcion { get; set; }
    public bool Activo { get; set; }
}