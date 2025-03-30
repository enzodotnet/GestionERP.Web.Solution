using AutoMapper;
using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Profiles.Principal;

public class PrincipalAreaProfile : Profile
{
    public PrincipalAreaProfile()
    {
        CreateMap<AreaObtenerDto, AreaEditarDto>();
    }
}