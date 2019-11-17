using System.Net;
using System.Linq;
using System.Collections.Generic;

namespace GoNTrip.ServerAccess
{
    public enum QueryMethod
    {
        GET,
        POST,
        POST_MULTIPART
    };

    public class Query : IQuery
    {
        public QueryMethod Method { get; private set; }
        public string ServerMethod { get; private set; }

        public string QueryBody { get; private set; }
        public IDictionary<string, string> Parameters { get; private set; }
        public IList<MultipartDataItem> MultipartData { get; private set; }

        public IList<string> NeededHeaders { get; private set; }
        public IList<string> NeededCookies { get; private set; }

        public IDictionary<string, string> Headers { get; private set; }

        public string ParametersString { get { return string.Join("&", Parameters.Select(KVP => KVP.Key + "=" + KVP.Value)); } }

        public Query(QueryMethod method, string serverMethod, string queryBody = "", IDictionary<string, string> parameters = null, IList<MultipartDataItem> multipartData = null, 
                     IList<string> neededHeaders = null, IList<string> neededCookies = null, IDictionary<string, string> headers = null)
        {
            Method = method;
            ServerMethod = serverMethod;

            QueryBody = queryBody;
            Parameters = parameters == null ? new Dictionary<string, string>() : parameters;
            MultipartData = multipartData == null ? new List<MultipartDataItem>() : multipartData;

            NeededCookies = neededCookies == null ? new List<string>() : neededCookies;
            NeededHeaders = neededHeaders == null ? new List<string>() : neededHeaders;

            Headers = headers == null ? new Dictionary<string, string>() : headers;
        }
    }
}
