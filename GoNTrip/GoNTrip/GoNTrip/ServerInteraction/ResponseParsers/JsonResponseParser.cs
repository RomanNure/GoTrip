using System.Collections;
using System.Collections.Generic;

using GoNTrip.Model;
using GoNTrip.ServerAccess;

using Newtonsoft.Json;

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

        public IEnumerable<T> ParseCollection<T>(IServerResponse modelElementJSON) where T : ModelElement
        {
            ResponseException ex = null;

            try { ex = JsonConvert.DeserializeObject<ResponseException>(modelElementJSON.Data); }
            catch { return JsonConvert.DeserializeObject<IEnumerable<T>>(modelElementJSON.Data); }

            throw ex;
        }
    }
}
