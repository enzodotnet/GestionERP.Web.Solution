using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class EntidadEditarDto
{
    public string Nombre { get; set; }
    public string CodigoActividadComercial { get; set; }
    public bool EsTransportista { get; set; }
    public bool EsSeguro { get; set; }
    public bool EsBanco { get; set; }
    public bool EsAFP { get; set; }
    public bool Activo { get; set; }
    public EntidadFichaSunatEditarDto FichaSunatEditar { get; set; }
    public List<EntidadRepresentanteEliminarDto> RepresentantesEliminar { get; set; } = [];
    public List<EntidadRepresentanteEditarDto> RepresentantesEditar { get; set; } = [];
    public List<EntidadRepresentanteInsertarDto> RepresentantesInsertar { get; set; } = [];
    public List<EntidadDireccionEliminarDto> DireccionesEliminar { get; set; } = [];
    public List<EntidadDireccionEditarDto> DireccionesEditar { get; set; } = [];
    public List<EntidadDireccionInsertarDto> DireccionesInsertar { get; set; } = [];
    public List<EntidadVehiculoEliminarDto> VehiculosEliminar { get; set; } = [];
    public List<EntidadVehiculoEditarDto> VehiculosEditar { get; set; } = [];
    public List<EntidadVehiculoInsertarDto> VehiculosInsertar { get; set; } = [];
}

public class EntidadEditarValidator : AbstractValidator<EntidadEditarDto>
{
    public EntidadEditarValidator()
    { 
        RuleFor(p => p.Nombre)
            .NotEmpty().WithMessage("El campo {PropertyName} es requerido") 
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener como máximo 100 caracteres");
 
        RuleFor(p => p.FichaSunatEditar).SetValidator(new EntidadFichaSunatEditarValidator());
    }
}