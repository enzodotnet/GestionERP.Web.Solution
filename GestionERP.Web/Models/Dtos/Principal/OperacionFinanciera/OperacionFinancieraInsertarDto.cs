using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class OperacionFinancieraInsertarDto
{ 
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public string FlagTipoCambio { get; set; } 
    public string CodigoTipoTransaccion { get; set; }  
    public bool EsReservado { get; set; }
}

public class OperacionFinancieraInsertarValidator : AbstractValidator<OperacionFinancieraInsertarDto>
{
    public OperacionFinancieraInsertarValidator()
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
         
        RuleFor(p => p.Descripcion)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");

        RuleFor(p => p.FlagTipoCambio)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.CodigoTipoTransaccion).NotEmpty().WithMessage("El campo {PropertyName} es requerido");
    }
}