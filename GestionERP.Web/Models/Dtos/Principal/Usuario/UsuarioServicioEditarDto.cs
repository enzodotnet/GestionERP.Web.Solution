using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class UsuarioServicioEditarDto
{  
    public Guid Id { get; set; }
    public bool EsAsignado { get; set; }
} 