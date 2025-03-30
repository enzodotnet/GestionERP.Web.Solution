namespace GestionERP.Web.Models.Dtos.Report;

public class ReportPrintDto
{
    public string ReportName { get; set; }
    public string PrinterHost {  get; set; }
    public int? CopieNumbers { get; set; }
    public IDictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();
}