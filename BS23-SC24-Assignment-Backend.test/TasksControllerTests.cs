using BS23_SC24_Assignment_Backend.Controllers;
using BS23_SC24_Assignment_Backend.Managers.Task;
using BS23_SC24_Assignment_Backend.Responses;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;

namespace BS23_SC24_Assignment_Backend.Tests
{
    public class TasksControllerTests
    {
        [Fact]
        public void GetMyTasks_Returns_OkResult_With_Tasks()
        {
            // Arrange
            IList<GetTaskResponse> fakeTasks = new List<GetTaskResponse>
            {
                new GetTaskResponse { Id = 1, UserId = 1, Title = "Task 1", Description = "Description 1", Status = "Pending" },
                new GetTaskResponse { Id = 2, UserId = 1, Title = "Task 2", Description = "Description 2", Status = "Completed" },
            };

            ITaskManager taskManager = A.Fake<ITaskManager>();
            A.CallTo(() => taskManager.GetMyTasks()).Returns(new GetTaskListResponse
            {
                StatusCode = 200,
                IsValid = true,
                Message = "",
                TaskList = fakeTasks
            });

            var controller = new TasksController(taskManager);

            // Act
            var result = controller.GetMyTasks();

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            var value = Assert.IsAssignableFrom<GetTaskListResponse>(objectResult.Value);
            var tasks = Assert.IsAssignableFrom<IEnumerable<GetTaskResponse>>(value.TaskList);
            Assert.Equal(fakeTasks.Count, tasks.Count());
            Assert.Equal(fakeTasks[0].Title, tasks.ElementAt(0).Title);
            Assert.Equal(fakeTasks[1].Title, tasks.ElementAt(1).Title);
        }
    }
}
