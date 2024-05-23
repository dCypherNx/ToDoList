using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using WebAPI.Domain.Entities;

namespace WebAPI.Services.DTO
{
    public class ToDoDTO
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] 
        public int? Id { get; set; }
        public DateTime? Date { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? Created { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? Updated { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Deleted { get; set; }


        public static explicit operator ToDoDTO(ToDo obj)
        {
           return new ToDoDTO()
            {
                Id = obj.Id,
                Date = obj.Date,
                Description = obj.Description,
                Status = obj.Status,
                Created = obj.Created,
                Updated = obj.Updated,
                Deleted = obj.Deleted
            };
        }

        public static explicit operator ToDo(ToDoDTO obj)
        {
            return new ToDo()
            {
                Id = obj.Id,
                Date = obj.Date,
                Description = obj.Description,
                Status = obj.Status,
                Created = obj.Created,
                Updated = obj.Updated,
                Deleted = obj.Deleted
            };
        }
    }
}
