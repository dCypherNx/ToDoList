using Worker.Data.Contexts;
using Worker.Domain.Interfaces.Data;
using Worker.Domain.Entities;

namespace Worker.Data.Repositories
{
    public class ToDoRepository : IToDoRepository
    {
        private readonly ApplicationContext _context;

        public ToDoRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ToDo toDo)
        {
            if (toDo == null)
                throw new ArgumentNullException(nameof(toDo));

            toDo.Created = DateTime.UtcNow; // Definindo a data de criação
            toDo.Deleted = false; // Novos itens não estão deletados
            await _context.ToDos.AddAsync(toDo);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ToDo toDo)
        {
            if (toDo == null)
                throw new ArgumentNullException(nameof(toDo));

            var existingToDo = await _context.ToDos.FindAsync(toDo.Id);
            if (existingToDo == null)
                throw new InvalidOperationException("ToDo not found.");

            existingToDo.Updated = DateTime.UtcNow;
            existingToDo.Deleted = toDo.Deleted;
            existingToDo.Status = toDo.Status;

            _context.ToDos.Update(existingToDo);
            await _context.SaveChangesAsync();
        }
    }
}