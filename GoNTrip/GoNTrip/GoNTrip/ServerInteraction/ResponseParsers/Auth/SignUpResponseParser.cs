using System;
using Android.Util;
using GoNTrip.Model;
using GoNTrip.ServerAccess;

using Newtonsoft.Json;

namespace GoNTrip.ServerInteraction.ResponseParsers.Auth
{
    public class SignUpResponseParser : IResponseParser
    {
        public ModelElement Parse(IServerResponse modelElementJSON)
        {
            ResponseException ex = null;

            try
            {
                ex = JsonConvert.DeserializeObject<ResponseException>(modelElementJSON.Data);
            }
            catch
            {
                return JsonConvert.DeserializeObject<User>(modelElementJSON.Data);
            }

            throw ex;
        }
    }
}
