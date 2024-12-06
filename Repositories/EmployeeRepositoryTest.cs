
using Moq;
using PruebaSisteCredito.Domain.Entities;
using PruebaSisteCredito.Infrastructure.Repositories.Impl;
using PruebaSisteCredito.Infrastructure.Repositories.Inter;
using Xunit;

namespace PruebaSisteCredito.Tests.Repositories{

    public class EmployeeRepositoryTest
    {
        private readonly IEmployeeRepository _repository = new EmployeeRepository();

        [Fact]
        public async Task AddAsync_ShouldAddEmployee()
        {
            // Arrange
            var employee = new Employee
            {
                Id = 1,
                Name = "John Doe",
                Position = "Developer",
                Area = "IT",
                IsManager = false
            };

            // Act
            await _repository.AddAsync(employee);
            var result = await _repository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John Doe", result!.Name);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllEmployees()
        {
            // Arrange
            await _repository.AddAsync(new Employee { Id = 1, Name = "Employee 1" });
            await _repository.AddAsync(new Employee { Id = 2, Name = "Employee 2" });

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveEmployee()
        {
            // Arrange
            var employee = new Employee { Id = 1, Name = "John Doe" };
            await _repository.AddAsync(employee);

            // Act
            await _repository.DeleteAsync(1);
            var result = await _repository.GetByIdAsync(1);

            // Assert
            Assert.Null(result);
        }
    }
}
