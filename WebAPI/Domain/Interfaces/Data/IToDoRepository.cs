using WebAPI.Domain.Entities;

namespace WebAPI.Domain.Interfaces.Data
{
    public interface IToDoRepository
    {
        ToDo GetById(int id);
        IEnumerable<ToDo> GetAll();
    }
}
