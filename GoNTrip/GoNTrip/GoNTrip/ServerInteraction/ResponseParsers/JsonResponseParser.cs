using GoNTrip.Model;
using GoNTrip.ServerAccess;

using Newtonsoft.Json;
using System;

namespace GoNTrip.ServerInteraction.ResponseParsers
{
    public class JsonResponseParser : IResponseParser
    {
        public T Parse<T>(IServerResponse modelElementJSON) where T : ModelElement
        {
            ResponseException ex = null;

            try { ex = JsonConvert.DeserializeObject<ResponseException>(modelElementJSON.Data); }
            catch { return JsonConvert.DeserializeObject<T>(modelElementJSON.Data); }

            throw ex;
        }

    }
}
