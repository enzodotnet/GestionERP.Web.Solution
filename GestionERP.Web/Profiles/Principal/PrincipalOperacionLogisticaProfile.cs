using AutoMapper;
using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Profiles.Principal;

public class PrincipalOperacionLogisticaProfile : Profile
{
    public PrincipalOperacionLogisticaProfile()
    {
        CreateMap<OperacionLogisticaObtenerDto, OperacionLogisticaEditarDto>();
    }
}