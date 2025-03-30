namespace GestionERP.Web.Models.Dtos.Principal;

public class VehiculoListarDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Marca { get; set; }
    public string Modelo { get; set; }
    public string NumeroCirculacion { get; set; }
    public string Descripcion { get; set; }
    public bool Activo { get; set; }
}