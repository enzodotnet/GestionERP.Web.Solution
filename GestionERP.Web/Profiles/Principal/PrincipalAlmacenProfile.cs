using AutoMapper;
using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Profiles.Principal;

public class PrincipalAlmacenProfile : Profile
{
    public PrincipalAlmacenProfile()
    {
        CreateMap<AlmacenObtenerDto, AlmacenEditarDto>();
    }
}