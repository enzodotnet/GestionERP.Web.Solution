using AutoMapper;
using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Profiles.Principal;

public class PrincipalCierreContableProfile : Profile
{
    public PrincipalCierreContableProfile()
    {
        CreateMap<CierreContableObtenerDto, CierreContableEditarDto>();
    }
}