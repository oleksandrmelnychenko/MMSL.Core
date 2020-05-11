using System.Net;
using Harvested.AI.Common.ResponseBuilder.Contracts;

namespace Harvested.AI.Common.ResponseBuilder
{
    public class ErrorResponse : IWebResponse
    {
        public object Body { get; set; }

        public string Message { get; set; }

        public HttpStatusCode StatusCode { get; set; }
    }
}
