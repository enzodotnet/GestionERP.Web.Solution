using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class EntidadInsertarDto
{ 
    public string Codigo { get; set; }
    public string FlagTipoPersona { get; set; }
    public string CodigoTipoIdentificacion { get; set; } 
    public string Nombre { get; set; } 
    public string CodigoPais { get; set; }
    public string CodigoActividadComercial { get; set; }
    public bool EsTransportista { get; set; }
    public bool EsSeguro { get; set; }
    public bool EsBanco { get; set; }
    public bool EsAFP { get; set; }
    public EntidadFichaSunatInsertarDto FichaSunat { get; set; }
    public List<EntidadRepresentanteInsertarDto> Representantes { get; set; }
    public List<EntidadDireccionInsertarDto> Direcciones { get; set; }
    public List<EntidadVehiculoInsertarDto> Vehiculos { get; set; }
}

public class EntidadInsertarValidator : AbstractValidator<EntidadInsertarDto>
{
    public EntidadInsertarValidator()
    {
        RuleFor(p => p.Codigo)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .MaximumLength(11).WithMessage("El campo {PropertyName} debe tener como máximo 11 caracteres") 
            .Matches("^[0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres numericos");
        
        RuleFor(p => p.FlagTipoPersona)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.Nombre)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");

        RuleFor(p => p.CodigoTipoIdentificacion).NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.CodigoPais).NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        When(p => (p.FlagTipoPersona ?? "" ) == "JU", () => {
            RuleFor(p => p.FichaSunat).SetValidator(new EntidadFichaSunatInsertarValidator());
        });
    }
}