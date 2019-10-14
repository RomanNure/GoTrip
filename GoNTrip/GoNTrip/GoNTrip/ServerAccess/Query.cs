using System.Linq;
using System.Collections.Generic;

namespace GoNTrip.ServerAccess
{
    public enum QueryMethod
    {
        GET,
        POST
    };

    public class Query : IQuery
    {
        public QueryMethod Method { get; private set; }
        public string QueryBody { get; private set; }
        public string ServerMethod { get; private set; }
        public IList<string> NeededHeaders { get; private set; }
        public IDictionary<string, string> Parameters { get; private set; }

        public string ParametersString { get { return string.Join("&", Parameters.Select(KVP => KVP.Key + "=" + KVP.Value)); } }

        public Query(QueryMethod method, string serverMethod, string queryBody = "", IDictionary<string, string> parameters = null)
        {
            Method = method;
            QueryBody = queryBody;
            ServerMethod = serverMethod;
            
            NeededHeaders = new List<string>();
            Parameters = parameters == null ? new Dictionary<string, string>() : parameters;
        }
    }
}
