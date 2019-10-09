using System;
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
        public string ServerMethod { get; private set; } 
        public IDictionary<string, string> Parameters { get; private set; }
        public string QueryBody { get; private set; }
        public IList<string> NeededHeaders { get; private set; }

        public string ParametersString { get { return string.Join("&", Parameters.Select(KVP => KVP.Key + "=" + KVP.Value)); } }

        public Query(QueryMethod method, string serverMethod, string queryBody = "")
        {
            Method = method;
            ServerMethod = serverMethod;
            Parameters = new Dictionary<string, string>();
            QueryBody = queryBody;
            NeededHeaders = new List<string>();
        }
    }
}
