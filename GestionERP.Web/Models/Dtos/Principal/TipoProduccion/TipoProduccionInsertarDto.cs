using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class TipoProduccionInsertarDto
{  
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string FlagTipoProceso { get; set; }
    public string FlagAmbito { get; set; }
    public string FlagTipoEncargo { get; set; }
	public string CodigoAlmacenProceso { get; set; }
	public bool EsExigibleTareo { get; set; } 
    public bool EsExigibleMaquila { get; set; } 
    public bool EsValidadorIngreso { get; set; }
    public bool EsValidadorSobrante { get; set; }
    public string Descripcion { get; set; }
}

public class TipoProduccionInsertarValidator : AbstractValidator<TipoProduccionInsertarDto>
{
    public TipoProduccionInsertarValidator()
    { 
        RuleFor(p => p.Codigo)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .Length(3).WithMessage("El campo {PropertyName} debe tener 3 caracteres") 
            .Matches("^[A-Za-z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos");
 
        RuleFor(p => p.Nombre)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .MaximumLength(50).WithMessage("El campo {PropertyName} debe tener como máximo 50 caracteres");

		RuleFor(p => p.CodigoAlmacenProceso)
			.NotEmpty().WithMessage("El campo {PropertyName} es requerido");

		RuleFor(p => p.FlagTipoProceso)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido"); 
        
        RuleFor(p => p.FlagAmbito)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.FlagTipoEncargo)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido");
 
        RuleFor(p => p.Descripcion)
            .MaximumLength(200).WithMessage("El campo {PropertyName} debe tener como máximo 200 caracteres");
    }
}