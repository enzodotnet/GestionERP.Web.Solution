using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class SerieDocumentoEliminarDto
{ 
    public Guid Id { get; set; }  
}

public class SerieDocumentoEliminarValidator : AbstractValidator<SerieDocumentoEliminarDto>
{
    public SerieDocumentoEliminarValidator()
    {
        RuleFor(p => p.Id).NotEmpty().WithMessage("El campo {PropertyName} es requerido en el eliminar numeradores del documento");
    }
}