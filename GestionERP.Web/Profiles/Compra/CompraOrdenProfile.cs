using AutoMapper;
using GestionERP.Web.Models.Dtos.Compra;

namespace GestionERP.Web.Profiles.Compra;

public class CompraOrdenProfile : Profile
{
    public CompraOrdenProfile()
    {
        CreateMap<OrdenObtenerDto, OrdenEditarDto>();

        CreateMap<OrdenDetalleObtenerDto, OrdenDetalleEditarDto>().ReverseMap();
        CreateMap<OrdenDetalleInsertarDto, OrdenDetalleObtenerDto>().ReverseMap();
        CreateMap<OrdenDetalleGrid, OrdenDetalleObtenerDto>().ReverseMap();

        CreateMap<SolicitudCatalogoAtenderDto, OrdenDetalleObtenerDto>()
            .ForMember(x => x.Observacion, opt => opt.Ignore())
            .ForMember(x => x.CodigoSolicitudReferencia, opt => opt.MapFrom( y => y.CodigoSolicitud)); 
    }
}