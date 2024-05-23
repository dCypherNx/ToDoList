using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Moq;
using WebAPI.Domain.Entities;
using WebAPI.Domain.Interfaces.Data;
using WebAPI.Infrastructure.Messaging;
using WebAPI.Services;
using WebAPI.Services.DTO;
using Xunit;

namespace WebAPI.Tests.Services
{
    public class ToDoServiceTests
    {
        [Fact]
        public void CreateToDo_ValidObject_CallsRepositoryAddAndMessagePublisherPublish()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ToDoService>>();
            var mockRepository = new Mock<IToDoRepository>();
            var mockMessagePublisher = new Mock<IMessagePublisher>();
            var service = new ToDoService(mockLogger.Object, mockRepository.Object, mockMessagePublisher.Object);
            var toDoDTO = new ToDoDTO { Description = "Test ToDo", Status = 0, Date = DateTime.Now };

            // Act
            service.CreateToDo(toDoDTO);

            // Assert
            mockMessagePublisher.Verify(publisher => publisher.Publish(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void GetAllActiveToDos_ReturnsListOfToDoDTO()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ToDoService>>();
            var mockRepository = new Mock<IToDoRepository>();
            var mockMessagePublisher = new Mock<IMessagePublisher>();
            var service = new ToDoService(mockLogger.Object, mockRepository.Object, mockMessagePublisher.Object);
            var mockToDoList = new List<ToDo> { new ToDo { Id = 1, Description = "Test ToDo", Status = 0, Date = DateTime.Now, Deleted = false } };
            mockRepository.Setup(repo => repo.GetAll()).Returns(mockToDoList);

            // Act
            var result = service.GetAllActiveToDos();

            // Assert
            var returnedList = Assert.IsAssignableFrom<IEnumerable<ToDoDTO>>(result);
            Assert.Single(returnedList);
        }

        [Fact]
        public void GetToDoById_ExistingId_ReturnsToDoDTO()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ToDoService>>();
            var mockRepository = new Mock<IToDoRepository>();
            var mockMessagePublisher = new Mock<IMessagePublisher>();
            var service = new ToDoService(mockLogger.Object, mockRepository.Object, mockMessagePublisher.Object);
            var toDo = new ToDo { Id = 1, Description = "Test ToDo", Status = 0, Date = DateTime.Now };
            mockRepository.Setup(repo => repo.GetById(It.IsAny<int>())).Returns(toDo);

            // Act
            var result = service.GetToDoById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(toDo.Id, result.Id);
        }

        [Fact]
        public void UpdateToDo_ExistingId_ValidObject_CallsRepositoryUpdateAndMessagePublisherPublish()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ToDoService>>();
            var mockRepository = new Mock<IToDoRepository>();
            var mockMessagePublisher = new Mock<IMessagePublisher>();
            var service = new ToDoService(mockLogger.Object, mockRepository.Object, mockMessagePublisher.Object);
            var toDo = new ToDo { Id = 1, Description = "Test ToDo", Status = 0, Date = DateTime.Now };
            var toDoDTO = new ToDoDTO { Id = 1, Description = "Updated ToDo", Status = 1, Date = DateTime.Now };
            mockRepository.Setup(repo => repo.GetById(It.IsAny<int>())).Returns(toDo);

            // Act
            service.UpdateToDo(1, toDoDTO);

            // Assert
            mockMessagePublisher.Verify(publisher => publisher.Publish(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void DeleteToDo_ExistingId_CallsRepositoryUpdateAndMessagePublisherPublish()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ToDoService>>();
            var mockRepository = new Mock<IToDoRepository>();
            var mockMessagePublisher = new Mock<IMessagePublisher>();
            var service = new ToDoService(mockLogger.Object, mockRepository.Object, mockMessagePublisher.Object);
            var toDo = new ToDo { Id = 1, Description = "Test ToDo", Status = 0, Date = DateTime.Now };
            mockRepository.Setup(repo => repo.GetById(It.IsAny<int>())).Returns(toDo);

            // Act
            service.DeleteToDo(1);

            // Assert
            mockMessagePublisher.Verify(publisher => publisher.Publish(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void CreateToDo_NullObject_ThrowsArgumentNullException()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ToDoService>>();
            var mockRepository = new Mock<IToDoRepository>();
            var mockMessagePublisher = new Mock<IMessagePublisher>();
            var service = new ToDoService(mockLogger.Object, mockRepository.Object, mockMessagePublisher.Object);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => service.CreateToDo(null));
        }

        [Fact]
        public void GetToDoById_InvalidId_ThrowsArgumentException()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ToDoService>>();
            var mockRepository = new Mock<IToDoRepository>();
            var mockMessagePublisher = new Mock<IMessagePublisher>();
            var service = new ToDoService(mockLogger.Object, mockRepository.Object, mockMessagePublisher.Object);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => service.GetToDoById(0));
        }

        [Fact]
        public void UpdateToDo_InvalidId_ThrowsArgumentException()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ToDoService>>();
            var mockRepository = new Mock<IToDoRepository>();
            var mockMessagePublisher = new Mock<IMessagePublisher>();
            var service = new ToDoService(mockLogger.Object, mockRepository.Object, mockMessagePublisher.Object);
            var toDoDTO = new ToDoDTO { Id = 0, Description = "Test ToDo", Status = 0, Date = DateTime.Now };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => service.UpdateToDo(0, toDoDTO));
        }

        [Fact]
        public void DeleteToDo_InvalidId_ThrowsArgumentException()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ToDoService>>();
            var mockRepository = new Mock<IToDoRepository>();
            var mockMessagePublisher = new Mock<IMessagePublisher>();
            var service = new ToDoService(mockLogger.Object, mockRepository.Object, mockMessagePublisher.Object);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => service.DeleteToDo(0));
        }

        [Fact]
        public void UpdateToDo_NullObject_ThrowsArgumentException()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ToDoService>>();
            var mockRepository = new Mock<IToDoRepository>();
            var mockMessagePublisher = new Mock<IMessagePublisher>();
            var service = new ToDoService(mockLogger.Object, mockRepository.Object, mockMessagePublisher.Object);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => service.UpdateToDo(1, null));
        }

        [Fact]
        public void UpdateToDo_ToDoNotFound_ThrowsInvalidOperationException()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ToDoService>>();
            var mockRepository = new Mock<IToDoRepository>();
            mockRepository.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((ToDo)null);
            var mockMessagePublisher = new Mock<IMessagePublisher>();
            var service = new ToDoService(mockLogger.Object, mockRepository.Object, mockMessagePublisher.Object);
            var toDoDTO = new ToDoDTO { Id = 1, Description = "Test ToDo", Status = 0, Date = DateTime.Now };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => service.UpdateToDo(1, toDoDTO));
        }

        [Fact]
        public void DeleteToDo_ToDoNotFound_ThrowsInvalidOperationException()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ToDoService>>();
            var mockRepository = new Mock<IToDoRepository>();
            mockRepository.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((ToDo)null);
            var mockMessagePublisher = new Mock<IMessagePublisher>();
            var service = new ToDoService(mockLogger.Object, mockRepository.Object, mockMessagePublisher.Object);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => service.DeleteToDo(1));
        }

    }
}
