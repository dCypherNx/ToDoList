using Worker.Domain.Interfaces.Services;

namespace Worker.Services
{
    public class WorkerService : BackgroundService
    {
        private readonly ILogger<WorkerService> _logger;
        private readonly IServiceProvider _services;

        public WorkerService(ILogger<WorkerService> logger, IServiceProvider services)
        {
            _logger = logger;
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            using (var scope = _services.CreateScope())
            {
                var rabbitMQService = scope.ServiceProvider.GetRequiredService<IRabbitMQService>();
                await rabbitMQService.StartAsync(stoppingToken);

                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }

                await rabbitMQService.StopAsync(stoppingToken);
            }
        }
    }
}
