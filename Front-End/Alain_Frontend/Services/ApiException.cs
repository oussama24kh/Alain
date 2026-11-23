using System.Net;

namespace project_03.Services
{
    internal class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public ApiException(HttpStatusCode statusCode, string Message) : base(Message)
        {
            StatusCode = statusCode;
        }
    }
}