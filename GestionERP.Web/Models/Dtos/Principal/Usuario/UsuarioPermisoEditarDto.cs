using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class UsuarioPermisoEditarDto
{  
    public Guid Id { get; set; }
    public bool EsAsignado { get; set; } 
} 