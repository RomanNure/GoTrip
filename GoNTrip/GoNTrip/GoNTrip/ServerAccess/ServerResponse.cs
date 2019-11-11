using System;
using System.Collections.Generic;
using System.Text;

namespace GoNTrip.ServerAccess
{
    public class ServerResponse : IServerResponse
    {
        public string Data { get; private set; }
        public IDictionary<string, string> Headers { get; private set; }

        public ServerResponse(string data, IDictionary<string, string> headers)
        {
            Data = data;
            Headers = headers;
        }
    }
}
