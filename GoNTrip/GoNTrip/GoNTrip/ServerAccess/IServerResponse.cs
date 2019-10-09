using System;
using System.Collections.Generic;
using System.Text;

namespace GoNTrip.ServerAccess
{
    public interface IServerResponse
    {
        string Data { get; }
        IDictionary<string, string> Headers { get; }
    }
}
