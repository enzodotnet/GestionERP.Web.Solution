using AutoMapper;
using GestionERP.Web.Models.Dtos.Importacion;

namespace GestionERP.Web.Profiles.Importacion;

public class ImportacionPedidoProfile : Profile
{
    public ImportacionPedidoProfile()
    {
        CreateMap<PedidoObtenerDto, PedidoEditarDto>();

		CreateMap<PedidoDetalleObtenerDto, PedidoDetalleEditarDto>().ReverseMap();
        CreateMap<PedidoDetalleInsertarDto, PedidoDetalleObtenerDto>().ReverseMap();
        CreateMap<PedidoDetalleGrid, PedidoDetalleObtenerDto>().ReverseMap();

		CreateMap<OrdenCatalogoAtenderDto, PedidoInsertarDto>().ForMember(x => x.FechaEmision, opt => opt.Ignore());
		CreateMap<OrdenCatalogoAtenderDto, PedidoObtenerDto>();
		CreateMap<OrdenDetalleCatalogoAtenderDto, PedidoDetalleObtenerDto>();
    }
}