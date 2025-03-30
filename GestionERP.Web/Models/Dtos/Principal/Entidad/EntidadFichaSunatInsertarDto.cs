using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class EntidadFichaSunatInsertarDto
{ 
    public string NombreComercial { get; set; }
    public string ActividadPrincipal { get; set; }
    public string ActividadSecundariaN1 { get; set; }
    public string ActividadSecundariaN2 { get; set; }
    public string DescripcionTipoContribuyente { get; set; }
    public string EstadoContribuyente { get; set; } 
    public string FlagCondicionContribuyente { get; set; } 
    public bool EsAfectoNuevoRus { get; set; }
    public bool EsAgenteRetenedor { get; set; }
    public bool EsEmisorElectronico { get; set; }
    public DateTime? FechaInscripcion { get; set; }
    public DateTime? FechaInicioActividad { get; set; }
    public DateTime? FechaCondicionNohabido { get; set; }
}

public class EntidadFichaSunatInsertarValidator : AbstractValidator<EntidadFichaSunatInsertarDto>
{
    public EntidadFichaSunatInsertarValidator()
    {
        RuleFor(p => p.NombreComercial)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido en el insertar ficha sunat")
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres en el insertar ficha sunat");
        
        RuleFor(p => p.ActividadPrincipal)
            .MaximumLength(200).WithMessage("El campo {PropertyName} debe tener como máximo 200 caracteres en el insertar ficha sunat");

        RuleFor(p => p.ActividadSecundariaN1)
            .MaximumLength(200).WithMessage("El campo {PropertyName} debe tener como máximo 200 caracteres en el insertar ficha sunat");

        RuleFor(p => p.ActividadSecundariaN2)
            .MaximumLength(200).WithMessage("El campo {PropertyName} debe tener como máximo 200 caracteres en el insertar ficha sunat");

        RuleFor(p => p.DescripcionTipoContribuyente)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido en el insertar ficha sunat")
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres en el insertar ficha sunat");

        RuleFor(p => p.EstadoContribuyente)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido en el insertar ficha sunat")
            .MaximumLength(50).WithMessage("El campo {PropertyName} debe tener como máximo 50 caracteres en el insertar ficha sunat");
        
        RuleFor(p => p.FlagCondicionContribuyente)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido en el insertar ficha sunat");
    }
}