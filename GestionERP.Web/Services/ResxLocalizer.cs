using Telerik.Blazor.Services;
using GestionERP.Web.Resources;

namespace GestionERP.Web.Services;
public class ResxLocalizer : ITelerikStringLocalizer
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
        return TelerikMessages.ResourceManager.GetString(key, TelerikMessages.Culture)!;
    }
}
