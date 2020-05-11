using Microsoft.AspNetCore.Mvc;

namespace MMSL.Common.WebApi {
    public abstract class BaseAssignControllerRouteAttribute : RouteAttribute {
        /// <summary>
        ///     Web Api version.
        /// </summary>
        public string Version { get; protected set; }

        /// <summary>
        ///     Dev or release environment.
        /// </summary>
        public string Environment { get; protected set; }

        public BaseAssignControllerRouteAttribute(string environment, int version, string routeTemplate)
            : base(routeTemplate) {

            Version = BuildRouteVersion(version);
            Environment = environment;
        }

        protected static string BuildRouteVersion(int number) {
            return $"v{number.ToString()}";
        }
    }
}
