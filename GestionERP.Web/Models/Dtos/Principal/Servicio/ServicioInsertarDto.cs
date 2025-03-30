using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class ServicioInsertarDto
{ 
    public string Codigo { get; set; }
    public string CodigoModulo { get; set; }
    public string Nombre { get; set; }
    public string FlagTipoFuncion { get; set; }
    public bool EsAdjuntableArchivo { get; set; }
    public string Descripcion { get; set; }
}

public class ServicioInsertarValidator : AbstractValidator<ServicioInsertarDto>
{
    public ServicioInsertarValidator()
    {
        RuleFor(p => p.CodigoModulo).NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        When(p => !string.IsNullOrEmpty(p.CodigoModulo), () => {
            RuleFor(p => p.Codigo)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
                .Length(4).WithMessage("El campo {PropertyName} debe tener 4 caracteres")
                .Matches("^[0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres numéricos");
        });

        RuleFor(p => p.Nombre)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres")
            .Matches(@"^[^""!@$%^&*(){}:;<>,.?/+_=|'~\\-]*$").WithMessage("El campo {PropertyName} no debe contener caracteres especiales");
        
         RuleFor(p => p.FlagTipoFuncion)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido");
    
        RuleFor(p => p.Descripcion)
            .MaximumLength(200).WithMessage("El campo {PropertyName} debe tener como máximo 200 caracteres");
    }
}