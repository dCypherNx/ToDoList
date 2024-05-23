using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Worker.Domain.Interfaces.Data;
using Worker.Domain.Entities;
using Worker.Services.DTO;
using Worker.Domain.Interfaces.Services;

namespace Worker.Services
{
    public class RabbitMQService : IRabbitMQService, IDisposable
    {
        private readonly ILogger<RabbitMQService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMQService(
            ILogger<RabbitMQService> logger,
            IConfiguration configuration,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            InitializeRabbitMQ();
            ConsumeMessages();
            return Task.CompletedTask;
        }

        private void InitializeRabbitMQ()
        {
            var factory = new ConnectionFactory() { HostName = _configuration["RabbitMQ:HostName"] };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _configuration["RabbitMQ:QueueName"], durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        private void ConsumeMessages()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var deliveryTag = ea.DeliveryTag;
                try
                {
                    ToDoDTO toDoDTO = MessageToDTO(ea);
                    if (IsValid(toDoDTO))
                    {
                        await ProcessMessage(toDoDTO);
                        _logger.LogInformation($"ToDo processed: {toDoDTO.Id}");
                        _channel.BasicAck(deliveryTag, false); // Confirmação manual da mensagem
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message.");
                    _channel.BasicNack(deliveryTag, false, true);
                }
            };

            _channel.BasicConsume(queue: _configuration["RabbitMQ:QueueName"], autoAck: false, consumer: consumer);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Dispose();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
        }

        private static bool IsValid(ToDoDTO obj)
        {
            return obj != null && !string.IsNullOrEmpty(obj.Description);
        }

        private async Task ProcessMessage(ToDoDTO obj)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var toDoRepository = scope.ServiceProvider.GetRequiredService<IToDoRepository>();
                var toDo = (ToDo)obj;

                if (toDo.Id is null)
                {
                    _logger.LogDebug("Updating ToDo.");
                    await toDoRepository.AddAsync(toDo);
                }
                else
                {
                    _logger.LogDebug("Updating ToDo.");
                    await toDoRepository.UpdateAsync(toDo);
                }
            }
        }

        private ToDoDTO MessageToDTO(BasicDeliverEventArgs ea)
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            _logger.LogInformation($"Received message: {message}");
            return JsonSerializer.Deserialize<ToDoDTO>(message);
        }

    }
}
