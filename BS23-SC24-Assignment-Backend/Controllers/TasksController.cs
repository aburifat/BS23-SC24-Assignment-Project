using BS23_SC24_Assignment_Backend.Context;
using BS23_SC24_Assignment_Backend.Managers.Security;
using BS23_SC24_Assignment_Backend.Models;
using BS23_SC24_Assignment_Backend.Requests;
using BS23_SC24_Assignment_Backend.Responses;
using BS23_SC24_Assignment_Backend.validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BS23_SC24_Assignment_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController(AppDbContext context, IAuthenticatedUser authenticatedUser, TasksValidators tasksValidators) : ControllerBase
    {
        private readonly AppDbContext _context = context;
        private readonly IAuthenticatedUser _authenticatedUser = authenticatedUser;
        private readonly TasksValidators _tasksValidators = tasksValidators;

        [Authorize]
        [HttpGet]
        public IActionResult GetMyTasks()
        {
            try
            {
                List <GetTaskResponse> response =
                [
                    .. _context.Tasks
                    .Where(x => x.UserId == _authenticatedUser.Id)
                    .Select(task => new GetTaskResponse
                    {
                        IsValid = true,
                        Message = "",
                        Id = task.Id,
                        Title = task.Title,
                        Description = task.Description,
                        Status = task.Status,
                        UserId = task.UserId
                    })
                ];
               
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetTaskById([FromRoute] long id)
        {
            try
            {
                GetTaskResponse response = new GetTaskResponse
                {
                    IsValid = false,
                    Message = "You don't have the permission to view the task!"
                };

                var task = _context.Tasks
                    .Where(x => x.Id == id)
                    .FirstOrDefault();

                if(task != null && (task.Id == _authenticatedUser.Id || _authenticatedUser.UserRole == Enums.UserRole.Administrator))
                {
                    response = new GetTaskResponse
                    {
                        IsValid = true,
                        Message = "Task successfully fetched.",
                        Id = task.Id,
                        Title = task.Title,
                        Description = task.Description,
                        Status = task.Status,
                        UserId = task.UserId
                    };
                }
                if (response.IsValid)
                {
                    return Ok(response);
                }
                else
                {
                    return Unauthorized(response);
                }
            }
            catch (Exception ex)
            {
                GetTaskResponse response = new GetTaskResponse
                {
                    IsValid = false,
                    Message = ex.Message
                };
                return BadRequest(response);
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet("All")]
        public IActionResult GetAllTasks()
        {
            try
            {
                List<GetTaskResponse> response =
                [
                    .. _context.Tasks
                    .Select(task => new GetTaskResponse
                    {
                        IsValid = true,
                        Message = "Task updated successfully",
                        Id = task.Id,
                        Title = task.Title,
                        Description = task.Description,
                        Status = task.Status,
                        UserId = task.UserId
                    })
                ];

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult PostTask(CreateUpdateTaskRequest request)
        {
            ValidationResponse validationResponse = _tasksValidators.CreateUpdateTasksValidator(request); // validation for the tasks input

            if (!validationResponse.IsValid)
            {
                return BadRequest(validationResponse);
            }

            try
            {
                Tasks task = new()
                {
                    Title = request.Title,
                    Description = request.Description,
                    Status = request.Status,
                    UserId = _authenticatedUser.Id
                };
                _context.Tasks.Add(task);
                _context.SaveChanges();

                GetTaskResponse response = new()
                {
                    IsValid = true,
                    Message = "Task created successfully",
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    Status = task.Status,
                    UserId = task.UserId
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                validationResponse.IsValid = false;
                validationResponse.Message = ex.Message;
                return StatusCode(500, validationResponse);
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult UpdateTask([FromRoute] long id, [FromBody] CreateUpdateTaskRequest request)
        {
            ValidationResponse validationResponse = _tasksValidators.CreateUpdateTasksValidator(request); // validation for the tasks input

            if (!validationResponse.IsValid)
            {
                return BadRequest(validationResponse);
            }

            try
            {
                var task = _context.Tasks.Where(x=>x.Id == id).FirstOrDefault();

                if (task == null)
                {
                    return NotFound("Task not found");
                }
                else if (task.UserId != _authenticatedUser.Id && _authenticatedUser.UserRole != Enums.UserRole.Administrator)
                {
                    return Unauthorized("You are not authorized to update the task.");
                }

                task.Title = request.Title;
                task.Description = request.Description;
                task.Status = request.Status;
                _context.SaveChanges();

                GetTaskResponse response = new()
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    Status = task.Status,
                    UserId = task.UserId
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteTask([FromRoute] long id)
        {
            try
            {
                var task = _context.Tasks.FirstOrDefault(x => x.Id == id);

                if (task == null)
                {
                    return NotFound("Task not found");
                }
                else if (task.UserId != _authenticatedUser.Id && _authenticatedUser.UserRole != Enums.UserRole.Administrator)
                {
                    return Unauthorized("You are not authorized to delete the task.");
                }

                _context.Tasks.Remove(task);
                _context.SaveChanges();

                return Ok("Task deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }


    }
}
