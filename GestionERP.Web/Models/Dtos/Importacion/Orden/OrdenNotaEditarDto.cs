using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Importacion;

public class OrdenNotaEditarDto
{
    public Guid Id { get; set; }
    public string Contenido { get; set; }
}

public class OrdenNotaEditarValidator : AbstractValidator<OrdenNotaEditarDto>
{
    public OrdenNotaEditarValidator()
    {
        RuleFor(p => p.Contenido).NotEmpty().WithMessage("El campo {PropertyName} es requerido en el editar notas de la orden");
    }
}