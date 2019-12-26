using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using GoNTrip.ServerInteraction.ResponseParsers;

namespace GoNTrip.ServerAccess
{
    public class ServerCommunicator : IServerCommunicator
    {
        public const string SERVER_URL = "https://go-trip.herokuapp.com";//"http://93.76.235.211:5000";
        public const string MULTIPART_SERVER_URL = "http://vvkyrychenko.zzz.com.ua/gotrip";

        public const string APP_JSON = "application/json";
        public const string APP_URLENCODED = "application/x-www-form-urlencoded";
        public const string MULTIPART_FORM = "multipart/form-data";

        private string serverUrl = "";
        public string ServerURL { get { return serverUrl; } set { serverUrl = LastSlash.Replace(value, ""); } }

        private Regex FirstSlash = new Regex("^/");
        private Regex LastSlash = new Regex("/$");

        public ServerCommunicator(string serverUrl = SERVER_URL) => ServerURL = serverUrl;

        public async Task<IServerResponse> SendQuery(IQuery query, CookieContainer container = null)
        {
            IDictionary<string, string> headers = new Dictionary<string, string>();

            try
            {
                string queryUrl = (query.Method == QueryMethod.POST_MULTIPART ? MULTIPART_SERVER_URL : ServerURL) + "/" + 
                    LastSlash.Replace(FirstSlash.Replace(query.ServerMethod, ""), "");

                (string data, IDictionary<string, string> headers, CookieContainer container) response = (null, null, null);

                if (query.Method == QueryMethod.GET)
                    response = await QueryGET(queryUrl, query, container);
                else if (query.Method == QueryMethod.POST)
                    response = await QueryPOST(queryUrl, query, container, false);
                else if(query.Method == QueryMethod.POST_URLENCODED)
                    response = await QueryPOST(queryUrl, query, container, true);
                else if (query.Method == QueryMethod.POST_MULTIPART)
                    response = await QueryPostMultipart(queryUrl, query, container);

                return new ServerResponse(response.data, response.headers, response.container);
            }
            catch (WebException wex)
            {
                try
                {
                    string error = "";
                    using (StreamReader str = new StreamReader(wex.Response.GetResponseStream()))
                        error = str.ReadToEnd();
                    return new ServerResponse(error, headers, container);
                }
                catch (Exception ex)
                {
                    return new ServerResponse(new ResponseException("No Internet connection").ToString(), headers, container);
                }
            }
            catch (Exception ex)
            {
                return new ServerResponse(ex.Message, headers, container);
            }
        }

        private async Task<(string, Dictionary<string, string>, CookieContainer)> QueryGET(string url, IQuery query, CookieContainer container)
        {
            string parameterString = query.ParametersString;

            HttpWebRequest request = WebRequest.CreateHttp(url + (parameterString == "" ? "" : "?" + parameterString));
            request.Method = WebRequestMethods.Http.Get;
            request.ContentType = APP_URLENCODED;

            AddHeaders(request, query.Headers);
            CookieContainer cont = AddCookies(query, request, container);

            return await GetResponse(request, query.NeededHeaders, query.NeededCookies, cont);
        }

        private async Task<(string, Dictionary<string, string>, CookieContainer)> QueryPOST(string url, IQuery query, CookieContainer container, bool urlEncoded)
        {
            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.Method = WebRequestMethods.Http.Post;
            request.ContentType = urlEncoded ? APP_URLENCODED : APP_JSON;

            byte[] queryBodyData = Encoding.UTF8.GetBytes(urlEncoded ? query.ParametersString : query.QueryBody);
            request.ContentLength = queryBodyData.Length;

            AddHeaders(request, query.Headers);
            CookieContainer cont = AddCookies(query, request, container);

            using (Stream strw = await request.GetRequestStreamAsync())
                strw.Write(queryBodyData, 0, queryBodyData.Length);

            return await GetResponse(request, query.NeededHeaders, query.NeededCookies, cont);
        }

        private async Task<(string, Dictionary<string, string>, CookieContainer)> QueryPostMultipart(string url, IQuery query, CookieContainer container)
        {
            MultipartFormDataContent form = new MultipartFormDataContent("-----");

            foreach (MultipartDataItem dataItem in query.MultipartData)
                if (dataItem.File == "")
                    form.Add(dataItem.Data, dataItem.Name);
                else
                    form.Add(dataItem.Data, dataItem.Name, dataItem.File);

            byte[] bytes = await form.ReadAsByteArrayAsync();

            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.Method = WebRequestMethods.Http.Post;
            request.ContentType = MULTIPART_FORM + ";boundary=-----";
            request.ContentLength = bytes.Length;

            AddHeaders(request, query.Headers);
            CookieContainer cont = AddCookies(query, request, container);

            using (Stream strw = await request.GetRequestStreamAsync())
                strw.Write(bytes, 0, bytes.Length);

            return await GetResponse(request, query.NeededHeaders, query.NeededCookies, cont);
        }

        protected async Task<(string, Dictionary<string, string>, CookieContainer)> GetResponse(HttpWebRequest request, IList<string> neededHeadersNames, 
                                                                                                IList<string> neededCookies, CookieContainer container)
        {
            using (HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse)
            using (StreamReader str = new StreamReader(response.GetResponseStream()))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>();
                foreach (string neededHeaderName in neededHeadersNames)
                    if (response.Headers.AllKeys.ToList().Contains(neededHeaderName))
                        headers.Add(neededHeaderName, response.Headers[neededHeaderName]);

                List<Cookie> cookies = new List<Cookie>();
                foreach (string neededCookie in neededCookies)
                    if (response.Cookies[neededCookie] != null)
                        request.CookieContainer?.SetCookies(new Uri(ServerURL), $"{response.Cookies[neededCookie].Name}={response.Cookies[neededCookie].Value}");

                return (str.ReadToEnd(), headers, container);
            }
        }

        protected void AddHeaders(HttpWebRequest request, IDictionary<string, string> headers)
        {
            foreach (KeyValuePair<string, string> header in headers)
                request.Headers.Add(header.Key, header.Value);
        }

        protected CookieContainer AddCookies(IQuery query, HttpWebRequest request, CookieContainer container)
        {
            CookieContainer cont = container != null ? container : (query.NeededCookies.Count != 0 ? new CookieContainer() : null);
            request.CookieContainer = cont;
            return cont;
        }
    }
}
