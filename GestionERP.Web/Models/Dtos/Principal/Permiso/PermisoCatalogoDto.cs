namespace GestionERP.Web.Models.Dtos.Principal;

public class PermisoCatalogoDto
{  
    public string CodigoPermiso { get; set; }
    public string NombrePermiso { get; set; } 
    public string CodigoServicio { get; set; }
    public string NombreServicio { get; set; }
    public string CodigoModulo { get; set; }
    public string NombreModulo { get; set; }

    /// <summary>
    /// Esta propiedad no es editable, es un campo condicional en accesos a nuevo usuario.
    /// </summary>
    public bool EsAsignado { get; set; }

    /// <summary>
    /// Esta propiedad no es editable, es un campo condicional en accesos a nuevo usuario.
    /// </summary>
    public bool EsBloqueado { get; set; }

    /// <summary>
    /// Esta propiedad no es editable, es un campo condicional en accesos a nuevo usuario.
    /// </summary>
    public bool EsAsignadoServicio { get; set; }
}