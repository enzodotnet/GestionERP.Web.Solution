using AutoMapper;
using GestionERP.Web.Models.Dtos.Importacion;

namespace GestionERP.Web.Profiles.Importacion;

public class ImportacionSolicitudProfile : Profile
{
    public ImportacionSolicitudProfile()
    {
        CreateMap<SolicitudObtenerDto, SolicitudEditarDto>();

        CreateMap<SolicitudDetalleObtenerDto, SolicitudDetalleEditarDto>().ReverseMap();
        CreateMap<SolicitudDetalleInsertarDto, SolicitudDetalleObtenerDto>().ReverseMap();
        CreateMap<SolicitudDetalleGrid, SolicitudDetalleObtenerDto>().ReverseMap();
    }
}