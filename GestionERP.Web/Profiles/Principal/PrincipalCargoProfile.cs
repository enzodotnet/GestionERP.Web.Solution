using AutoMapper;
using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Profiles.Principal;

public class PrincipalCargoProfile : Profile
{
    public PrincipalCargoProfile()
    {
        CreateMap<CargoObtenerDto, CargoEditarDto>();
    }
}