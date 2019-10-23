using System.Net.Http;

namespace GoNTrip.ServerAccess
{
    public class MultipartDataItem
    {
        public HttpContent Data { get; private set; }
        public string Name { get; private set; }
        public string File { get; private set; }

        public MultipartDataItem(HttpContent data, string name, string file = "")
        {
            Data = data;
            Name = name;
            File = file;
        }
    }
}
