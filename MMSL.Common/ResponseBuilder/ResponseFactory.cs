using Harvested.AI.Common.ResponseBuilder;
using Harvested.AI.Common.ResponseBuilder.Contracts;
using MMSL.Common.ResponseBuilder.Contracts;

namespace MMSL.Common.ResponseBuilder
{
    public class ResponseFactory : IResponseFactory
    {
        public IWebResponse GetSuccessReponse()
        {
            return new SuccessResponse();
        }

        public IWebResponse GetErrorResponse()
        {
            return new ErrorResponse();
        }
    }
}
