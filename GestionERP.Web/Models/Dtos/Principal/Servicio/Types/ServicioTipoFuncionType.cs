namespace GestionERP.Web.Models.Dtos.Principal.Types;

public class ServicioTipoFuncionType
{
    public string Codigo { get; set; }
    public string Nombre { get; set; }

    public static IEnumerable<ServicioTipoFuncionType> ObtenerTipos()
    { 
        return new List<ServicioTipoFuncionType>
        {
            new ServicioTipoFuncionType {Codigo = "C", Nombre = "Consulta"},
            new ServicioTipoFuncionType {Codigo = "M", Nombre = "Mantenimiento"},
            new ServicioTipoFuncionType {Codigo = "R", Nombre = "Reporte"},
            new ServicioTipoFuncionType {Codigo = "V", Nombre = "Validación"}
        };
    }
}