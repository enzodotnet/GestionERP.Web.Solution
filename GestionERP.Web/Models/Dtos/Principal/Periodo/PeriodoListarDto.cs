namespace GestionERP.Web.Models.Dtos.Principal;

public class PeriodoListarDto
{
    public Guid Id { get; set; }
    public string CodigoEjercicio { get; set; }
    public string NombreEjercicio { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public int NumeroMes { get; set; } 
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public bool IniciaApertura { get; set; }
    public bool Activo { get; set; }
}