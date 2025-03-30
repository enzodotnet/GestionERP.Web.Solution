using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class TipoFacturaNegociableInsertarDto
{
    public string CodigoEmpresa { get; set; }
    public string Codigo { get; set; }
    public string Nombre { get; set; }
    public int CuentaBancariaId { get; set; }
    public string NumeroProductoBanco { get; set; }
    public int? DocumentoGeneraId { get; set; }
    public string Descripcion { get; set; }
}

public class TipoFacturaNegociableInsertarValidator : AbstractValidator<TipoFacturaNegociableInsertarDto>
{
    public TipoFacturaNegociableInsertarValidator()
    {
        RuleFor(p => p.CodigoEmpresa)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.Codigo)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .Length(2).WithMessage("El campo {PropertyName} debe tener 2 caracteres")
            .Matches("^[A-Z0-9]*$").WithMessage("El campo {PropertyName} solo debe contener caracteres alfanuméricos");

        RuleFor(p => p.CuentaBancariaId)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido");

        RuleFor(p => p.Nombre)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .MaximumLength(50).WithMessage("El campo {PropertyName} debe tener como máximo 50 caracteres");

        RuleFor(p => p.NumeroProductoBanco)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido")
            .MaximumLength(10).WithMessage("El campo {PropertyName} debe tener como máximo 10 caracteres");

        RuleFor(p => p.Descripcion)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
    }
}