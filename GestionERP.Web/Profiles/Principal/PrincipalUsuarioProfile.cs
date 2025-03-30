using AutoMapper;
using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Profiles.Principal;

public class PrincipalUsuarioProfile : Profile
{
    public PrincipalUsuarioProfile()
    {
        CreateMap<UsuarioObtenerDto, UsuarioEditarDto>().ReverseMap(); 
    }
}