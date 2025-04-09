using GestionERP.Web.Services.Interfaces;
using GestionERP.Web.Models.Dtos.Principal;
using GestionERP.Web.Models.Dtos.Produccion;
using GestionERP.Web.Models.Dtos.Importacion;
using Telerik.Blazor.Components;
using GestionERP.Web.Global;
using GestionERP.Web.Handlers;
using Microsoft.AspNetCore.Components.Authorization; 

namespace GestionERP.Web.Services;

public class UtilService
(
    AuthenticationStateProvider IAuthState,
    IPrincipalTipoImpuesto tipoImpuesto,
    IPrincipalTipoProvision tipoProvision,
    IPrincipalPais pais,
    IPrincipalSituacionImportacion situacionImportacion,
    IPrincipalRegimenImportacion regimenImportacion,
    IPrincipalTransporteImportacion transporteImportacion,
    IPrincipalTipoImportacion tipoImportacion,
    IPrincipalPuerto puerto,
    IPrincipalAduana aduana,
    IPrincipalTipoServicio tipoServicio,
    IPrincipalTipoTermino tipoTermino,
    IPrincipalArea area,
    IPrincipalTipoDevengo tipoDevengo,
    IPrincipalTipoProduccion tipoProduccion,   
    IPrincipalEntidad entidad,
    IPrincipalArticulo articulo,
    IPrincipalLocal local,
    IPrincipalAlmacen almacen,
    IPrincipalCentroCosto centroCosto,
    IPrincipalUsuario usuario,
    IImportacionNotaReporteOrden notaReporteOrden,
    IProduccionPlan plan,
    IProduccionVersionPlan versionPlan
)
{
    public async Task<(TipoImpuestoObtenerPorCodigoDto item, string mensajeError)> ObtenerTipoImpuestoPorCodigo(TelerikNotification alertNotify, string codigo, bool? esOperacionVenta = null)
    { 
        (TipoImpuestoObtenerPorCodigoDto item, string messageError) resultado = (null, null);
        try
        { 
            if ((await IAuthState.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated)
                resultado.item = await tipoImpuesto.ObtenerPorCodigo(codigo, esOperacionVenta);
            else
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNoAuthenticated, "error");
        }
        catch (Exception ex)
        {
            if (ex is HttpRequestException)
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNotConnectApi, "error");
            else if (ex is HttpResponseException)
            {
                string codeError = (ex as HttpResponseException).Code;
                if (codeError is "VA")
                    resultado.messageError = ex.Message;
                else
                    Fnc.MostrarAlerta(alertNotify, codeError is "NF" ? Cnf.MsgErrorNotFoundAPi : codeError is "TK" ? Cnf.MsgErrorExpiredToken : ex.Message, "error");
            }
            else
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorFuncAppWeb, "error");
        }
        return resultado;
    }

    public async Task<(TipoProvisionObtenerPorCodigoDto item, string mensajeError)> ObtenerTipoProvisionPorCodigo(TelerikNotification alertNotify, string codigo)
    {
        (TipoProvisionObtenerPorCodigoDto item, string messageError) resultado = (null, null);
        try
        {
            if ((await IAuthState.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated)
                resultado.item = await tipoProvision.ObtenerPorCodigo(codigo);
            else
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNoAuthenticated, "error");
        }
        catch (Exception ex)
        {
            if (ex is HttpRequestException)
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNotConnectApi, "error");
            else if (ex is HttpResponseException)
            {
                string codeError = (ex as HttpResponseException).Code;
                if (codeError is "VA")
                    resultado.messageError = ex.Message;
                else
                    Fnc.MostrarAlerta(alertNotify, codeError is "NF" ? Cnf.MsgErrorNotFoundAPi : codeError is "TK" ? Cnf.MsgErrorExpiredToken : ex.Message, "error");
            }
            else
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorFuncAppWeb, "error");
        }
        return resultado;
    }

    public async Task<(AduanaObtenerPorCodigoDto item, string mensajeError)> ObtenerAduanaPorCodigo(TelerikNotification alertNotify, string codigoAduana)
    {
        (AduanaObtenerPorCodigoDto item, string messageError) resultado = (null, null);
        try
        {
            if ((await IAuthState.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated)
                resultado.item = await aduana.ObtenerPorCodigo(codigoAduana);
            else
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNoAuthenticated, "error");
        }
        catch (Exception ex)
        {
            if (ex is HttpRequestException)
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNotConnectApi, "error");
            else if (ex is HttpResponseException)
            {
                string codeError = (ex as HttpResponseException).Code;
                if (codeError is "VA")
                    resultado.messageError = ex.Message;
                else
                    Fnc.MostrarAlerta(alertNotify, codeError is "NF" ? Cnf.MsgErrorNotFoundAPi : codeError is "TK" ? Cnf.MsgErrorExpiredToken : ex.Message, "error");
            }
            else
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorFuncAppWeb, "error");
        }
        return resultado;
    }

    public async Task<(RegimenImportacionObtenerPorCodigoDto item, string mensajeError)> ObtenerRegimenImportacionPorCodigo(TelerikNotification alertNotify, string codigo)
    {
        (RegimenImportacionObtenerPorCodigoDto item, string messageError) resultado = (null, null);
        try
        {
            if ((await IAuthState.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated)
                resultado.item = await regimenImportacion.ObtenerPorCodigo(codigo);
            else
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNoAuthenticated, "error");
        }
        catch (Exception ex)
        {
            if (ex is HttpRequestException)
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNotConnectApi, "error");
            else if (ex is HttpResponseException)
            {
                string codeError = (ex as HttpResponseException).Code;
                if (codeError is "VA")
                    resultado.messageError = ex.Message;
                else
                    Fnc.MostrarAlerta(alertNotify, codeError is "NF" ? Cnf.MsgErrorNotFoundAPi : codeError is "TK" ? Cnf.MsgErrorExpiredToken : ex.Message, "error");
            }
            else
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorFuncAppWeb, "error");
        }
        return resultado;
    }

    public async Task<(SituacionImportacionObtenerPorCodigoDto item, string mensajeError)> ObtenerSituacionImportacionPorCodigo(TelerikNotification alertNotify, string codigo)
    {
        (SituacionImportacionObtenerPorCodigoDto item, string messageError) resultado = (null, null);
        try
        {
            if ((await IAuthState.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated)
                resultado.item = await situacionImportacion.ObtenerPorCodigo(codigo);
            else
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNoAuthenticated, "error");
        }
        catch (Exception ex)
        {
            if (ex is HttpRequestException)
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNotConnectApi, "error");
            else if (ex is HttpResponseException)
            {
                string codeError = (ex as HttpResponseException).Code;
                if (codeError is "VA")
                    resultado.messageError = ex.Message;
                else
                    Fnc.MostrarAlerta(alertNotify, codeError is "NF" ? Cnf.MsgErrorNotFoundAPi : codeError is "TK" ? Cnf.MsgErrorExpiredToken : ex.Message, "error");
            }
            else
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorFuncAppWeb, "error");
        }
        return resultado;
    }

    public async Task<(TipoImportacionObtenerPorCodigoDto item, string mensajeError)> ObtenerTipoImportacionPorCodigo(TelerikNotification alertNotify, string codigo)
    {
        (TipoImportacionObtenerPorCodigoDto item, string messageError) resultado = (null, null);
        try
        {
            if ((await IAuthState.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated)
                resultado.item = await tipoImportacion.ObtenerPorCodigo(codigo);
            else
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNoAuthenticated, "error");
        }
        catch (Exception ex)
        {
            if (ex is HttpRequestException)
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNotConnectApi, "error");
            else if (ex is HttpResponseException)
            {
                string codeError = (ex as HttpResponseException).Code;
                if (codeError is "VA")
                    resultado.messageError = ex.Message;
                else
                    Fnc.MostrarAlerta(alertNotify, codeError is "NF" ? Cnf.MsgErrorNotFoundAPi : codeError is "TK" ? Cnf.MsgErrorExpiredToken : ex.Message, "error");
            }
            else
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorFuncAppWeb, "error");
        }
        return resultado;
    }

    public async Task<(TransporteImportacionObtenerPorCodigoDto item, string mensajeError)> ObtenerTransporteImportacionPorCodigo(TelerikNotification alertNotify, string codigo)
    {
        (TransporteImportacionObtenerPorCodigoDto item, string messageError) resultado = (null, null);
        try
        {
            if ((await IAuthState.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated)
                resultado.item = await transporteImportacion.ObtenerPorCodigo(codigo);
            else
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNoAuthenticated, "error");
        }
        catch (Exception ex)
        {
            if (ex is HttpRequestException)
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNotConnectApi, "error");
            else if (ex is HttpResponseException)
            {
                string codeError = (ex as HttpResponseException).Code;
                if (codeError is "VA")
                    resultado.messageError = ex.Message;
                else
                    Fnc.MostrarAlerta(alertNotify, codeError is "NF" ? Cnf.MsgErrorNotFoundAPi : codeError is "TK" ? Cnf.MsgErrorExpiredToken : ex.Message, "error");
            }
            else
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorFuncAppWeb, "error");
        }
        return resultado;
    }

    public async Task<(PuertoObtenerPorCodigoDto item, string mensajeError)> ObtenerPuertoPorCodigo(TelerikNotification alertNotify, string codigo)
    {
        (PuertoObtenerPorCodigoDto item, string messageError) resultado = (null, null);
        try
        {
            if ((await IAuthState.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated)
                resultado.item = await puerto.ObtenerPorCodigo(codigo);
            else
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNoAuthenticated, "error");
        }
        catch (Exception ex)
        {
            if (ex is HttpRequestException)
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNotConnectApi, "error");
            else if (ex is HttpResponseException)
            {
                string codeError = (ex as HttpResponseException).Code;
                if (codeError is "VA")
                    resultado.messageError = ex.Message;
                else
                    Fnc.MostrarAlerta(alertNotify, codeError is "NF" ? Cnf.MsgErrorNotFoundAPi : codeError is "TK" ? Cnf.MsgErrorExpiredToken : ex.Message, "error");
            }
            else
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorFuncAppWeb, "error");
        }
        return resultado;
    }

    public async Task<(PaisObtenerPorCodigoDto item, string mensajeError)> ObtenerPaisPorCodigo(TelerikNotification alertNotify, string codigo)
    {
        (PaisObtenerPorCodigoDto item, string messageError) resultado = (null, null);
        try
        {
            if ((await IAuthState.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated)
                resultado.item = await pais.ObtenerPorCodigo(codigo);
            else
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNoAuthenticated, "error");
        }
        catch (Exception ex)
        {
            if (ex is HttpRequestException)
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNotConnectApi, "error");
            else if (ex is HttpResponseException)
            {
                string codeError = (ex as HttpResponseException).Code;
                if (codeError is "VA")
                    resultado.messageError = ex.Message;
                else
                    Fnc.MostrarAlerta(alertNotify, codeError is "NF" ? Cnf.MsgErrorNotFoundAPi : codeError is "TK" ? Cnf.MsgErrorExpiredToken : ex.Message, "error");
            }
            else
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorFuncAppWeb, "error");
        }
        return resultado;
    }

    public async Task<(TipoTerminoObtenerPorCodigoDto item, string mensajeError)> ObtenerTipoTerminoPorCodigo(TelerikNotification alertNotify, string codigo, string flagRegistro = null)
    {
        (TipoTerminoObtenerPorCodigoDto item, string messageError) resultado = (null, null);
        try
        {
            if ((await IAuthState.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated)
                resultado.item = await tipoTermino.ObtenerPorCodigo(codigo, flagRegistro);
            else
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNoAuthenticated, "error");
        }
        catch (Exception ex)
        {
            if (ex is HttpRequestException)
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNotConnectApi, "error");
            else if (ex is HttpResponseException)
            {
                string codeError = (ex as HttpResponseException).Code;
                if (codeError is "VA")
                    resultado.messageError = ex.Message;
                else
                    Fnc.MostrarAlerta(alertNotify, codeError is "NF" ? Cnf.MsgErrorNotFoundAPi : codeError is "TK" ? Cnf.MsgErrorExpiredToken : ex.Message, "error");
            }
            else
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorFuncAppWeb, "error");
        }
        return resultado;
    }

    public async Task<(TipoServicioObtenerPorCodigoDto item, string mensajeError)> ObtenerTipoServicioPorCodigo(TelerikNotification alertNotify, string codigo, string flagRegistro = null)
    {
        (TipoServicioObtenerPorCodigoDto item, string messageError) resultado = (null, null);
        try
        {
            if ((await IAuthState.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated)
                resultado.item = await tipoServicio.ObtenerPorCodigo(codigo, flagRegistro);
            else
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNoAuthenticated, "error");
        }
        catch (Exception ex)
        {
            if (ex is HttpRequestException)
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNotConnectApi, "error");
            else if (ex is HttpResponseException)
            {
                string codeError = (ex as HttpResponseException).Code;
                if (codeError is "VA")
                    resultado.messageError = ex.Message;
                else
                    Fnc.MostrarAlerta(alertNotify, codeError is "NF" ? Cnf.MsgErrorNotFoundAPi : codeError is "TK" ? Cnf.MsgErrorExpiredToken : ex.Message, "error");
            }
            else
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorFuncAppWeb, "error");
        }
        return resultado;
    }

	public async Task<(AreaObtenerPorCodigoDto item, string mensajeError)> ObtenerAreaPorCodigo(TelerikNotification alertNotify, string codigo)
	{
		(AreaObtenerPorCodigoDto item, string messageError) resultado = (null, null);
		try
		{
			if ((await IAuthState.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated)
				resultado.item = await area.ObtenerPorCodigo(codigo);
			else
				Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNoAuthenticated, "error");
		}
		catch (Exception ex)
		{
			if (ex is HttpRequestException)
				Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNotConnectApi, "error");
			else if (ex is HttpResponseException)
			{
				string codeError = (ex as HttpResponseException).Code;
				if (codeError is "VA")
					resultado.messageError = ex.Message;
				else
					Fnc.MostrarAlerta(alertNotify, codeError is "NF" ? Cnf.MsgErrorNotFoundAPi : codeError is "TK" ? Cnf.MsgErrorExpiredToken : ex.Message, "error");
			}
			else
				Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorFuncAppWeb, "error");
		}
		return resultado;
	}

	public async Task<(TipoProduccionObtenerPorCodigoDto item, string mensajeError)> ObtenerTipoProduccionPorCodigo(TelerikNotification alertNotify, string codigo, string flagTipoProceso = null)
	{
		(TipoProduccionObtenerPorCodigoDto item, string messageError) resultado = (null, null);
		try
		{
			if ((await IAuthState.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated)
				resultado.item = await tipoProduccion.ObtenerPorCodigo(codigo, flagTipoProceso);
			else
				Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNoAuthenticated, "error");
		}
		catch (Exception ex)
		{
			if (ex is HttpRequestException)
				Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNotConnectApi, "error");
			else if (ex is HttpResponseException)
			{
				string codeError = (ex as HttpResponseException).Code;
				if (codeError is "VA")
					resultado.messageError = ex.Message;
				else
					Fnc.MostrarAlerta(alertNotify, codeError is "NF" ? Cnf.MsgErrorNotFoundAPi : codeError is "TK" ? Cnf.MsgErrorExpiredToken : ex.Message, "error");
			}
			else
				Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorFuncAppWeb, "error");
		}
		return resultado;
	}

	public async Task<(TipoDevengoObtenerPorCodigoDto item, string mensajeError)> ObtenerTipoDevengoPorCodigo(TelerikNotification alertNotify, string codigo)
	{
		(TipoDevengoObtenerPorCodigoDto item, string messageError) resultado = (null, null);
		try
		{
			if ((await IAuthState.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated)
				resultado.item = await tipoDevengo.ObtenerPorCodigo(codigo);
			else
				Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNoAuthenticated, "error");
		}
		catch (Exception ex)
		{
			if (ex is HttpRequestException)
				Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNotConnectApi, "error");
			else if (ex is HttpResponseException)
			{
				string codeError = (ex as HttpResponseException).Code;
				if (codeError is "VA")
					resultado.messageError = ex.Message;
				else
					Fnc.MostrarAlerta(alertNotify, codeError is "NF" ? Cnf.MsgErrorNotFoundAPi : codeError is "TK" ? Cnf.MsgErrorExpiredToken : ex.Message, "error");
			}
			else
				Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorFuncAppWeb, "error");
		}
		return resultado;
	}

    public async Task<(EntidadObtenerPorCodigoEmpresaDto item, string mensajeError)> ObtenerEntidadPorCodigoEmpresa(TelerikNotification alertNotify, string codigo, string codigoEmpresa, string flagTipo = null)
    { 
        (EntidadObtenerPorCodigoEmpresaDto item, string messageError) resultado = (null, null);
        try
        {
            if ((await IAuthState.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated)
                resultado.item = await entidad.ObtenerPorCodigoEmpresa(codigo, codigoEmpresa, flagTipo);
            else
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNoAuthenticated, "error");
        }
        catch (Exception ex)
        {
            if (ex is HttpRequestException)
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNotConnectApi, "error");
            else if (ex is HttpResponseException)
            {
                string codeError = (ex as HttpResponseException).Code;
                if (codeError is "VA")
                    resultado.messageError = ex.Message;
                else
                    Fnc.MostrarAlerta(alertNotify, codeError is "NF" ? Cnf.MsgErrorNotFoundAPi : codeError is "TK" ? Cnf.MsgErrorExpiredToken : ex.Message, "error");
            }
            else
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorFuncAppWeb, "error");
        }
        return resultado;
    }

    public async Task<(LocalObtenerPorCodigoEmpresaDto item, string mensajeError)> ObtenerLocalPorCodigoEmpresa(TelerikNotification alertNotify, string codigo, string codigoEmpresa)
    { 
        (LocalObtenerPorCodigoEmpresaDto item, string messageError) resultado = (null, null);
        try
        {
            if ((await IAuthState.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated)
                resultado.item = await local.ObtenerPorCodigoEmpresa(codigo, codigoEmpresa);
            else
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNoAuthenticated, "error");
        }
        catch (Exception ex)
        {
            if (ex is HttpRequestException)
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNotConnectApi, "error");
            else if (ex is HttpResponseException)
            {
                string codeError = (ex as HttpResponseException).Code;
                if (codeError is "VA")
                    resultado.messageError = ex.Message;
                else
                    Fnc.MostrarAlerta(alertNotify, codeError is "NF" ? Cnf.MsgErrorNotFoundAPi : codeError is "TK" ? Cnf.MsgErrorExpiredToken : ex.Message, "error");
            }
            else
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorFuncAppWeb, "error");
        }
        return resultado;
    }

    public async Task<(AlmacenObtenerPorCodigoEmpresaOperacionLogisticaSesionDto item, string mensajeError)> ObtenerAlmacenPorCodigoEmpresaOperacionLogisticaSesion(TelerikNotification alertNotify, string codigo, string codigoEmpresa, string codigoOperacionLogistica, string codigoTipoArticulo = null, string codigoAlmacenDestino = null)
    {
        (AlmacenObtenerPorCodigoEmpresaOperacionLogisticaSesionDto item, string messageError) resultado = (null, null);
        try
        {
            if ((await IAuthState.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated)
                resultado.item = await almacen.ObtenerPorCodigoEmpresaOperacionLogisticaSesion(codigo, codigoEmpresa, codigoOperacionLogistica, codigoTipoArticulo, codigoAlmacenDestino);
            else
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNoAuthenticated, "error");
        }
        catch (Exception ex)
        {
            if (ex is HttpRequestException)
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNotConnectApi, "error");
            else if (ex is HttpResponseException)
            {
                string codeError = (ex as HttpResponseException).Code;
                if (codeError is "VA")
                    resultado.messageError = ex.Message;
                else
                    Fnc.MostrarAlerta(alertNotify, codeError is "NF" ? Cnf.MsgErrorNotFoundAPi : codeError is "TK" ? Cnf.MsgErrorExpiredToken : ex.Message, "error");
            }
            else
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorFuncAppWeb, "error");
        }
        return resultado;
    }

    public async Task<(PlanObtenerPorCodigoDto item, string mensajeError)> ObtenerPlanProduccionPorCodigo(TelerikNotification alertNotify, string codigoEmpresa, string codigo, string flagTipoProceso = null)
    {
        (PlanObtenerPorCodigoDto item, string messageError) resultado = (null, null);
        try
        {
            if ((await IAuthState.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated)
                resultado.item = await plan.ObtenerPorCodigo(codigoEmpresa, codigo, flagTipoProceso);
            else
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNoAuthenticated, "error");
        }
        catch (Exception ex)
        {
            if (ex is HttpRequestException)
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNotConnectApi, "error");
            else if (ex is HttpResponseException)
            {
                string codeError = (ex as HttpResponseException).Code;
                if (codeError is "VA")
                    resultado.messageError = ex.Message;
                else
                    Fnc.MostrarAlerta(alertNotify, codeError is "NF" ? Cnf.MsgErrorNotFoundAPi : codeError is "TK" ? Cnf.MsgErrorExpiredToken : ex.Message, "error");
            }
            else
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorFuncAppWeb, "error");
        }
        return resultado;
    }

    public async Task<(CentroCostoObtenerPorCodigoDto item, string mensajeError)> ObtenerCentroCostoPorCodigo(TelerikNotification alertNotify, string codigo, string codigoEmpresa = null)
    { 
        (CentroCostoObtenerPorCodigoDto item, string messageError) resultado = (null, null);
        try
        {
            if ((await IAuthState.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated)
                resultado.item = await centroCosto.ObtenerPorCodigo(codigo, codigoEmpresa);
            else
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNoAuthenticated, "error");
        }
        catch (Exception ex)
        {
            if (ex is HttpRequestException)
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNotConnectApi, "error");
            else if (ex is HttpResponseException)
            {
                string codeError = (ex as HttpResponseException).Code;
                if (codeError is "VA")
                    resultado.messageError = ex.Message;
                else
                    Fnc.MostrarAlerta(alertNotify, codeError is "NF" ? Cnf.MsgErrorNotFoundAPi : codeError is "TK" ? Cnf.MsgErrorExpiredToken : ex.Message, "error");
            }
            else
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorFuncAppWeb, "error");
        }
        return resultado;
    }

    public async Task<(UsuarioObtenerPorCodigoEmpresaDto item, string mensajeError)> ObtenerUsuarioPorCodigoEmpresa(TelerikNotification alertNotify, string codigo, string codigoEmpresa)
    { 
        (UsuarioObtenerPorCodigoEmpresaDto item, string messageError) resultado = (null, null);
        try
        { 
            if ((await IAuthState.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated)
                resultado.item = await usuario.ObtenerPorCodigoEmpresa(codigo, codigoEmpresa);
            else
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNoAuthenticated, "error");
        }
        catch (Exception ex)
        {
            if (ex is HttpRequestException)
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNotConnectApi, "error");
            else if (ex is HttpResponseException)
            {
                string codeError = (ex as HttpResponseException).Code;
                if (codeError is "VA")
                    resultado.messageError = ex.Message;
                else
                    Fnc.MostrarAlerta(alertNotify, codeError is "NF" ? Cnf.MsgErrorNotFoundAPi : codeError is "TK" ? Cnf.MsgErrorExpiredToken : ex.Message, "error");
            }
            else
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorFuncAppWeb, "error");
        }
        return resultado;
    }

    public async Task<(EntidadProveedorObtenerPorCodigoEmpresaDto item, string mensajeError)> ObtenerProveedorPorCodigoEmpresa(TelerikNotification alertNotify, string codigo, string codigoEmpresa, string flagAmbito = null)
    { 
        (EntidadProveedorObtenerPorCodigoEmpresaDto item, string messageError) resultado = (null, null);
        try
        { 
            if ((await IAuthState.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated)
                resultado.item = await entidad.ObtenerProveedorPorCodigoEmpresa(codigo, codigoEmpresa, flagAmbito);
            else
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNoAuthenticated, "error");
        }
        catch (Exception ex)
        {
            if (ex is HttpRequestException)
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNotConnectApi, "error");
            else if (ex is HttpResponseException)
            {
                string codeError = (ex as HttpResponseException).Code;
                if (codeError is "VA")
                    resultado.messageError = ex.Message;
                else
                    Fnc.MostrarAlerta(alertNotify, codeError is "NF" ? Cnf.MsgErrorNotFoundAPi : codeError is "TK" ? Cnf.MsgErrorExpiredToken : ex.Message, "error");
            }
            else
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorFuncAppWeb, "error");
        }
        return resultado;
    }

    public async Task<(ArticuloObtenerPorCodigoEmpresaProcesoDocumentoDto item, string mensajeError)> ObtenerArticuloPorCodigoEmpresaProcesoDocumento(TelerikNotification alertNotify, string codigo, string codigoEmpresa, string codigoProcesoDocumento)
    { 
        (ArticuloObtenerPorCodigoEmpresaProcesoDocumentoDto item, string messageError) resultado = (null, null);
        try
        { 
            if ((await IAuthState.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated)
                resultado.item = await articulo.ObtenerPorCodigoEmpresaProcesoDocumento(codigo, codigoEmpresa, codigoProcesoDocumento);
            else
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNoAuthenticated, "error");
        }
        catch (Exception ex)
        {
            if (ex is HttpRequestException)
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNotConnectApi, "error");
            else if (ex is HttpResponseException)
            {
                string codeError = (ex as HttpResponseException).Code;
                if (codeError is "VA")
                    resultado.messageError = ex.Message;
                else
                    Fnc.MostrarAlerta(alertNotify, codeError is "NF" ? Cnf.MsgErrorNotFoundAPi : codeError is "TK" ? Cnf.MsgErrorExpiredToken : ex.Message, "error");
            }
            else
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorFuncAppWeb, "error");
        }
        return resultado;
    }
     
    public async Task<(NotaReporteOrdenObtenerPorCodigoDto item, string mensajeError)> ObtenerNotaReporteOrdenImportacionPorCodigo(TelerikNotification alertNotify, string codigoEmpresa, string codigo)
    {
        (NotaReporteOrdenObtenerPorCodigoDto item, string messageError) resultado = (null, null);
        try
        {
            if ((await IAuthState.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated)
                resultado.item = await notaReporteOrden.ObtenerPorCodigo(codigoEmpresa, codigo);
            else
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNoAuthenticated, "error");
        }
        catch (Exception ex)
        {
            if (ex is HttpRequestException)
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNotConnectApi, "error");
            else if (ex is HttpResponseException)
            {
                string codeError = (ex as HttpResponseException).Code;
                if (codeError is "VA")
                    resultado.messageError = ex.Message;
                else
                    Fnc.MostrarAlerta(alertNotify, codeError is "NF" ? Cnf.MsgErrorNotFoundAPi : codeError is "TK" ? Cnf.MsgErrorExpiredToken : ex.Message, "error");
            }
            else
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorFuncAppWeb, "error");
        }
        return resultado;
    }

    public async Task<(VersionPlanObtenerPorCodigoDto item, string mensajeError)> ObtenerVersionPlanProduccionPorCodigo(TelerikNotification alertNotify, string codigoEmpresa, string codigo, string codigoPlan = null)
    {
        (VersionPlanObtenerPorCodigoDto item, string messageError) resultado = (null, null);
        try
        {
            if ((await IAuthState.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated)
                resultado.item = await versionPlan.ObtenerPorCodigo(codigoEmpresa, codigo, codigoPlan);
            else
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNoAuthenticated, "error");
        }
        catch (Exception ex)
        {
            if (ex is HttpRequestException)
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorNotConnectApi, "error");
            else if (ex is HttpResponseException)
            {
                string codeError = (ex as HttpResponseException).Code;
                if (codeError is "VA")
                    resultado.messageError = ex.Message;
                else
                    Fnc.MostrarAlerta(alertNotify, codeError is "NF" ? Cnf.MsgErrorNotFoundAPi : codeError is "TK" ? Cnf.MsgErrorExpiredToken : ex.Message, "error");
            }
            else
                Fnc.MostrarAlerta(alertNotify, Cnf.MsgErrorFuncAppWeb, "error");
        }
        return resultado;
    }
} 