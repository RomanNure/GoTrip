using System.Net;
using System.Collections.Generic;

namespace GoNTrip.ServerAccess
{
    public interface IServerResponse
    {
        string Data { get; }
        IDictionary<string, string> Headers { get; }
        CookieContainer CookieContainer { get; }
    }
}
