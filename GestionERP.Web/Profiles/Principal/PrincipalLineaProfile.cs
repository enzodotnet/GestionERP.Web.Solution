using AutoMapper;
using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Profiles.Principal;

public class PrincipalLineaProfile : Profile
{
    public PrincipalLineaProfile()
    {
        CreateMap<SegmentoArticuloObtenerDto, SegmentoArticuloEditarDto>();
    }
}