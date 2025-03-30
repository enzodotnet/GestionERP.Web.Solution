namespace GestionERP.Web.Models.Dtos.Principal;

public class TipoArticuloObtenerDto
{
    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public string FlagCategoria { get; set; }
    public bool EsExistencia { get; set; }
    public bool Activo { get; set; }
    public IEnumerable<TipoArticuloExistenciaObtenerDto> Existencias { get; set ;}
}