using BS23_SC24_Assignment_Backend.Context;
using BS23_SC24_Assignment_Backend.Managers.Security;
using BS23_SC24_Assignment_Backend.Models;
using BS23_SC24_Assignment_Backend.Requests;
using BS23_SC24_Assignment_Backend.Responses;
using BS23_SC24_Assignment_Backend.validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BS23_SC24_Assignment_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IAuthenticatedUser _authenticatedUser;
        private readonly TasksValidators _tasksValidators;

        public TasksController(AppDbContext context, IAuthenticatedUser authenticatedUser, TasksValidators tasksValidators)
        {
            _context = context;
            _authenticatedUser = authenticatedUser;
            _tasksValidators = tasksValidators;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetMyTasks()
        {
            try
            {
                List <GetTaskResponse> response = _context.Tasks
                                        .Where(x => x.UserId == _authenticatedUser.Id)
                                        .Select(task => new GetTaskResponse
                                        {
                                            Id = task.Id,
                                            Title = task.Title,
                                            Description = task.Description,
                                            Status = task.Status,
                                            UserId = task.UserId
                                        })
                                        .ToList();
               
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
                Tasks task = new Tasks
                {
                    Title = request.Title,
                    Description = request.Description,
                    Status = request.Status,
                    UserId = _authenticatedUser.Id
                };
                _context.Tasks.Add(task);
                _context.SaveChanges();
                return Ok("Task added successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
