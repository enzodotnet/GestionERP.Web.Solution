using AutoMapper;
using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Profiles.Principal;

public class PrincipalTipoDevengoProfile : Profile
{
    public PrincipalTipoDevengoProfile()
    {
        CreateMap<TipoDevengoObtenerDto, TipoDevengoEditarDto>();
    }
}