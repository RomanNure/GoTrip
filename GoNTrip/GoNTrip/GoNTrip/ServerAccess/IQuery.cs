using System.Net;
using System.Collections.Generic;

namespace GoNTrip.ServerAccess
{
    public interface IQuery
    {
        QueryMethod Method { get; }
        string ServerMethod { get; }

        string QueryBody { get; }
        string ParametersString { get; }
        IDictionary<string, string> Parameters { get; }
        IList<MultipartDataItem> MultipartData { get; }

        IList<string> NeededHeaders { get; }
        IList<string> NeededCookies { get; }

        IDictionary<string, string> Headers { get; }
    }
}
