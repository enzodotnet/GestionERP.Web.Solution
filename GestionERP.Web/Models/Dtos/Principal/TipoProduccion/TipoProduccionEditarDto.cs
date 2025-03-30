using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class TipoProduccionEditarDto
{  
    public string Nombre { get; set; }
	public string CodigoAlmacenProceso { get; set; }
	public bool EsExigibleTareo { get; set; } 
    public bool EsExigibleMaquila { get; set; } 
    public bool EsValidadorIngreso { get; set; }
    public bool EsValidadorSobrante { get; set; }
    public string Descripcion { get; set; }
    public bool Activo { get; set; }
}

public class TipoProduccionEditarValidator : AbstractValidator<TipoProduccionEditarDto>
{
    public TipoProduccionEditarValidator()
    { 
        RuleFor(p => p.Nombre)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .MaximumLength(50).WithMessage("El campo {PropertyName} debe tener como máximo 50 caracteres");

		RuleFor(p => p.CodigoAlmacenProceso)
			.NotEmpty().WithMessage("El campo {PropertyName} es requerido");

		RuleFor(p => p.Descripcion)
            .MaximumLength(200).WithMessage("El campo {PropertyName} debe tener como máximo 200 caracteres");
    }
}