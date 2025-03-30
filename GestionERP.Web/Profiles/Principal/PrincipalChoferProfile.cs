using AutoMapper;
using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Profiles.Principal;

public class PrincipalChoferProfile : Profile
{
    public PrincipalChoferProfile()
    {
        CreateMap<ChoferObtenerDto, ChoferEditarDto>();
    }
}