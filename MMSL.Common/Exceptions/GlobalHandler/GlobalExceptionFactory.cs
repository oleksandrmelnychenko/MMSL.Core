using Microsoft.AspNetCore.Mvc;
using MMSL.Common.Exceptions.GlobalHandler.Contracts;

namespace MMSL.Common.Exceptions.GlobalHandler
{
    public class GlobalExceptionFactory : IGlobalExceptionFactory
    {
        private readonly IGlobalExceptionHandler _globalExceptionHandler;

        public GlobalExceptionFactory([FromServices] IGlobalExceptionHandler globalExceptionHandler)
        {
            _globalExceptionHandler = globalExceptionHandler;
        }

        public IGlobalExceptionHandler New()
        {
            return _globalExceptionHandler;
        }
    }
}
