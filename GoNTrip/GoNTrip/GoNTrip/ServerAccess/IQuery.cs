using System.Collections.Generic;

namespace GoNTrip.ServerAccess
{
    public interface IQuery
    {
        QueryMethod Method { get; }
        string ServerMethod { get; }
        IDictionary<string, string> Parameters { get; }
        string ParametersString { get; }
        string QueryBody { get; }
        IList<string> NeededHeaders { get; }
        IList<MultipartDataItem> MultipartData { get; }
    }
}
