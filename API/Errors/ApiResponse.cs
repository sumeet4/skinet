namespace API.Errors
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string message = null )
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessgeForStatusCode(statusCode);
        }

     

        public int StatusCode { get; set; }

        public string Message {get; set;}


        private string GetDefaultMessgeForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A bad request you have made",
                401 => "Authorize you are not",
                404 => "Resource found it was not",
                500 => "Errors are the path to the dark side",
                _ => null
            };
        }
    }
}