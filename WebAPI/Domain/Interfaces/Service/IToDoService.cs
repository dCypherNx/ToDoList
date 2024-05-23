using WebAPI.Services.DTO;

namespace WebAPI.Domain.Interfaces.Service
{
    public interface IToDoService
    {
        void CreateToDo(ToDoDTO obj);
        IEnumerable<ToDoDTO> GetAllActiveToDos();
        ToDoDTO GetToDoById(int id);
        void UpdateToDo(int id, ToDoDTO obj);
        void DeleteToDo(int id);
    }
}