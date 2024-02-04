namespace BS23_SC24_Assignment_Backend.Responses
{
    public class GetTaskListResponse : BaseResponse
    {
        public IList<GetTaskResponse> TaskList { get; set; }
    }
}
