using System;

using Client.EventArgs;

using Core.DataTransferObjects.RabbitMQ;

using Microsoft.AspNetCore.SignalR.Client;

namespace Client.Services
{
    public class HubMessageService : Interfaces.IMessageService, IDisposable
    {
        // FIELDS
        HubConnection hubConnection;

        // EVENTS
        public event EventHandler<SerealizationEventArgs> Received;

        // CONSTRUCTORS
        public HubMessageService(string url)
        {
            hubConnection = new HubConnectionBuilder().WithUrl(url).Build();

            hubConnection.StartAsync();
            hubConnection.On<SerializationResult>("NewMessage", OnMesegeReceived);            
        }
        public void Dispose()
        {
            hubConnection.StopAsync();
            hubConnection.DisposeAsync();
        }

        // METHODS
        protected void OnMesegeReceived(SerializationResult message)
        {
            Received?.Invoke(this, new SerealizationEventArgs(message));
        }
    }
}
