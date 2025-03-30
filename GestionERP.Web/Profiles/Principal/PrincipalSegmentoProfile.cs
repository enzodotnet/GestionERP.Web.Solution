using AutoMapper;
using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Profiles.Principal;

public class PrincipalSegmentoProfile : Profile
{
    public PrincipalSegmentoProfile()
    {
        CreateMap<LineaArticuloObtenerDto, LineaArticuloEditarDto>();
    }
}