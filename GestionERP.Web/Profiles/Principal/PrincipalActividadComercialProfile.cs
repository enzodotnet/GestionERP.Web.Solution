using AutoMapper;
using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Profiles.Principal;

public class PrincipalActividadComercialProfile : Profile
{
    public PrincipalActividadComercialProfile()
    {
        CreateMap<ActividadComercialObtenerDto, ActividadComercialEditarDto>();
    }
}