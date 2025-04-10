using AutoMapper;
using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Profiles.Principal;

public class PrincipalVehiculoProfile : Profile
{
    public PrincipalVehiculoProfile()
    {
        CreateMap<VehiculoObtenerDto, VehiculoEditarDto>().ReverseMap();
    }
}