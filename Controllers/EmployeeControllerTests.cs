using Microsoft.AspNetCore.Mvc;
using Moq;
using PruebaSisteCredito.Application.Controllers;
using PruebaSisteCredito.Domain.Entities;
using PruebaSisteCredito.Infrastructure.Repositories.Inter;
using Xunit;


namespace PruebaSisteCredito.Tests.Repositories{
    public class EmployeeControllerTests
    {
        private readonly Mock<IEmployeeRepository> _mockRepo;
        private readonly EmployeeController _controller;

        public EmployeeControllerTests()
        {
            _mockRepo = new Mock<IEmployeeRepository>();
            _controller = new EmployeeController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkWithEmployees()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Employee>
            {
                new Employee { Id = 1, Name = "John Doe" },
                new Employee { Id = 2, Name = "Jane Smith" }
            });

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var employees = Assert.IsAssignableFrom<IEnumerable<Employee>>(okResult.Value);
            Assert.Equal(2, employees.Count());
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFoundWhenEmployeeDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Employee?)null);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedAtActionWithEmployee()
        {
            // Arrange
            var employee = new Employee { Id = 1, Name = "John Doe" };

            // Act
            var result = await _controller.Create(employee);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdEmployee = Assert.IsType<Employee>(createdResult.Value);
            Assert.Equal("John Doe", createdEmployee.Name);
        }
    }
}