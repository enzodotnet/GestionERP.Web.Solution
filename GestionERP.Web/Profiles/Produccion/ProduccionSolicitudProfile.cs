using AutoMapper;
using GestionERP.Web.Models.Dtos.Produccion;

namespace GestionERP.Web.Profiles.Produccion;

public class ProduccionSolicitudProfile : Profile
{
    public ProduccionSolicitudProfile()
    {
        CreateMap<SolicitudObtenerDto, SolicitudEditarDto>();
    }
}