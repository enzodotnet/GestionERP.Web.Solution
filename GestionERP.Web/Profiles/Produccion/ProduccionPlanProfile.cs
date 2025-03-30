using AutoMapper;
using GestionERP.Web.Models.Dtos.Produccion;

namespace GestionERP.Web.Profiles.Produccion;

public class ProduccionPlanProfile : Profile
{
    public ProduccionPlanProfile()
    {
        CreateMap<PlanObtenerDto, PlanEditarDto>();
    }
}