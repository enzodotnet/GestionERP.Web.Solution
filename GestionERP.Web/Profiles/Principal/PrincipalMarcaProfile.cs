using AutoMapper;
using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Profiles.Principal;

public class PrincipalMarcaProfile : Profile
{
    public PrincipalMarcaProfile()
    {
        CreateMap<MarcaObtenerDto, MarcaEditarDto>();
    }
}