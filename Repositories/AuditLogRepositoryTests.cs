using Moq;
using MongoDB.Driver;
using PruebaSisteCredito.Domain.Entities;
using PruebaSisteCredito.Infrastructure.Repositories.Inter;
using PruebaSisteCredito.Infrastructure.Repositories.Impl;
using Xunit;

namespace PruebaSisteCredito.Tests.Repositories{
    public class AuditLogRepositoryTests
    {
        private readonly Mock<IMongoDatabase> _mockDatabase;
        private readonly Mock<IMongoCollection<AuditLog>> _mockCollection;
        private readonly IAuditLogRepository _repository;

        public AuditLogRepositoryTests()
        {
            _mockDatabase = new Mock<IMongoDatabase>();
            _mockCollection = new Mock<IMongoCollection<AuditLog>>();
            _mockDatabase.Setup(db => db.GetCollection<AuditLog>("AuditLogs", null))
                .Returns(_mockCollection.Object);

            _repository = new AuditLogRepository(_mockDatabase.Object);
        }

        [Fact]
        public async Task InsertLogAsync_ShouldInsertLog()
        {
            // Arrange
            var log = new AuditLog
            {
                Action = "CreateEmployee",
                User = "Admin",
                Timestamp = DateTime.UtcNow,
                Details = "Created employee John Doe"
            };

            // Act
            await _repository.InsertLogAsync(log);

            // Assert
            _mockCollection.Verify(col => col.InsertOneAsync(log, null, default), Times.Once);
        }
    }
}