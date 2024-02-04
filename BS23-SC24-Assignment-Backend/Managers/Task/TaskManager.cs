using BS23_SC24_Assignment_Backend.Context;
using BS23_SC24_Assignment_Backend.Managers.Security;
using BS23_SC24_Assignment_Backend.Models;
using BS23_SC24_Assignment_Backend.Requests;
using BS23_SC24_Assignment_Backend.Responses;
using BS23_SC24_Assignment_Backend.validators;

namespace BS23_SC24_Assignment_Backend.Managers.Task
{
    public class TaskManager : ITaskManager
    {
        private readonly AppDbContext _context;
        private readonly IAuthenticatedUser _authenticatedUser;
        private readonly TasksValidators _tasksValidators;

        public TaskManager(AppDbContext context, IAuthenticatedUser authenticatedUser, TasksValidators tasksValidators)
        {
            _context = context;
            _authenticatedUser = authenticatedUser;
            _tasksValidators = tasksValidators;
        }

        public BaseResponse DeleteTask(long id)
        {
            try
            {
                var task = _context.Tasks.FirstOrDefault(x => x.Id == id);

                if (task == null)
                {
                    return new BaseResponse
                    {
                        StatusCode = 404,
                        IsValid = false,
                        Message = "Task not found"
                    };
                }
                else if (task.UserId != _authenticatedUser.Id && _authenticatedUser.UserRole != Enums.UserRole.Administrator)
                {
                    return new BaseResponse
                    {
                        StatusCode = 401,
                        IsValid = false,
                        Message = "You are not authorized to delete the task"
                    };
                }

                _context.Tasks.Remove(task);
                _context.SaveChanges();

                return new BaseResponse
                {
                    StatusCode = 200,
                    IsValid = true,
                    Message = "Task deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    StatusCode = 500,
                    IsValid = false,
                    Message = $"Internal Server Error: {ex.Message}"
                };
            }
        }

        public GetTaskListResponse GetAllTasks()
        {
            try
            {
                List<GetTaskResponse> taskList =
                [
                    .. _context.Tasks
                    .Select(task => new GetTaskResponse
                    {
                        Id = task.Id,
                        Title = task.Title,
                        Description = task.Description,
                        Status = task.Status,
                        UserId = task.UserId
                    })
                ];

                return new GetTaskListResponse
                {
                    StatusCode = 200,
                    IsValid = true,
                    Message = "",
                    TaskList = taskList
                };
            }
            catch(Exception ex)
            {
                return new GetTaskListResponse
                {
                    StatusCode = 500,
                    IsValid = false,
                    Message = $"Internal Server Error: {ex.Message}"
                };
            }
        }

        public GetTaskListResponse GetMyTasks()
        {
            try
            {
                List<GetTaskResponse> taskList =
                [
                    .. _context.Tasks
                    .Where(x => x.UserId == _authenticatedUser.Id)
                    .Select(task => new GetTaskResponse
                    {
                        Id = task.Id,
                        Title = task.Title,
                        Description = task.Description,
                        Status = task.Status,
                        UserId = task.UserId
                    })
                ];

                return new GetTaskListResponse
                {
                    StatusCode = 200,
                    IsValid = true,
                    Message = "",
                    TaskList = taskList
                };
            }
            catch (Exception ex)
            {
                return new GetTaskListResponse
                {
                    StatusCode = 500,
                    IsValid = false,
                    Message = $"Internal Server Error: {ex.Message}"
                };
            }
        }

        public GetTaskResponse GetTaskById(long id)
        {
            try
            {
                GetTaskResponse response = new GetTaskResponse
                {
                    StatusCode = 401,
                    IsValid = false,
                    Message = "You don't have the permission to view the task!"
                };

                var task = _context.Tasks
                    .Where(x => x.Id == id)
                    .FirstOrDefault();

                if (task != null && (task.Id == _authenticatedUser.Id || _authenticatedUser.UserRole == Enums.UserRole.Administrator))
                {
                    response = new GetTaskResponse
                    {
                        StatusCode = 200,
                        IsValid = true,
                        Message = "Task successfully fetched.",
                        Id = task.Id,
                        Title = task.Title,
                        Description = task.Description,
                        Status = task.Status,
                        UserId = task.UserId
                    };
                }
                return response;
            }
            catch (Exception ex)
            {
                GetTaskResponse response = new GetTaskResponse
                {
                    StatusCode = 500,
                    IsValid = false,
                    Message = ex.Message
                };
                return response;
            }
        }

        public GetTaskResponse PostTask(CreateUpdateTaskRequest request)
        {
            GetTaskResponse validationResponse = _tasksValidators.CreateUpdateTasksValidator(request); // validation for the tasks input

            if (!validationResponse.IsValid)
            {
                return validationResponse;
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
                    StatusCode = 200,
                    IsValid = true,
                    Message = "Task created successfully",
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    Status = task.Status,
                    UserId = task.UserId
                };

                return response;
            }
            catch (Exception ex)
            {
                return new GetTaskResponse
                {
                    StatusCode = 500,
                    IsValid = false,
                    Message = ex.Message
                };
            }
        }

        public GetTaskResponse UpdateTask(long id, CreateUpdateTaskRequest request)
        {
            GetTaskResponse validationResponse = _tasksValidators.CreateUpdateTasksValidator(request);

            if (!validationResponse.IsValid)
            {
                return validationResponse;
            }

            try
            {
                var task = _context.Tasks.Where(x => x.Id == id).FirstOrDefault();

                if (task == null)
                {
                    return new GetTaskResponse
                    {
                        StatusCode = 404,
                        IsValid = false,
                        Message = "Task not found"
                    };
                }
                else if (task.UserId != _authenticatedUser.Id && _authenticatedUser.UserRole != Enums.UserRole.Administrator)
                {
                    return new GetTaskResponse
                    {
                        StatusCode = 401,
                        IsValid = false,
                        Message = "You are not authorized to update the task"
                    };
                }

                task.Title = request.Title;
                task.Description = request.Description;
                task.Status = request.Status;
                _context.SaveChanges();

                GetTaskResponse response = new()
                {
                    StatusCode = 200,
                    IsValid = true,
                    Message = "Task updated successfully",
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    Status = task.Status,
                    UserId = task.UserId
                };

                return response;
            }
            catch (Exception ex)
            {
                return new GetTaskResponse
                {
                    StatusCode = 500,
                    IsValid = false,
                    Message = $"Internal Server Error: {ex.Message}"
                };
            }
        }
    }
}
