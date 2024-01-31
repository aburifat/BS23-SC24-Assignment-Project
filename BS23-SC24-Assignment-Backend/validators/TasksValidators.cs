using BS23_SC24_Assignment_Backend.Requests;
using BS23_SC24_Assignment_Backend.Responses;

namespace BS23_SC24_Assignment_Backend.validators
{
    public class TasksValidators
    {
        public ValidationResponse CreateUpdateTasksValidator(CreateUpdateTaskRequest request)
        {
            bool isValid = true;

            string message = "";

            if (string.IsNullOrWhiteSpace(request.Title) ||
                string.IsNullOrWhiteSpace(request.Description) ||
                string.IsNullOrWhiteSpace(request.Status)) // Empty Input Field Case 
            {
                isValid = false;
                message = "Input fields can't be empty.";
            }

            ValidationResponse validationResponse = new()
            {
                IsValid = isValid,
                Message = message,
            };
            return validationResponse;
        }
    }
}
