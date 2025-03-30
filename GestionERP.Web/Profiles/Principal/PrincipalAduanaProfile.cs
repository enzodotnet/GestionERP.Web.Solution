using AutoMapper;
using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Profiles.Principal;

public class PrincipalAduanaProfile : Profile
{
    public PrincipalAduanaProfile()
    {
        CreateMap<AduanaObtenerDto, AduanaEditarDto>();
    }
}