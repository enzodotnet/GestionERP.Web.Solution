using AutoMapper;
using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Profiles.Principal;

public class PrincipalTipoProduccionProfile : Profile
{
    public PrincipalTipoProduccionProfile()
    {
        CreateMap<TipoProduccionObtenerDto, TipoProduccionEditarDto>(); 
    }
}