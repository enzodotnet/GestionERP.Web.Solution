using Microsoft.AspNetCore.Components.Forms;
using System.Security.Claims;
using System.Text.Json;
using Telerik.Blazor.Components;

namespace GestionERP.Web.Global;

/// <summary>
/// Clase global <c>Función</c> de métodos estáticos compartidos.
/// </summary>
public class Fnc
{
    public static string ObtenerEstado(string codigoEstado, string tipoMensaje)
    {
        string mensaje = "";

        if (tipoMensaje == "title")
        {
            mensaje = codigoEstado switch
            {
                "RV" => "Revisando",
                "AX" => "Anulando",
                "EX" => "Excluyendo",
                "DX" => "Desestimando",
                "RR" => "Retirando revisión",
                "AP" => "Aprobando",
                "DP" => "Desaprobando",
                "CX" => "Cancelando",
                "RS" => "Rescindiendo",
				"CO" => "Cerrando",
                "AU" => "Aperturando",
                "CE" => "Costeando",
                "RE" => "Retirando costeo",
				"EI" => "Enviando a ingreso en almacén",
                "RI" => "Revirtiendo envío a ingreso en almacén",
                "ER" => "Enviando a revisión",
                "RO" => "Revirtiendo envío a revisión",
                "EA" => "Enviando a aprobación",
                "RC" => "Revirtiendo envío a aprobación",
                "EP" => "Enviando a proceso",
                "RP" => "Revirtiendo envío a proceso",
                _ => "Eliminando"
            };
        }
        else if (tipoMensaje == "result")
        {
            mensaje = codigoEstado switch
            {
                "RV" => "revisado",
                "AX" => "anulado",
                "EX" => "excluído",
                "DX" => "desestimado",
                "RR" => "retirado la revisión",
                "AP" => "aprobado",
                "DP" => "desaprobado",
                "CX" => "cancelado",
                "RS" => "rescindido",
				"CO" => "cerrado",
				"AU" => "aperturado",
				"CE" => "costeado",
				"RE" => "retirado el costeo",
				"EI" => "enviado a ingreso en almacén",
                "RI" => "revertido envío a ingreso en almacén",
                "ER" => "enviado a revisión",
                "RO" => "revertido envío a revisión",
                "EA" => "enviado a aprobación",
                "RC" => "revertido envío a aprobación",
                "EP" => "enviado a proceso",
                "RP" => "revertido envío a proceso",
                _ => "eliminado"
            };
        }
        else if (tipoMensaje is "loading" or "view")
        {
            mensaje = codigoEstado switch
            {
                "RV" => "Revisión",
                "AX" => "Anulación",
                "EX" => "Exclusión",
                "DX" => "Desestimación",
                "RR" => "Retiro de revisión",
                "AP" => "Aprobación",
                "DP" => "Desaprobación",
                "CX" => "Cancelación",
                "RS" => "Rescisión",
				"CO" => "Cierre",
				"AU" => "Apertura",
				"CE" => "Costeo",
				"RE" => "Retiro de costeo",
				"EI" => "Envío a ingreso en almacén",
                "RI" => "Reversión de envío a ingreso en almacén",
                "ER" => "Envío a revisión",
                "RO" => "Reversión de envío a revisión",
                "EA" => "Envío a aprobación",
                "RC" => "Reversión de envío a aprobación",
                "EP" => "Envío a proceso",
                "RP" => "Reversión de envío a proceso",
                _ => "Eliminación"
            };
        }
        else if (tipoMensaje == "action")
        {
            mensaje = codigoEstado switch
            {
                "RV" => "Revisar",
                "AX" => "Anular",
                "EX" => "Excluir",
                "DX" => "Desestimar",
                "RR" => "Retirar revisión",
                "AP" => "Aprobar",
                "DP" => "Desaprobar",
                "CX" => "Cancelar",
                "RS" => "Rescindir",
				"CO" => "Cerrar",
				"AU" => "Aperturar",
				"CE" => "Costear",
				"RE" => "Retirar costeo",
				"EI" => "Enviar a ingreso en almacén",
                "RI" => "Revertir el envío a ingreso en almacén",
                "ER" => "Enviar a revisión",
                "RO" => "Revertir el envío a revisión",
                "EA" => "Enviar a aprobación",
                "RC" => "Revertir el envío a aprobación",
                "EP" => "Enviar a proceso",
                "RP" => "Revertir el envío a proceso",
                _ => "eliminar"
            };
        }

        return mensaje;
    }

    public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        List<Claim> claims = [];
        string payload = jwt.Split('.')[1];
        byte[] jsonBytes = ParseBase64WithoutPadding(payload);

        Dictionary<string, object> keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

        return claims;
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }

    public static string GetMimeType(string extension)
    {
        ArgumentNullException.ThrowIfNull(extension);

        if (extension.StartsWith(".".ToString()))
            extension = extension[1..];

        return extension.ToLower() switch
        {
            "7z" => "application/x-7z-compressed",
            "doc" => "application/msword",
            "docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            "jpe" => "image/jpeg",
            "jpeg" => "image/jpeg",
            "jpg" => "image/jpeg",
            "pdf" => "application/pdf",
            "png" => "image/png",
            "pnz" => "image/png",
            "ppt" => "application/vnd.ms-powerpoint",
            "pptx" => "application/vnd.openxmlformats-officedocument.presentationml.presentation",
            "txt" => "text/plain",
            "xls" => "application/vnd.ms-excel",
            "xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "xml" => "text/xml",
            "zip" => "application/x-zip-compressed",
            _ => "application/octet-stream",
        };
    }

    public static void MostrarAlerta(TelerikNotification telerikNotification, string mensaje, string tipo)
    {
        telerikNotification.HideAll();
        telerikNotification.Show(new NotificationModel()
        {
            Text = mensaje,
            ThemeColor = tipo,
            CloseAfter = 3000,
            Closable = false
        });
    }

    public static bool VerifyContextIsChanged(bool isFieldChanged, EditContext context, string nameProperty)
    {
        if (isFieldChanged)
            context.NotifyFieldChanged(context.Field(nameProperty));
        else
            context.MarkAsUnmodified(context.Field(nameProperty));
        return context.IsModified();
    }
}