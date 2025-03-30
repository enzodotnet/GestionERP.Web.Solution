namespace GestionERP.Web.Models.Dtos.Principal;

public class ModuloCatalogoDto
{
    public string CodigoModulo { get; set; }
    public string NombreModulo { get; set; }
    public string FlagTipoAcceso { get; set; }

    /// <summary>
    /// Esta propiedad no es editable, es un campo condicional en accesos a usuario.
    /// </summary>
    public bool EsAsignado { get; set; }

    /// <summary>
    /// Esta propiedad no es editable, es un campo condicional en accesos a usuario.
    /// </summary>
    public bool EsBloqueado { get; set; }
}