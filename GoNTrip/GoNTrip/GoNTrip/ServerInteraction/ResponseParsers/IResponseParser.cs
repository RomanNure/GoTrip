using GoNTrip.Model;
using GoNTrip.ServerAccess;

namespace GoNTrip.ServerInteraction.ResponseParsers
{
    public interface IResponseParser
    {
        ModelElement Parse(IServerResponse modelElementJSON);
    }
}
