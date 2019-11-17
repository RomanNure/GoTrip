using System.Net;
using System.Threading.Tasks;

namespace GoNTrip.ServerAccess
{
    public interface IServerCommunicator
    {
        string ServerURL { get; set; }
        Task<IServerResponse> SendQuery(IQuery query, CookieContainer container = null);
    }
}
