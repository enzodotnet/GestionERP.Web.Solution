using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class TipoDevengoInsertarDto
{ 
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string CodigoGrupoDevengo { get; set; } 
    public int CantidadDiasCalculo { get; set; }
	public string CodigoOperacionFinanciera { get; set; }
	public string CodigoDocumentoCuota { get; set; }
	public string CodigoDocumentoGasto { get; set; }
	public string CodigoCuentaContableAdministrativo { get; set; } 
    public string CodigoCuentaContableComercial { get; set; } 
    public string CodigoCuentaContableEmision { get; set; } 
    public string CodigoCuentaContableGanancia { get; set; } 
    public string CodigoCuentaContablePerdida { get; set; } 
    public string Descripcion { get; set; }
}

public class TipoDevengoInsertarValidator : AbstractValidator<TipoDevengoInsertarDto>
{
    public TipoDevengoInsertarValidator()
    {
        RuleFor(p => p.Codigo)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .Length(2).WithMessage("El campo {PropertyName} debe tener 2 caracteres") 
            .Matches("^[A-Za-z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos");
 
        RuleFor(p => p.Nombre)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
        
        RuleFor(p => p.CodigoGrupoDevengo)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.CantidadDiasCalculo)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .ExclusiveBetween(1,31).WithMessage("El campo {PropertyName} debe estar comprendido entre 1 y 31");

		RuleFor(p => p.CodigoOperacionFinanciera).NotEmpty().WithMessage("El campo {PropertyName} es requerido");

		RuleFor(p => p.CodigoDocumentoCuota).NotEmpty().WithMessage("El campo {PropertyName} es requerido");

		RuleFor(p => p.CodigoDocumentoGasto).NotEmpty().WithMessage("El campo {PropertyName} es requerido");

		RuleFor(p => p.CodigoCuentaContableAdministrativo).NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.CodigoCuentaContableGanancia).NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.CodigoCuentaContablePerdida).NotEmpty().WithMessage("El campo {PropertyName} es requerido");
 
        RuleFor(p => p.Descripcion).MaximumLength(200).WithMessage("El campo {PropertyName} debe tener como máximo 200 caracteres");
    }
}