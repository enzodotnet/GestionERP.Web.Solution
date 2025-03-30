using AutoMapper;
using GestionERP.Web.Models.Dtos.Produccion;

namespace GestionERP.Web.Profiles.Produccion;

public class ProduccionEquipoProfile : Profile
{
    public ProduccionEquipoProfile()
    {
        CreateMap<EquipoObtenerDto, EquipoEditarDto>();
    }
}