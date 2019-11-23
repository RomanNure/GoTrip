using System;
using System.Collections.Generic;

using GoNTrip.Model;
using GoNTrip.ServerAccess;

namespace GoNTrip.ServerInteraction.ResponseParsers
{
    public interface IResponseParser
    {
        T Parse<T, E>(IServerResponse modelElementJSON) where T : ModelElement
                                                        where E : Exception;

        IEnumerable<T> ParseCollection<T, E>(IServerResponse modelElementJSON) where T : ModelElement
                                                                               where E : Exception;
    }
}
