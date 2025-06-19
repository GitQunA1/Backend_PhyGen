namespace PhyGen_SWD392.Helpers
{
    public class ApiResponse<T>
    {
        public string Message { get; set; }

        public T Body { get; set; }

        public int Status { get; set; }

        public ApiResponse(int status, string message, T body)
        {
            Status = status;
            Message = message;
            Body = body;
        }
    }
}
