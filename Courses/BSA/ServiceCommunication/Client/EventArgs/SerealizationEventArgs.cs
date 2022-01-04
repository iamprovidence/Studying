using SerializationResult = Core.DataTransferObjects.RabbitMQ.SerializationResult;

namespace Client.EventArgs
{
    public class SerealizationEventArgs : System.EventArgs
    {
        public SerealizationEventArgs(SerializationResult serializationResult)
        {
            SerializationResult = serializationResult ?? throw new System.ArgumentNullException(nameof(serializationResult));
        }

        public SerializationResult SerializationResult { get; private set; }
    }
}
