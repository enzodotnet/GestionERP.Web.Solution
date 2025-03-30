using AutoMapper;
using GestionERP.Web.Models.Dtos.Servicio;

namespace GestionERP.Web.Profiles.Servicio;

public class ServicioSolicitudProfile : Profile
{
    public ServicioSolicitudProfile()
    {
        CreateMap<SolicitudObtenerDto, SolicitudEditarDto>();

        CreateMap<SolicitudDetalleObtenerDto, SolicitudDetalleEditarDto>().ReverseMap();
        CreateMap<SolicitudDetalleInsertarDto, SolicitudDetalleObtenerDto>().ReverseMap();
        CreateMap<SolicitudDetalleGrid, SolicitudDetalleObtenerDto>().ReverseMap();
    }
}