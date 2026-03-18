namespace Application.Wrappers
{
    public class ApiResponse<T>
    {
        public ApiResponse()
        {

        }

        //success response
        public ApiResponse(T data, string message = null)
        {
            Succeed = true;
            Message = message;
            Data = data;
        }

        //failed response
        public ApiResponse(string message)
        {
            Succeed = false;
            Message = message;
        }

        public bool Succeed { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public T Data { get; set; }
    }
}
