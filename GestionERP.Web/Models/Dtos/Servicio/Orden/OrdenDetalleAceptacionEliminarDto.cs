using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Servicio;

public class OrdenDetalleAceptacionEliminarDto
{  
    public Guid OrdenDetalleAceptacionId { get; set; }
    public string Motivo { get; set; }
}