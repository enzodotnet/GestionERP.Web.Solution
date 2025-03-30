using AutoMapper;
using GestionERP.Web.Models.Dtos.Servicio;

namespace GestionERP.Web.Profiles.Servicio;

public class ServicioContratoProfile : Profile
{
    public ServicioContratoProfile()
    {
        CreateMap<ContratoObtenerDto, ContratoEditarDto>();

        CreateMap<ContratoDetalleObtenerDto, ContratoDetalleEditarDto>().ReverseMap();
        CreateMap<ContratoDetalleInsertarDto, ContratoDetalleObtenerDto>().ReverseMap();
        CreateMap<ContratoDetalleGrid, ContratoDetalleObtenerDto>().ReverseMap();

        CreateMap<ContratoCuotaObtenerDto, ContratoCuotaEditarDto>().ReverseMap();
        CreateMap<ContratoCuotaInsertarDto, ContratoCuotaObtenerDto>().ReverseMap();
        CreateMap<ContratoCuotaGrid, ContratoCuotaObtenerDto>().ReverseMap();

        CreateMap<ContratoTerminoObtenerDto, ContratoTerminoEditarDto>().ReverseMap();
        CreateMap<ContratoTerminoInsertarDto, ContratoTerminoObtenerDto>().ReverseMap();
        CreateMap<ContratoTerminoGrid, ContratoTerminoObtenerDto>().ReverseMap();
    }
}