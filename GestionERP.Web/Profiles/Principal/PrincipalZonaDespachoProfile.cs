using AutoMapper;
using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Profiles.Principal;

public class PrincipalZonaDespachoProfile : Profile
{
    public PrincipalZonaDespachoProfile()
    {
        CreateMap<ZonaDespachoObtenerDto, ZonaDespachoEditarDto>();
    }
}