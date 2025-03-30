using AutoMapper;
using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Profiles.Principal;

public class PrincipalPermisoProfile : Profile
{
    public PrincipalPermisoProfile()
    {
        CreateMap<PermisoObtenerDto, PermisoEditarDto>().ReverseMap();
    }
}