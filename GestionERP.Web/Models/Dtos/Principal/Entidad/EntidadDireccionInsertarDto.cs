using FluentValidation;
using System.Text.Json.Serialization;

namespace GestionERP.Web.Models.Dtos.Principal;

public class EntidadDireccionInsertarDto
{ 
    /// <summary>
    /// Esta propiedad no es editable, es un campo clave temporal
    /// </summary>
    [JsonIgnore]
    public int NumeroItem { get; set; }
    public string Nombre { get; set; }
    public string CodigoDistrito { get; set; }
    public string Referencia { get; set; } 
    public decimal? Latitud { get; set; }
    public decimal? Longitud { get; set; }
    public bool EsOficina { get; set; }
    public bool EsPuntoLlegada { get; set; }
    public bool EsNormal { get; set; }
    public bool EsPredeterminado { get; set; }
}

public class EntidadDireccionInsertarValidator : AbstractValidator<EntidadDireccionInsertarDto>
{
    public EntidadDireccionInsertarValidator()
    {
        RuleFor(p => p.CodigoDistrito).NotEmpty().WithMessage("El campo {PropertyName} es requerido en el insertar dirección");
        
        RuleFor(p => p.Nombre)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido en el insertar dirección") 
            .MaximumLength(200).WithMessage("El campo {PropertyName} debe tener como máximo 200 caracteres en el insertar dirección");

        RuleFor(p => p.Referencia)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres en el insertar dirección");

        RuleFor(p => p.Latitud)
            .PrecisionScale(9, 6, true);
        
        RuleFor(p => p.Longitud)
            .PrecisionScale(9, 6, true);
    }
}