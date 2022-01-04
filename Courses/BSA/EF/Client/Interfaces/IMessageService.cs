namespace Client.Interfaces
{
    public interface IMessageService
    {
        event System.EventHandler<EventArgs.SerealizationEventArgs> Received;

    }
}
