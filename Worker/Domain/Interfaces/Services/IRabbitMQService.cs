namespace Worker.Domain.Interfaces.Services
{
    public interface IRabbitMQService
    {
        Task StartAsync(CancellationToken stoppingToken);
        Task StopAsync(CancellationToken stoppingToken);
    }
}