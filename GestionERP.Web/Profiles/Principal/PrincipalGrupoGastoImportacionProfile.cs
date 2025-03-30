using AutoMapper;
using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Profiles.Principal;

public class PrincipalGrupoGastoImportacionProfile : Profile
{
    public PrincipalGrupoGastoImportacionProfile()
    {
        CreateMap<GrupoGastoImportacionObtenerDto, GrupoGastoImportacionEditarDto>();
    }
}