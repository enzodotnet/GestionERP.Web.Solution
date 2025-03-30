using AutoMapper;
using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Profiles.Principal;

public class PrincipalDiarioProfile : Profile
{
    public PrincipalDiarioProfile()
    {
        CreateMap<DiarioObtenerDto, DiarioEditarDto>();
    }
}