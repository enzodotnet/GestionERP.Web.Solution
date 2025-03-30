using AutoMapper;
using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Profiles.Principal;

public class PrincipalTransporteImportacionProfile : Profile
{
    public PrincipalTransporteImportacionProfile()
    {
        CreateMap<TransporteImportacionObtenerDto, TransporteImportacionEditarDto>();
    }
}