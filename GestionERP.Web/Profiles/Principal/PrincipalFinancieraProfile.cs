using AutoMapper;
using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Profiles.Principal;

public class PrincipalFinancieraProfile : Profile
{
    public PrincipalFinancieraProfile()
    {
        CreateMap<FinancieraObtenerDto, FinancieraEditarDto>();
    }
}