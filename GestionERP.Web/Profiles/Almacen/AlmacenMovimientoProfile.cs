using AutoMapper;
using GestionERP.Web.Models.Dtos.Almacen;
using GestionERP.Web.Models.Dtos.Compra;

namespace GestionERP.Web.Profiles.Almacen;

public class AlmacenMovimientoProfile : Profile
{
    public AlmacenMovimientoProfile()
    {

        CreateMap<OrdenCatalogoIngresarDto, MovimientoInsertarDto>() 
            .ForMember(x => x.CodigoDocumentoReferencia, opt => opt.MapFrom(y => y.CodigoDocumento))
            .ForMember(x => x.SerieDocumentoReferencia, opt => opt.MapFrom(y => y.CodigoSerieDocumento))
            .ForMember(x => x.NumeroDocumentoReferencia, opt => opt.MapFrom(y => y.NumeroSerieDocumento))
            .ForMember(x => x.CodigoCentroCosto, opt => opt.MapFrom(y => y.CodigoCentroCostoOrden))
            .ForMember(x => x.CodigoLocal, opt => opt.MapFrom(y => y.CodigoLocalRecepcion));

        CreateMap<OrdenCatalogoIngresarDto, MovimientoObtenerDto>()
            .ForMember(x => x.CodigoCentroCosto, opt => opt.MapFrom(y => y.CodigoCentroCostoOrden))
            .ForMember(x => x.NombreCentroCosto, opt => opt.MapFrom(y => y.NombreCentroCostoOrden))
            .ForMember(x => x.CodigoLocal, opt => opt.MapFrom(y => y.CodigoLocalRecepcion))
            .ForMember(x => x.NombreLocal, opt => opt.MapFrom(y => y.NombreLocalRecepcion))
            .ForMember(x => x.DocumentoReferencia, opt => opt.MapFrom(y => y.CodigoOrden));

        CreateMap<OrdenCatalogoIngresarDto, MovimientoDetalleObtenerDto>();
        CreateMap<MovimientoDetalleInsertarDto, MovimientoDetalleObtenerDto>().ReverseMap();
        CreateMap<MovimientoDetalleGrid, MovimientoDetalleObtenerDto>().ReverseMap();
    }
}