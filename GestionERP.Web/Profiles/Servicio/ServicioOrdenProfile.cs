using AutoMapper;
using GestionERP.Web.Models.Dtos.Servicio;

namespace GestionERP.Web.Profiles.Servicio;

public class ServicioOrdenProfile : Profile
{
    public ServicioOrdenProfile()
    {
        CreateMap<OrdenObtenerDto, OrdenEditarDto>();

        CreateMap<OrdenDetalleObtenerDto, OrdenDetalleEditarDto>().ReverseMap();
        CreateMap<OrdenDetalleInsertarDto, OrdenDetalleObtenerDto>().ReverseMap();
        CreateMap<OrdenDetalleGrid, OrdenDetalleObtenerDto>().ReverseMap();
        CreateMap<OrdenCatalogoAceptarDto, OrdenObtenerAceptarDto>();

        CreateMap<SolicitudCatalogoAtenderDto, OrdenDetalleObtenerDto>()
            .ForMember(x => x.Observacion, opt => opt.Ignore())
            .ForMember(x => x.CodigoSolicitudReferencia, opt => opt.MapFrom( y => y.CodigoSolicitud)); 
	}
}