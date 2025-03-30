using AutoMapper;
using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Profiles.Principal;

public class PrincipalGrupoArticuloProfile : Profile
{
    public PrincipalGrupoArticuloProfile()
    {
        CreateMap<GrupoArticuloObtenerDto, GrupoArticuloEditarDto>();
    }
}