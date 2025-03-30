using AutoMapper;
using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Profiles.Principal;

public class PrincipalCuentaContableProfile : Profile
{
    public PrincipalCuentaContableProfile()
    {
        CreateMap<CuentaContableObtenerDto, CuentaContableEditarDto>().ReverseMap();
        CreateMap<CuentaContableDestinoObtenerDto, CuentaContableDestinoInsertarDto>().ReverseMap();
        CreateMap<CuentaContableDetalleObtenerDto, CuentaContableDetalleInsertarDto>().ReverseMap();
        CreateMap<CuentaContableDestinoObtenerDto, CuentaContableDestinoEditarDto>().ReverseMap();
        CreateMap<CuentaContableDetalleObtenerDto, CuentaContableDetalleEditarDto>().ReverseMap();
    }
}