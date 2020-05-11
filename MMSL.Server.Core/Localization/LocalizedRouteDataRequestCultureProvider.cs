using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

namespace MMSL.Server.Core.Localization {
    public class LocalizedRouteDataRequestCultureProvider : IRequestCultureProvider {

        public readonly CultureInfo _defaultCulture;

        public LocalizedRouteDataRequestCultureProvider(RequestCulture requestCulture) {
            _defaultCulture = requestCulture.Culture;
        }

        public Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext) {
            PathString url = httpContext.Request.Path;

            if (url.ToString().Length <= 1) {
                return Task.FromResult(new ProviderCultureResult(_defaultCulture.TwoLetterISOLanguageName));
            }

            string[] parts = url.Value
                .Trim(Path.AltDirectorySeparatorChar)
                .Split(Path.AltDirectorySeparatorChar);

            if (parts.Length < 3) {
                return Task.FromResult(new ProviderCultureResult(_defaultCulture.TwoLetterISOLanguageName));
            }

            string culture = parts[3];

            return Task.FromResult(
                !Regex.IsMatch(culture, @"^[a-z]{2}(-A-Z{2})*$")
                    ? new ProviderCultureResult(_defaultCulture.TwoLetterISOLanguageName)
                    : new ProviderCultureResult(culture)
                );
        }
    }
}
