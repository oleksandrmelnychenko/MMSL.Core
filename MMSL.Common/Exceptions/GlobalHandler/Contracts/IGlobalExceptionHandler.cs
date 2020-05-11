using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace MMSL.Common.Exceptions.GlobalHandler.Contracts
{
    public interface IGlobalExceptionHandler
    {
        Task HandleException(HttpContext httpContext, IExceptionHandlerFeature exceptionHandlerFeature, bool isDevelopmentMode);
    }
}
