namespace GestionERP.Web.Models.Dtos.Produccion;

public class EquipoObtenerDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; } 
    public string FlagTipoMaquinaria { get; set; }
    public string Descripcion { get; set; }
    public bool Activo { get; set; } 
}