using AutoMapper; 
using GestionERP.Web.Models.Dtos.Produccion;

namespace GestionERP.Web.Profiles.Produccion;

public class ProduccionOrdenProfile : Profile
{
    public ProduccionOrdenProfile()
    {
        CreateMap<OrdenObtenerDto, OrdenEditarDto>();
        CreateMap<OrdenLoteListarDto, OrdenLoteObtenerDto>();
        CreateMap<OrdenMaquilaListarDto, OrdenMaquilaListarDto>();
        CreateMap<OrdenTareoListarDto, OrdenTareoListarDto>();

        CreateMap<SolicitudCatalogoAtenderDto, OrdenInsertarDto>()
            .ForMember(x => x.FechaEmision, opt => opt.Ignore())
            .ForMember(x => x.CodigoSolicitudReferencia, opt => opt.MapFrom(y => y.CodigoSolicitud));
        CreateMap<SolicitudCatalogoAtenderDto, OrdenObtenerDto>();
        CreateMap<VersionPlanMaterialConsultaDto, OrdenMaterialObtenerDto>();
    }
}