using AutoMapper;
using GestionERP.Web.Models.Dtos.Importacion;

namespace GestionERP.Web.Profiles.Importacion;

public class ImportacionNotaReporteOrdenProfile : Profile
{
    public ImportacionNotaReporteOrdenProfile()
    {
        CreateMap<NotaReporteOrdenObtenerDto, NotaReporteOrdenEditarDto>(); 
    }
}