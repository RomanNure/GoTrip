using GoNTrip.Model;
using GoNTrip.ServerAccess;

namespace GoNTrip.ServerInteraction.ResponseParsers
{
    public interface IResponseParser
    {
        T Parse<T>(IServerResponse modelElementJSON) where T : ModelElement;
    }
}
