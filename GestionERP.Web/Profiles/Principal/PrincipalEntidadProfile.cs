using AutoMapper;
using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Profiles.Principal;

public class PrincipalEntidadProfile : Profile
{
    public PrincipalEntidadProfile()
    {
        CreateMap<EntidadObtenerDto, EntidadEditarDto>().ReverseMap();
        CreateMap<EntidadDireccionObtenerDto, EntidadDireccionInsertarDto>().ReverseMap();
        CreateMap<EntidadRepresentanteObtenerDto, EntidadRepresentanteInsertarDto>().ReverseMap();
        CreateMap<EntidadVehiculoObtenerDto, EntidadVehiculoInsertarDto>().ReverseMap();
        CreateMap<EntidadDireccionObtenerDto, EntidadDireccionEditarDto>().ReverseMap();
        CreateMap<EntidadRepresentanteObtenerDto, EntidadRepresentanteEditarDto>().ReverseMap();
        CreateMap<EntidadVehiculoObtenerDto, EntidadVehiculoEditarDto>().ReverseMap();
        CreateMap<EntidadFichaSunatObtenerDto, EntidadFichaSunatEditarDto>().ReverseMap();
    }
}