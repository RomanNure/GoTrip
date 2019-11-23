namespace GoNTrip.Model
{
    public class Session
    {
        public Session() { }

        public User CurrentUser { get; set; }
        public string SessionId { get; set; }
    }
}
