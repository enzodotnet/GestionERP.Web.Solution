namespace GestionERP.Web.Models.Dtos.Principal.Types;

public class FinancieraRegistroType
{
    public string Codigo { get; set; }
    public string Nombre { get; set; } 

    public static IEnumerable<FinancieraRegistroType> ObtenerTipos()
    { 
        return new List<FinancieraRegistroType>
        {
            new FinancieraRegistroType {Codigo = "B", Nombre = "Banco"},
            new FinancieraRegistroType {Codigo = "C", Nombre = "Caja" },
            new FinancieraRegistroType {Codigo = "O", Nombre = "Otro" }
        };
    }
}