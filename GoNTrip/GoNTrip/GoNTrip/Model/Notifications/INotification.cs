namespace GoNTrip.Model.Notifications
{
    public interface INotification : ModelElement
    {
        string Id { get; }
        bool IsChecked { get; }
        bool IsConfirmable { get; }
        string Topic { get; }
        string ServerMethodConfirm { get; }
        string ServerMethodRefuse { get; }

        string GetPreviewMessage();
        string GetDetailMessage();
    }
}
