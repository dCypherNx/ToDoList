using Microsoft.AspNetCore.Mvc;
using WebAPI.Domain.Interfaces.Service;
using WebAPI.Services.DTO;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace WebAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly IToDoService _toDoService;
        private readonly ILogger<ToDoController> _logger;

        public ToDoController(IToDoService toDoService, ILogger<ToDoController> logger)
        {
            _toDoService = toDoService;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult CreateToDo(ToDoDTO obj)
        {
            try
            {
                _toDoService.CreateToDo(obj);
                return Accepted();
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating ToDo");
                return StatusCode(500);
            }
        }

        [HttpGet]
        public IActionResult GetAllActiveToDos()
        {
            try
            {
                var activeToDos = _toDoService.GetAllActiveToDos();

                if (!activeToDos.Any())
                {
                    return NoContent();
                }

                return Ok(activeToDos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error retrieving active ToDo items");
                return StatusCode(500);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetToDo(int id)
        {
            try
            {
                var obj = _toDoService.GetToDoById(id);
                if (obj == null)
                {
                    return NotFound("ToDo not found");
                }

                return Ok(obj);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error retrieving ToDo item");
                return StatusCode(500);
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateToDo(int id, ToDoDTO obj)
        {
            try
            {
                _toDoService.UpdateToDo(id, obj);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating ToDo");
                return StatusCode(500);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteToDo(int id)
        {
            try
            {
                _toDoService.DeleteToDo(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting ToDo");
                return StatusCode(500);
            }
        }
    }
}
