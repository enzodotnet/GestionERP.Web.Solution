using AutoMapper;
using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Profiles.Principal;

public class PrincipalLocalProfile : Profile
{
    public PrincipalLocalProfile()
    {
        CreateMap<LocalObtenerDto, LocalEditarDto>();
    }
}