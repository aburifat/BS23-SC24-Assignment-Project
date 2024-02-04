using BS23_SC24_Assignment_Backend.Requests;
using BS23_SC24_Assignment_Backend.Responses;

namespace BS23_SC24_Assignment_Backend.validators
{
    public class TasksValidators
    {
        public GetTaskResponse CreateUpdateTasksValidator(CreateUpdateTaskRequest request)
        {
            bool isValid = true;
            string message = "";

            if (string.IsNullOrWhiteSpace(request.Title) ||
                string.IsNullOrWhiteSpace(request.Description) ||
                string.IsNullOrWhiteSpace(request.Status))
            {
                isValid = false;
                message = "Input fields can't be empty.";
            }

            GetTaskResponse validationResponse = new()
            {
                StatusCode = (isValid) ? 200 : 400,
                IsValid = isValid,
                Message = message,
            };
            return validationResponse;
        }
    }
}
