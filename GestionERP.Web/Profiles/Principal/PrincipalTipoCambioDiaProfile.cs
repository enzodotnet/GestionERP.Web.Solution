using AutoMapper;
using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Profiles.Principal;

public class PrincipalTipoCambioDiaProfile : Profile
{
    public PrincipalTipoCambioDiaProfile()
    {
        CreateMap<TipoCambioDiaObtenerDto, TipoCambioDiaActualizarMontoDto>();
    }
}