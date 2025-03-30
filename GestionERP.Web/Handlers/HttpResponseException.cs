namespace GestionERP.Web.Handlers;

[Serializable]
public class HttpResponseException : Exception
{
    public string Code { get; }

    public HttpResponseException() { }

    public HttpResponseException(string message)
        : base(message) { }

    public HttpResponseException(string message, Exception inner)
        : base(message, inner) { }

    // public HttpResponseException (SerializationInfo info,  StreamingContext context)
    //     : base (info, context){}

    public HttpResponseException(string message, string code)
        : this(message)
    {
        Code = code;
    }
}