using Telerik.Blazor.Services;
using GestionERP.Web.Resources;
using Telerik.ReportViewer.BlazorNative.Services;

namespace GestionERP.Web.Services;
public class ReportingResxLocalizer : ITelerikReportingStringLocalizer
{
    public string this[string name]
    {
        get
        {
            return GetStringFromResource(name);
        }
    }
    public static string GetStringFromResource(string key)
    {
        return ReportViewerMessages.ResourceManager.GetString(key, ReportViewerMessages.Culture)!;
    }
}
