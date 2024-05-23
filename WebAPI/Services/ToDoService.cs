using System.Text.Json;
using WebAPI.Domain.Interfaces.Data;
using WebAPI.Domain.Interfaces.Service;
using WebAPI.Infrastructure.Messaging;
using WebAPI.Services.DTO;

namespace WebAPI.Services
{
    public class ToDoService : IToDoService
    {
        private readonly ILogger<ToDoService> _logger;
        private readonly IToDoRepository _toDoRepository;
        private readonly IMessagePublisher _messagePublisher;

        public ToDoService(ILogger<ToDoService> logger, IToDoRepository toDoRepository, IMessagePublisher messagePublisher)
        {
            _logger = logger;
            _toDoRepository = toDoRepository;
            _messagePublisher = messagePublisher;
        }

        public void CreateToDo(ToDoDTO obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj), "ToDo data cannot be null");
            }

            string message = JsonSerializer.Serialize(obj);
            // Implementar validação e consistência de obj

            _messagePublisher.Publish(message);

            _logger.LogInformation($"ToDo create enqueued:\n {message}");
        }

        public IEnumerable<ToDoDTO> GetAllActiveToDos()
        {
            var allToDos = _toDoRepository.GetAll();
            var activeToDos = allToDos.Where(t => (bool)!t.Deleted).ToList();

            return activeToDos.Select(toDo => (ToDoDTO)toDo).ToList();
        }

        public ToDoDTO GetToDoById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ToDo ID");
            }

            var toDo = _toDoRepository.GetById(id);
            return toDo != null ? (ToDoDTO)toDo : null;
        }

        public void UpdateToDo(int id, ToDoDTO obj)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ToDo ID");
            }

            if (obj == null)
            {
                throw new ArgumentException("Invalid ToDo object");
            }

            var toDo = _toDoRepository.GetById(id);
            if (toDo == null)
            {
                throw new InvalidOperationException("ToDo not found");
            }

            toDo.Status = obj.Status;
            string message = JsonSerializer.Serialize(toDo);
            _messagePublisher.Publish(message);

            _logger.LogInformation($"ToDo updated enqueued:\n {message}");
        }

        public void DeleteToDo(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid ToDo ID");
            }

            var toDo = _toDoRepository.GetById(id);
            if (toDo == null)
            {
                throw new InvalidOperationException("ToDo not found");
            }

            toDo.Deleted = true;
            string message = JsonSerializer.Serialize(toDo);
            _messagePublisher.Publish(message);
            _logger.LogInformation($"ToDo deleted enqueued:\n {message}");
        }
    }
}
