using AutoMapper;
using GestionERP.Web.Models.Dtos.Produccion;

namespace GestionERP.Web.Profiles.Produccion;

public class ProduccionFuncionProfile : Profile
{
    public ProduccionFuncionProfile()
    {
        CreateMap<FuncionObtenerDto, FuncionEditarDto>();
    }
}