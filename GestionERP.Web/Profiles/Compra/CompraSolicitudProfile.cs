using AutoMapper;
using GestionERP.Web.Models.Dtos.Compra;

namespace GestionERP.Web.Profiles.Compra;

public class CompraSolicitudProfile : Profile
{
    public CompraSolicitudProfile()
    {
        CreateMap<SolicitudObtenerDto, SolicitudEditarDto>();

        CreateMap<SolicitudDetalleObtenerDto, SolicitudDetalleEditarDto>().ReverseMap();
        CreateMap<SolicitudDetalleInsertarDto, SolicitudDetalleObtenerDto>().ReverseMap();
        CreateMap<SolicitudDetalleGrid, SolicitudDetalleObtenerDto>().ReverseMap();
    }
}