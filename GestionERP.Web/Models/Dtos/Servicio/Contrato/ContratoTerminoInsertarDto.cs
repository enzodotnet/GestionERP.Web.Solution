using FluentValidation;
using System.Text.Json.Serialization;

namespace GestionERP.Web.Models.Dtos.Servicio;

public class ContratoTerminoInsertarDto
{
    [JsonIgnore]
    public int? NumeroTermino { get; set; }
    public string CodigoTipoTermino { get; set; } 
    public string Descripcion { get; set; }
}

public class ContratoTerminoInsertarValidator : AbstractValidator<ContratoTerminoInsertarDto>
{
    public string MsgErrorTipoTermino { get; set; }

    public ContratoTerminoInsertarValidator()
    {
        RuleFor(p => p.CodigoTipoTermino)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .Matches("^[A-Za-z]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfabéticos")
            .Must(x => string.IsNullOrEmpty(MsgErrorTipoTermino)).WithMessage(x => MsgErrorTipoTermino);

        RuleFor(p => p.Descripcion)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .MaximumLength(500).WithMessage("El campo {PropertyName} debe tener como máximo 500 caracteres");
    }
}