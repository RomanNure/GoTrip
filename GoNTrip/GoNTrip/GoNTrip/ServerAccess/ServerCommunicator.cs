using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GoNTrip.ServerAccess
{
    public class ServerCommunicator : IServerCommunicator
    {
        public const string SERVER_URL = "https://go-trip.herokuapp.com";

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
            Dictionary<string, string> Headers = new Dictionary<string, string>();
            try
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                using (StreamReader str = new StreamReader(response.GetResponseStream()))
                {
                    foreach (string neededHeaderName in neededHeadersNames)
                        if (response.Headers.AllKeys.ToList().Contains(neededHeaderName))
                            Headers.Add(neededHeaderName, response.Headers[neededHeaderName]);

                    return (str.ReadToEnd(), Headers);
                }
            }
            catch(WebException wex)
            {
                try
                {
                    string error = "";
                    using (StreamReader str = new StreamReader(wex.Response.GetResponseStream()))
                        error = str.ReadToEnd();
                    return (error, Headers);
                }
                catch(Exception ex)
                {
                    return (ex.Message, Headers);
                }
            }
            catch(Exception ex)
            {
                return (ex.Message, Headers);
            }
        }
    }
}
