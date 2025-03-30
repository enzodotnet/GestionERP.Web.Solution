using AutoMapper;
using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Profiles.Principal;

public class PrincipalOperacionFinancieraProfile : Profile
{
    public PrincipalOperacionFinancieraProfile()
    {
        CreateMap<OperacionFinancieraObtenerDto, OperacionFinancieraEditarDto>();
    }
}