using AutoMapper;
using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Profiles.Principal;

public class PrincipalZonaVentaProfile : Profile
{
    public PrincipalZonaVentaProfile()
    {
        CreateMap<ZonaVentaObtenerDto, ZonaVentaEditarDto>();
    }
}