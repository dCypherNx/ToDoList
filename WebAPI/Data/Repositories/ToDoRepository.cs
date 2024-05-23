using WebAPI.Data.Contexts;
using WebAPI.Domain.Interfaces.Data;
using WebAPI.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace WebAPI.Data.Repositories
{
    [ExcludeFromCodeCoverage]
    public class ToDoRepository : IToDoRepository
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<ToDoRepository> _logger;

        public ToDoRepository(ApplicationContext context, ILogger<ToDoRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<ToDo> GetAll()
        {
            try
            {
                return _context.ToDos.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocorreu um erro ao obter todos os ToDos", ex);
                throw;
            }
        }

        public ToDo GetById(int id)
        {
            try
            {
                return _context.ToDos.FirstOrDefault(t => t.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocorreu um erro ao obter o ToDo com id: {id}", ex);
                throw;
            }
        }
    }
}