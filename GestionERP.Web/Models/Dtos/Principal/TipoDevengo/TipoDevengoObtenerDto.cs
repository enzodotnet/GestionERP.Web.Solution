namespace GestionERP.Web.Models.Dtos.Principal;

public class TipoDevengoObtenerDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string CodigoGrupoDevengo { get; set; }
    public string NombreGrupoDevengo { get; set; }
    public int CantidadDiasCalculo { get; set; }
	public string CodigoOperacionFinanciera { get; set; }
	public string NombreOperacionFinanciera { get; set; }
	public string CodigoDocumentoCuota { get; set; }
	public string NombreDocumentoCuota { get; set; }
	public string CodigoDocumentoGasto { get; set; }
	public string NombreDocumentoGasto { get; set; }
	public string CodigoCuentaContableAdministrativo { get; set; }
    public string NombreCuentaContableAdministrativo { get; set; }
    public string CodigoCuentaContableComercial { get; set; }
    public string NombreCuentaContableComercial { get; set; }
    public string CodigoCuentaContableEmision { get; set; }
    public string NombreCuentaContableEmision { get; set; }
    public string CodigoCuentaContableGanancia { get; set; }
    public string NombreCuentaContableGanancia { get; set; }
    public string CodigoCuentaContablePerdida { get; set; }
    public string NombreCuentaContablePerdida { get; set; }
    public string Descripcion { get; set; }
    public bool Activo { get; set; }  
}