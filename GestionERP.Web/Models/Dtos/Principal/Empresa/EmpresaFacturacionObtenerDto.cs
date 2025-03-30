using FluentValidation;

namespace GestionERP.Web.Models.Dtos.Principal;

public class EmpresaFacturacionObtenerDto
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