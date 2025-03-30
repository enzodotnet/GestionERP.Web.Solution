using AutoMapper;
using GestionERP.Web.Models.Dtos.Importacion;

namespace GestionERP.Web.Profiles.Importacion;

public class ImportacionOrdenProfile : Profile
{
    public ImportacionOrdenProfile()
    {
        CreateMap<OrdenObtenerDto, OrdenEditarDto>();

        CreateMap<OrdenDetalleObtenerDto, OrdenDetalleEditarDto>().ReverseMap();
        CreateMap<OrdenDetalleInsertarDto, OrdenDetalleObtenerDto>().ReverseMap();
        CreateMap<OrdenDetalleGrid, OrdenDetalleObtenerDto>().ReverseMap();

		CreateMap<OrdenNotaObtenerDto, OrdenNotaEditarDto>().ReverseMap();
		CreateMap<OrdenNotaInsertarDto, OrdenNotaObtenerDto>().ReverseMap();
		CreateMap<OrdenNotaGrid, OrdenNotaObtenerDto>().ReverseMap();

	    CreateMap<SolicitudCatalogoAtenderDto, OrdenDetalleObtenerDto>()
            .ForMember(x => x.Observacion, opt => opt.Ignore())
            .ForMember(x => x.CodigoSolicitudReferencia, opt => opt.MapFrom( y => y.CodigoSolicitud)); 
	}
}