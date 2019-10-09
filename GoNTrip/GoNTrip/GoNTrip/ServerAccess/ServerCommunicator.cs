using System.IO;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace GoNTrip.ServerAccess
{
    public class ServerCommunicator : IServerCommunicator
    {
        public const string SERVER_URL = "http://93.76.235.211:5000";

        private string serverUrl = "";
        public string ServerURL { get { return serverUrl; } set { serverUrl = LastSlash.Replace(value, ""); } }

        private Regex FirstSlash = new Regex("^/");
        private Regex LastSlash = new Regex("/$");

        public ServerCommunicator(string serverUrl = SERVER_URL) => ServerURL = serverUrl;

        public IServerResponse SendQuery(IQuery query)
        {
            string queryUrl = ServerURL + "/" + LastSlash.Replace(FirstSlash.Replace(query.ServerMethod, ""), "") + (query.ParametersString == "" ? "" : "?" + query.ParametersString);

            (string data, IDictionary<string, string> headers) response;
            if (query.Method == QueryMethod.GET)
                response = QueryGET(queryUrl, query.ParametersString, query.NeededHeaders);
            else
                response = QueryPOST(queryUrl, query.QueryBody, query.NeededHeaders);

            return new ServerResponse(response.data, response.headers);
        }

        private (string data, Dictionary<string, string> headers) QueryGET(string url, string parametersString, IList<string> neededHeadersNames)
        {
            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.Method = WebRequestMethods.Http.Get;

            return GetResponse(request, neededHeadersNames);
        }

        private (string data, Dictionary<string, string> headers) QueryPOST(string url, string queryBody, IList<string> neededHeadersNames)
        {
            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.Method = WebRequestMethods.Http.Post;
            request.ContentType = "application/x-www-form-urlencoded";

            byte[] queryBodyData = Encoding.UTF8.GetBytes(queryBody);
            request.ContentLength = queryBodyData.Length;

            using (Stream strw = request.GetRequestStream())
                strw.Write(queryBodyData, 0, queryBodyData.Length);

            return GetResponse(request, neededHeadersNames);
        }

        private (string, Dictionary<string, string>) GetResponse(HttpWebRequest request, IList<string> neededHeadersNames)
        {
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            using (StreamReader str = new StreamReader(response.GetResponseStream()))
            {
                Dictionary<string, string> Headers = new Dictionary<string, string>();
                foreach (string neededHeaderName in neededHeadersNames)
                    if(response.Headers.AllKeys.ToList().Contains(neededHeaderName))
                        Headers.Add(neededHeaderName, response.Headers[neededHeaderName]);

                return (str.ReadToEnd(), Headers);
            }
        }
    }
}
