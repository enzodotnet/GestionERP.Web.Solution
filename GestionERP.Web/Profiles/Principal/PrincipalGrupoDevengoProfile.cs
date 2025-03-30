using AutoMapper;
using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Profiles.Principal;

public class PrincipalGrupoDevengoProfile : Profile
{
    public PrincipalGrupoDevengoProfile()
    {
        CreateMap<GrupoDevengoObtenerDto, GrupoDevengoEditarDto>();
    }
}