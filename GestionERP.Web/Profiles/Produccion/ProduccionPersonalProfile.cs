using AutoMapper;
using GestionERP.Web.Models.Dtos.Produccion;

namespace GestionERP.Web.Profiles.Produccion;

public class ProduccionPersonalProfile : Profile
{
    public ProduccionPersonalProfile()
    {
        CreateMap<PersonalObtenerDto, PersonalEditarDto>();
    }
}