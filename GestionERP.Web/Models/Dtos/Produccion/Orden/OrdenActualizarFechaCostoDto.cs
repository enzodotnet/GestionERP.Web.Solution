using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Produccion;

public class OrdenActualizarFechaCostoDto
{
    public Guid OrdenId { get; set; } 
    private DateTime? fechaCosto;
    public DateTime? FechaCosto
    { 
        get { return fechaCosto?.Date; }
        set { fechaCosto = value; }
    }
    public string CodigoTipoCambioDia { get; set; }
    public decimal? MontoTipoCambioDia { get; set; }
}

public class OrdenActualizarFechaCostoValidator : AbstractValidator<OrdenActualizarFechaCostoDto>
{
    public OrdenActualizarFechaCostoValidator()
    {    
        RuleFor(p => p.FechaCosto).NotNull().WithMessage("El campo {PropertyName} es requerido");
    }
}