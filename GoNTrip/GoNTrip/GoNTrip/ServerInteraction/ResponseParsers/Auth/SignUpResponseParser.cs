using GoNTrip.Model;
using GoNTrip.ServerAccess;

using Newtonsoft.Json;

namespace GoNTrip.ServerInteraction.ResponseParsers.Auth
{
    public class SignUpResponseParser : IResponseParser
    {
        public ModelElement Parse(IServerResponse modelElementJSON)
        {
            return JsonConvert.DeserializeObject<User>(modelElementJSON.Data);
        }
    }
}
