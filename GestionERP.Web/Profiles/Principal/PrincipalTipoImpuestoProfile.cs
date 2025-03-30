using AutoMapper;
using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Profiles.Principal;

public class PrincipalTipoImpuestoProfile : Profile
{
    public PrincipalTipoImpuestoProfile()
    {
        CreateMap<TipoImpuestoObtenerDto, TipoImpuestoEditarDto>().ReverseMap(); 
    }
}