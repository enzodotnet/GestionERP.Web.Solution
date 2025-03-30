using FluentValidation;
using System.Text.Json.Serialization;

namespace GestionERP.Web.Models.Dtos.Principal;

public class CuentaContableDetalleObtenerDto
{
    public Guid? Id { get; set; }
    public string FlagRegistro { get; set; }
    public string FlagTipo { get; set; }
    public string FlagTipoCambio { get; set; }
    public string CodigoCuentaContableGanancia { get; set; }
    public string NombreCuentaContableGanancia { get; set; }
    public string CodigoCuentaContablePerdida { get; set; }
    public string NombreCuentaContablePerdida { get; set; }
}