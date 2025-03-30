using AutoMapper;
using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Profiles.Principal;

public class PrincipalGlosarioAnalisisProfile : Profile
{
    public PrincipalGlosarioAnalisisProfile()
    {
        CreateMap<GlosarioAnalisisObtenerDto, GlosarioAnalisisEditarDto>();
    }
}