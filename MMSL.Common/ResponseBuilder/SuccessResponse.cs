using System.Net;
using Harvested.AI.Common.ResponseBuilder.Contracts;

namespace MMSL.Common.ResponseBuilder
{
    public class SuccessResponse : IWebResponse
    {
        public object Body { get; set; }

        public string Message { get; set; }

        public HttpStatusCode StatusCode { get; set; }
    }
}
