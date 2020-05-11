using System;
using Microsoft.AspNetCore.Mvc;

namespace MMSL.Common.WebApi
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AssignActionRouteAttribute : RouteAttribute
    {

        public AssignActionRouteAttribute(string template) : base(template)
        {
        }
    }
}
