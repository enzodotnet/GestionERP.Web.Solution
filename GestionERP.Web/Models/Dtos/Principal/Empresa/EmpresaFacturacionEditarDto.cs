using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class EmpresaFacturacionEditarDto
{  
    public int? UbicacionCertificado { get; set; }
    public string NumeroSerieCertificado { get; set; }
    public string UsuarioSOL { get; set; }
    public string ClaveSOL { get; set; }
    public string UsuarioOSE { get; set; }
    public string ClaveOSE { get; set; }
    public string NumeroResolucion { get; set; }
    public bool EnviaCorreo { get; set; }
    public string HostSMTP { get; set; }
    public string PuertoSMTP { get; set; }
    public bool CertificadoSSL { get; set; }
    public string EmailUsuarioSMTP { get; set; }
    public string PasswordSMTP { get; set; }
    public string PaginaWebConsultaCDR { get; set; }
    public string PaginaWebDescargaXML { get; set; }
    public string ColorPrincipalPDF { get; set; }
    public string ColorSecundarioPDF { get; set; }
    public string ColorAlternoPDF { get; set; }
    public string RutaUbicacionLogoPDF { get; set; }
    public string RutaBoletaXML { get; set; }
    public string RutaFacturaXML { get; set; }
    public string RutaNotaCreditoXML { get; set; }
    public string RutaNotaDebitoXML { get; set; }
    public string RutaResumenDiarioXML { get; set; }
    public string RutaComunicacionBajaXML { get; set; }
    public string RutaRetencionXML { get; set; }
    public string RutaResumenReversionXML { get; set; }
    public string RutaTicketXML { get; set; }
    public string RutaGuiaRemisionXML { get; set; }
}

public class EmpresaFacturacionEditarValidator : AbstractValidator<EmpresaFacturacionEditarDto>
{
    public EmpresaFacturacionEditarValidator()
    { 
        RuleFor(p => p.UbicacionCertificado)
            .GreaterThanOrEqualTo(0).WithMessage("El campo {PropertyName} debe ser mayor o igual a 0 (cero) en el editar facturacion");

        RuleFor(p => p.NumeroSerieCertificado)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener máximo 100 caracteres en el editar facturacion");
        
        RuleFor(p => p.UsuarioSOL)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener máximo 100 caracteres en el editar facturacion");
        
        RuleFor(p => p.ClaveSOL)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener máximo 100 caracteres en el editar facturacion");
        
        RuleFor(p => p.UsuarioOSE)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener máximo 100 caracteres en el editar facturacion");
        
        RuleFor(p => p.ClaveOSE)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener máximo 100 caracteres en el editar facturacion");
        
        RuleFor(p => p.NumeroResolucion)
            .MaximumLength(50).WithMessage("El campo {PropertyName} debe tener máximo 50 caracteres en el editar facturacion");
        
        RuleFor(p => p.HostSMTP)  
            .MaximumLength(20).WithMessage("El campo {PropertyName} debe tener máximo 20 caracteres en el editar facturacion");
        
        RuleFor(p => p.PuertoSMTP)  
            .MaximumLength(4).WithMessage("El campo {PropertyName} debe tener máximo 4 caracteres en el editar facturacion");
        
        RuleFor(p => p.EmailUsuarioSMTP)  
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener máximo 100 caracteres en el editar facturacion");
        
        RuleFor(p => p.PasswordSMTP)  
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener máximo 100 caracteres en el editar facturacion");
        
        RuleFor(p => p.PaginaWebConsultaCDR)  
            .MaximumLength(50).WithMessage("El campo {PropertyName} debe tener máximo 50 caracteres en el editar facturacion");
        
        RuleFor(p => p.PaginaWebDescargaXML)  
            .MaximumLength(50).WithMessage("El campo {PropertyName} debe tener máximo 50 caracteres en el editar facturacion");
        
        RuleFor(p => p.ColorPrincipalPDF)
            .MaximumLength(7).WithMessage("El campo {PropertyName} debe tener máximo 7 caracteres en el editar facturacion");
        
        RuleFor(p => p.ColorSecundarioPDF)
            .MaximumLength(7).WithMessage("El campo {PropertyName} debe tener máximo 7 caracteres en el editar facturacion");
        
        RuleFor(p => p.ColorAlternoPDF)
            .MaximumLength(7).WithMessage("El campo {PropertyName} debe tener máximo 7 caracteres en el editar facturacion");
        
        RuleFor(p => p.RutaUbicacionLogoPDF)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener máximo 100 caracteres en el editar facturacion");

        RuleFor(p => p.RutaBoletaXML)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener máximo 100 caracteres en el editar facturacion");
        
        RuleFor(p => p.RutaFacturaXML)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener máximo 100 caracteres en el editar facturacion");

        RuleFor(p => p.RutaNotaCreditoXML)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener máximo 100 caracteres en el editar facturacion");
        
        RuleFor(p => p.RutaNotaDebitoXML)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener máximo 100 caracteres en el editar facturacion");
        
        RuleFor(p => p.RutaResumenDiarioXML)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener máximo 100 caracteres en el editar facturacion");
        
        RuleFor(p => p.RutaComunicacionBajaXML)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener máximo 100 caracteres en el editar facturacion");
        
        RuleFor(p => p.RutaRetencionXML)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener máximo 100 caracteres en el editar facturacion");
        
        RuleFor(p => p.RutaResumenReversionXML)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener máximo 100 caracteres en el editar facturacion");
        
        RuleFor(p => p.RutaTicketXML)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener máximo 100 caracteres en el editar facturacion");
        
        RuleFor(p => p.RutaGuiaRemisionXML)
            .MaximumLength(100).WithMessage("El campo {PropertyName} debe tener máximo 100 caracteres en el editar facturacion");
    }
}