

using Harvested.AI.Common.ResponseBuilder.Contracts;

namespace MMSL.Common.ResponseBuilder.Contracts
{
    public interface IResponseFactory
    {
        IWebResponse GetSuccessReponse();

        IWebResponse GetErrorResponse();
    }
}
