using GestionERP.Web.Models.Dtos.Report;

namespace GestionERP.Web.Services.Interfaces;

public interface IReport
{
    Task Print(ReportPrintDto reportPrint); 
}