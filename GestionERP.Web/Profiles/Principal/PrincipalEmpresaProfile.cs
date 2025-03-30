using AutoMapper;
using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Profiles.Principal;

public class PrincipalEmpresaProfile : Profile
{
    public PrincipalEmpresaProfile()
    {
        CreateMap<EmpresaObtenerDto, EmpresaEditarDto>();
        CreateMap<EmpresaAtributoObtenerDto, EmpresaAtributoEditarDto>();
        CreateMap<EmpresaFacturacionObtenerDto, EmpresaFacturacionEditarDto>();
    }
}