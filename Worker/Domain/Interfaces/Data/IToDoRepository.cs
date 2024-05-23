using Worker.Domain.Entities;

namespace Worker.Domain.Interfaces.Data
{
    public interface IToDoRepository
    {
        Task AddAsync(ToDo toDo);
        Task UpdateAsync(ToDo toDo);
    }
}