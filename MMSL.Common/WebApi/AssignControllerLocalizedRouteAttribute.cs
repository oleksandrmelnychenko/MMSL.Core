using System;

namespace MMSL.Common.WebApi
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class AssignControllerLocalizedRouteAttribute : BaseAssignControllerRouteAttribute {
        public AssignControllerLocalizedRouteAttribute(string environment, int version, string template) :
            base(environment, version, $"{environment}/{BuildRouteVersion(version)}/{{culture}}/{template}") { }
    }
}
