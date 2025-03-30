using AutoMapper;
using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Profiles.Principal;

public class PrincipalTipoFacturaNegociableProfile : Profile
{
    public PrincipalTipoFacturaNegociableProfile()
    {
        CreateMap<TipoSituacionLetraObtenerDto, TipoSituacionLetraEditarDto>(); 
    }
}