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

            List<string> details = new List<string>();

            if (request.Title == "" || request.Description == "" || request.Status == "") // Empty Input Field Case 
            {
                isValid = false;
                details.Add("Input fields can't be empty.");
            }

            if (!isValid) message = "One or more validation errors occurred.";

            ValidationResponse validationResponse = new ValidationResponse
            {
                IsValid = isValid,
                Message = message,
                Details = details
            };
            return validationResponse;
        }
    }
}
