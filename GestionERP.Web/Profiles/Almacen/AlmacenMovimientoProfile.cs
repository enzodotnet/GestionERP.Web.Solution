using AutoMapper;
using GestionERP.Web.Models.Dtos.Almacen;
using GestionERP.Web.Models.Dtos.Compra;

namespace GestionERP.Web.Profiles.Almacen;

public class AlmacenMovimientoProfile : Profile
{
    public AlmacenMovimientoProfile()
    {
        CreateMap<OrdenCatalogoIngresarDto, MovimientoDetalleObtenerDto>();
        CreateMap<MovimientoDetalleInsertarDto, MovimientoDetalleObtenerDto>().ReverseMap();
        CreateMap<MovimientoDetalleGrid, MovimientoDetalleObtenerDto>().ReverseMap();
    }
}