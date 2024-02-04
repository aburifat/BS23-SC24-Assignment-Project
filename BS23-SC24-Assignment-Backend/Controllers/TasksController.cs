using BS23_SC24_Assignment_Backend.Managers.Task;
using BS23_SC24_Assignment_Backend.Requests;
using BS23_SC24_Assignment_Backend.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BS23_SC24_Assignment_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController(ITaskManager taskManager) : ControllerBase
    {
        ITaskManager _taskManager = taskManager;

        [Authorize]
        [HttpGet]
        public IActionResult GetMyTasks()
        {
            GetTaskListResponse response = _taskManager.GetMyTasks();
            return StatusCode(response.StatusCode, response);
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetTaskById([FromRoute] long id)
        {
            GetTaskResponse response = _taskManager.GetTaskById(id);
            return StatusCode(response.StatusCode, response);
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("All")]
        public IActionResult GetAllTasks()
        {
            GetTaskListResponse response = _taskManager.GetAllTasks();
            return StatusCode(response.StatusCode, response);
        }

        [Authorize]
        [HttpPost]
        public IActionResult PostTask(CreateUpdateTaskRequest request)
        {
            GetTaskResponse response = _taskManager.PostTask(request);
            return StatusCode(response.StatusCode, response);
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult UpdateTask([FromRoute] long id, [FromBody] CreateUpdateTaskRequest request)
        {
            GetTaskResponse response = _taskManager.UpdateTask(id, request);
            return StatusCode(response.StatusCode, response);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteTask([FromRoute] long id)
        {
            BaseResponse response = _taskManager.DeleteTask(id);
            return StatusCode(response.StatusCode, response);
        }


    }
}
