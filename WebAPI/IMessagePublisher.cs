namespace WebAPI.Infrastructure.Messaging
{
    public interface IMessagePublisher
    {
        void Publish(string message);
    }
}
