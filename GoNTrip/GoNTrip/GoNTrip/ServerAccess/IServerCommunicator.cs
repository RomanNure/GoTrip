namespace GoNTrip.ServerAccess
{
    public interface IServerCommunicator
    {
        string ServerURL { get; set; }
        IServerResponse SendQuery(IQuery query);
    }
}
