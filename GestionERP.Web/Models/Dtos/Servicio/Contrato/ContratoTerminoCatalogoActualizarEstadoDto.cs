using GestionERP.Web.Models.Dtos.Control;

namespace GestionERP.Web.Models.Dtos.Servicio;

public class ContratoTerminoCatalogoActualizarEstadoDto
{  
    public string CodigoTipoTermino { get; set; }
    public string NombreTipoTermino { get; set; }
    public int NumeroTermino { get; set; }
    public string Descripcion { get; set; } 
}