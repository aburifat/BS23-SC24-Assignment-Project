using System;
using System.Collections.Generic;
using System.Linq;
using BS23_SC24_Assignment_Backend.Context;
using BS23_SC24_Assignment_Backend.Controllers;
using BS23_SC24_Assignment_Backend.Managers.Security;
using BS23_SC24_Assignment_Backend.Models;
using BS23_SC24_Assignment_Backend.Requests;
using BS23_SC24_Assignment_Backend.Responses;
using BS23_SC24_Assignment_Backend.validators;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace BS23_SC24_Assignment_Backend.Tests
{
    public class TasksControllerTests
    {
        [Fact]
        public void GetMyTasks_Returns_OkResult_With_Tasks()
        {
            // Arrange
            var fakeTasks = new List<Tasks>
            {
                new Tasks { Id = 1, UserId = 1, Title = "Task 1", Description = "Description 1", Status = "Pending" },
                new Tasks { Id = 2, UserId = 1, Title = "Task 2", Description = "Description 2", Status = "Completed" },
            };

            var authUser = A.Fake<IAuthenticatedUser>();
            A.CallTo(() => authUser.Id).Returns(1);

            var taskValidate = A.Fake<TasksValidators>();

            var configuration = new ConfigurationBuilder()
                                .AddInMemoryCollection(new Dictionary<string, string>
                                {
                                    {"DefaultAdminUser:UserName", "admin"},
                                    {"DefaultAdminUser:Email", "superadmin@gmail.com"},
                                    {"DefaultAdminUser:Password", "This(1sAD3m0)paSS"},
                                })
                                .Build();

            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connection)
                .Options;

            var dbContext = new AppDbContext(options, A.Dummy<IConfiguration>());

            dbContext.Database.EnsureCreated();

            dbContext.Tasks.AddRange(fakeTasks);
            dbContext.SaveChanges();

            var controller = new TasksController(dbContext, authUser, taskValidate);

            // Act
            var result = controller.GetMyTasks();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var tasks = Assert.IsAssignableFrom<IEnumerable<GetTaskResponse>>(okResult.Value);
            Assert.Equal(fakeTasks.Count, tasks.Count());
            Assert.Equal(fakeTasks[0].Title, tasks.ElementAt(0).Title);
            Assert.Equal(fakeTasks[1].Title, tasks.ElementAt(1).Title);
        }
    }
}
