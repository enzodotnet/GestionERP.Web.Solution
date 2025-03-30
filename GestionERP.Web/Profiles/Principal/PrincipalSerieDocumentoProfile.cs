using AutoMapper;
using GestionERP.Web.Models.Dtos.Principal;

namespace GestionERP.Web.Profiles.Principal;

public class PrincipalSerieDocumentoProfile : Profile
{
    public PrincipalSerieDocumentoProfile()
    {
        CreateMap<DocumentoObtenerDto, DocumentoEditarDto>();
        CreateMap<SerieDocumentoObtenerDto, SerieDocumentoEditarDto>().ReverseMap();
        CreateMap<SerieDocumentoInsertarDto, SerieDocumentoObtenerDto>().ReverseMap();
        CreateMap<SerieDocumentoEditarDto, SerieDocumentoObtenerDto>().ReverseMap();
    }
}