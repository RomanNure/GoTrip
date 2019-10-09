using System;
using Newtonsoft.Json;

namespace GoNTrip.ServerInteraction.ResponseParsers
{
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

        public static bool IsObjectResponseException(dynamic obj)
        {
            return obj.timestamp != null && obj.status != null && obj.error != null && obj.message != null && obj.path != null;
        }
    }
}
