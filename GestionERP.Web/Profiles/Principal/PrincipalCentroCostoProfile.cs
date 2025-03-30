using AutoMapper;
using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Profiles.Principal;

public class PrincipalCentroCostoProfile : Profile
{
    public PrincipalCentroCostoProfile()
    {
        CreateMap<CentroCostoObtenerDto, CentroCostoEditarDto>();
    }
}