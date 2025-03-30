using AutoMapper;
using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Profiles.Principal;

public class PrincipalDocumentoProfile : Profile
{
    public PrincipalDocumentoProfile()
    {
        CreateMap<DocumentoObtenerDto, DocumentoEditarDto>();
    }
}