using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class SerieDocumentoInsertarDto
{ 
    public string Serie { get; set; }
    public int SecuenciaNumero { get; set; } = 1;
    public int NumeroInicio { get; set; } = 1;
    public int NumeroFinal { get; set; } = 99999999;
    public string Nombre { get; set; }
    public string CodigoProcesoDocumento { get; set; } 
    public string Descripcion { get; set; }
    public string CodigoLocal { get; set; }
    public string RutaHostImpresora { get; set; } 
    public string NombreImpresora { get; set; } 
}

public class SerieDocumentoInsertarValidator : AbstractValidator<SerieDocumentoInsertarDto>
{
    public SerieDocumentoInsertarValidator()
    {
        RuleFor(p => p.CodigoProcesoDocumento).NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.Serie)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .Length(4).WithMessage("El campo {PropertyName} debe tener 4 caracteres")
            .Matches("^[A-Za-z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos");
        
        RuleFor(p => p.SecuenciaNumero)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .GreaterThan(0).WithMessage("El campo {PropertyName} debe ser mayor a cero");

        RuleFor(p => p.NumeroInicio)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .GreaterThan(0).WithMessage("El campo {PropertyName} debe ser mayor a cero");

        RuleFor(p => p.NumeroFinal)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .GreaterThan(0).WithMessage("El campo {PropertyName} debe ser mayor a cero");

        RuleFor(p => p.Nombre)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");

        RuleFor(p => p.Descripcion)
            .MaximumLength(200).WithMessage("El campo {PropertyName} debe tener como máximo 200 caracteres");

        RuleFor(p => p.RutaHostImpresora)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
        
        RuleFor(p => p.NombreImpresora)
            .MaximumLength(50).WithMessage("El campo {PropertyName} debe tener como máximo 50 caracteres");
    }
}