using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using WebAPI.Domain.Interfaces.Service;
using WebAPI.Presentation.Controllers;
using WebAPI.Services.DTO;

namespace WebAPI.Tests.Controllers
{
    public class ToDoControllerTests
    {
        [Fact]
        public void CreateToDo_ValidObject_ReturnsAcceptedResult()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ToDoController>>();
            var mockToDoService = new Mock<IToDoService>();
            var controller = new ToDoController(mockToDoService.Object, mockLogger.Object);
            var toDoDTO = new ToDoDTO { Description = "Test ToDo", Status = 0, Date = DateTime.Now };

            // Act
            var result = controller.CreateToDo(toDoDTO);

            // Assert
            var acceptedResult = Assert.IsType<AcceptedResult>(result);
            Assert.Equal(202, acceptedResult.StatusCode);
        }

        [Fact]
        public void GetAllActiveToDos_ReturnsOkResultWithListOfToDoDTO()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ToDoController>>();
            var mockToDoService = new Mock<IToDoService>();
            var controller = new ToDoController(mockToDoService.Object, mockLogger.Object);
            var mockToDoList = new List<ToDoDTO> { new ToDoDTO { Id = 1, Description = "Test ToDo", Status = 0, Date = DateTime.Now } };
            mockToDoService.Setup(service => service.GetAllActiveToDos()).Returns(mockToDoList);

            // Act
            var result = controller.GetAllActiveToDos();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedList = Assert.IsAssignableFrom<IEnumerable<ToDoDTO>>(okResult.Value);
            Assert.Single(returnedList);
        }

        [Fact]
        public void GetToDo_ExistingId_ReturnsOkResultWithToDoDTO()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ToDoController>>();
            var mockToDoService = new Mock<IToDoService>();
            var controller = new ToDoController(mockToDoService.Object, mockLogger.Object);
            var toDoDTO = new ToDoDTO { Id = 1, Description = "Test ToDo", Status = 0, Date = DateTime.Now };
            mockToDoService.Setup(service => service.GetToDoById(It.IsAny<int>())).Returns(toDoDTO);

            // Act
            var result = controller.GetToDo(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedToDo = Assert.IsType<ToDoDTO>(okResult.Value);
            Assert.Equal(toDoDTO.Id, returnedToDo.Id);
        }

        [Fact]
        public void UpdateToDo_ValidObject_ReturnsNoContentResult()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ToDoController>>();
            var mockToDoService = new Mock<IToDoService>();
            var controller = new ToDoController(mockToDoService.Object, mockLogger.Object);
            var toDoDTO = new ToDoDTO { Id = 1, Description = "Test ToDo", Status = 0, Date = DateTime.Now };

            // Act
            var result = controller.UpdateToDo(1, toDoDTO);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void DeleteToDo_ExistingId_ReturnsNoContentResult()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ToDoController>>();
            var mockToDoService = new Mock<IToDoService>();
            var controller = new ToDoController(mockToDoService.Object, mockLogger.Object);

            // Act
            var result = controller.DeleteToDo(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void CreateToDo_ExceptionOccurs_ReturnsStatusCode500()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ToDoController>>();
            var mockToDoService = new Mock<IToDoService>();
            mockToDoService.Setup(service => service.CreateToDo(It.IsAny<ToDoDTO>())).Throws(new Exception());
            var controller = new ToDoController(mockToDoService.Object, mockLogger.Object);
            var toDoDTO = new ToDoDTO { Description = "Test ToDo", Status = 0, Date = DateTime.Now };

            // Act
            var result = controller.CreateToDo(toDoDTO);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public void GetAllActiveToDos_ExceptionOccurs_ReturnsStatusCode500()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ToDoController>>();
            var mockToDoService = new Mock<IToDoService>();
            mockToDoService.Setup(service => service.GetAllActiveToDos()).Throws(new Exception());
            var controller = new ToDoController(mockToDoService.Object, mockLogger.Object);

            // Act
            var result = controller.GetAllActiveToDos();

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public void UpdateToDo_NullObject_ReturnsBadRequestResult()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ToDoController>>();
            var mockToDoService = new Mock<IToDoService>();
            mockToDoService
                .Setup(service => service.UpdateToDo(It.IsAny<int>(), null))
                .Throws(new ArgumentException("Invalid ToDo object")); // Lança ArgumentException com a mensagem correta
            var controller = new ToDoController(mockToDoService.Object, mockLogger.Object);

            // Act
            var result = controller.UpdateToDo(1, null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid ToDo object", badRequestResult.Value);
        }

        [Fact]
        public void UpdateToDo_InvalidId_ReturnsBadRequestResult()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ToDoController>>();
            var mockToDoService = new Mock<IToDoService>();
            mockToDoService.Setup(service => service.UpdateToDo(It.IsAny<int>(), It.IsAny<ToDoDTO>())).Throws<ArgumentException>();
            var controller = new ToDoController(mockToDoService.Object, mockLogger.Object);
            var toDoDTO = new ToDoDTO { Description = "Test ToDo", Status = 0, Date = DateTime.Now };

            // Act
            var result = controller.UpdateToDo(0, toDoDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void UpdateToDo_ExceptionOccurs_ReturnsStatusCode500()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ToDoController>>();
            var mockToDoService = new Mock<IToDoService>();
            mockToDoService.Setup(service => service.UpdateToDo(It.IsAny<int>(), It.IsAny<ToDoDTO>())).Throws(new Exception());
            var controller = new ToDoController(mockToDoService.Object, mockLogger.Object);
            var toDoDTO = new ToDoDTO { Description = "Test ToDo", Status = 0, Date = DateTime.Now };

            // Act
            var result = controller.UpdateToDo(1, toDoDTO);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public void DeleteToDo_InvalidId_ReturnsBadRequestResult()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ToDoController>>();
            var mockToDoService = new Mock<IToDoService>();
            mockToDoService.Setup(service => service.DeleteToDo(It.IsAny<int>())).Throws<ArgumentException>();
            var controller = new ToDoController(mockToDoService.Object, mockLogger.Object);

            // Act
            var result = controller.DeleteToDo(0); // ID inválido

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void DeleteToDo_ExceptionOccurs_ReturnsStatusCode500()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ToDoController>>();
            var mockToDoService = new Mock<IToDoService>();
            mockToDoService.Setup(service => service.DeleteToDo(It.IsAny<int>())).Throws(new Exception());
            var controller = new ToDoController(mockToDoService.Object, mockLogger.Object);

            // Act
            var result = controller.DeleteToDo(1);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public void GetToDo_NotExistingId_ReturnsNotFoundResult()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ToDoController>>();
            var mockToDoService = new Mock<IToDoService>();
            var controller = new ToDoController(mockToDoService.Object, mockLogger.Object);
            mockToDoService.Setup(service => service.GetToDoById(It.IsAny<int>())).Returns((ToDoDTO)null); // Simula um retorno nulo da service

            // Act
            var result = controller.GetToDo(1); // Passa um ID que não existe

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("ToDo not found", notFoundResult.Value);
        }

        [Fact]
        public void GetToDo_ExceptionOccurs_ReturnsStatusCode500()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ToDoController>>();
            var mockToDoService = new Mock<IToDoService>();
            mockToDoService.Setup(service => service.GetToDoById(It.IsAny<int>())).Throws(new Exception()); // Simula uma exceção na service
            var controller = new ToDoController(mockToDoService.Object, mockLogger.Object);

            // Act
            var result = controller.GetToDo(1); // Passa um ID válido

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public void GetAllActiveToDos_NoActiveToDos_ReturnsNoContentResult()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ToDoController>>();
            var mockToDoService = new Mock<IToDoService>();
            var controller = new ToDoController(mockToDoService.Object, mockLogger.Object);
            mockToDoService.Setup(service => service.GetAllActiveToDos()).Returns(new List<ToDoDTO>()); // Simula uma lista vazia

            // Act
            var result = controller.GetAllActiveToDos();

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, noContentResult.StatusCode);
        }

        [Fact]
        public void CreateToDo_NullObject_ThrowsArgumentNullException()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ToDoController>>();
            var mockToDoService = new Mock<IToDoService>();
            mockToDoService
                .Setup(service => service.CreateToDo(null))
                .Throws(new ArgumentNullException("obj", "ToDo data cannot be null"));
            var controller = new ToDoController(mockToDoService.Object, mockLogger.Object);

            // Act
            var result = controller.CreateToDo(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(result, badRequestResult);
        }

    }
}
