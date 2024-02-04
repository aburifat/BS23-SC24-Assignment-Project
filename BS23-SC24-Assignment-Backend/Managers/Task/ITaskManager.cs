using BS23_SC24_Assignment_Backend.Requests;
using BS23_SC24_Assignment_Backend.Responses;

namespace BS23_SC24_Assignment_Backend.Managers.Task
{
    public interface ITaskManager
    {
        GetTaskListResponse GetMyTasks();
        GetTaskResponse GetTaskById(long id);
        GetTaskListResponse GetAllTasks();
        GetTaskResponse PostTask(CreateUpdateTaskRequest request);
        GetTaskResponse UpdateTask(long id, CreateUpdateTaskRequest request);
        BaseResponse DeleteTask(long id);
    }
}
