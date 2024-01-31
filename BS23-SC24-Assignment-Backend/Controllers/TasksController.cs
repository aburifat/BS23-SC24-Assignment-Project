using BS23_SC24_Assignment_Backend.Context;
using BS23_SC24_Assignment_Backend.Managers.Security;
using BS23_SC24_Assignment_Backend.Models;
using BS23_SC24_Assignment_Backend.Requests;
using BS23_SC24_Assignment_Backend.Responses;
using BS23_SC24_Assignment_Backend.validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
                List<Tasks> myTasks = _context.Tasks.Where(x => x.UserId == _authenticatedUser.Id).ToList();
                return Ok(myTasks);
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
                User currUser = _context.Users.FirstOrDefault(x => x.Id == _authenticatedUser.Id);
                Tasks task = new Tasks
                {
                    Title = request.Title,
                    Description = request.Description,
                    Status = request.Status,
                    UserId = _authenticatedUser.Id,
                    User = currUser
                };
                _context.Tasks.Add(task);
                _context.SaveChanges();
                return Ok(task);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
