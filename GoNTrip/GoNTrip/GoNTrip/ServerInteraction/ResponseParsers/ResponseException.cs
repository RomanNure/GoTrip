using System;

using Android.Runtime;

using Newtonsoft.Json;

namespace GoNTrip.ServerInteraction.ResponseParsers
{
    [Preserve(AllMembers = true)]
    [JsonObject(MemberSerialization.OptOut)]
    public class ResponseException : Exception
    {
        [JsonRequired]
        public DateTime timestamp { get; set; }

        [JsonRequired]
        public string status { get; set; }

        [JsonRequired]
        public string error { get; set; }

        [JsonRequired]
        public string message { get; set; }

        [JsonRequired]
        public string path { get; set; }

        public ResponseException() { }

        public ResponseException(string message)
        {
            timestamp = DateTime.Now;
            status = "error";
            error = "error";
            this.message = message;
            path = "";
        }

        public override string ToString()
        {
            return "{ \"timestamp\" : " + JsonConvert.SerializeObject(DateTime.Now) + ", \"status\" : \"err\", \"error\" : \"" + message + "\", \"message\" : \"" + message + "\", \"path\" : \"\" }";
        }
    }
}
