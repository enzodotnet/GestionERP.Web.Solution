using AutoMapper;
using GestionERP.Web.Models.Dtos.Produccion;

namespace GestionERP.Web.Profiles.Produccion;

public class ProduccionVersionPlanProfile : Profile
{
    public ProduccionVersionPlanProfile()
    {
        CreateMap<VersionPlanObtenerDto, VersionPlanEditarDto>();

        CreateMap<VersionPlanEquipoObtenerDto, VersionPlanEquipoEditarDto>().ReverseMap();
        CreateMap<VersionPlanEquipoInsertarDto, VersionPlanEquipoObtenerDto>().ReverseMap();  

        CreateMap<VersionPlanFuncionObtenerDto, VersionPlanFuncionEditarDto>().ReverseMap();
        CreateMap<VersionPlanFuncionInsertarDto, VersionPlanFuncionObtenerDto>().ReverseMap(); 

        CreateMap<VersionPlanMaterialObtenerDto, VersionPlanMaterialEditarDto>().ReverseMap();
        CreateMap<VersionPlanMaterialInsertarDto, VersionPlanMaterialObtenerDto>().ReverseMap(); 
    }
}