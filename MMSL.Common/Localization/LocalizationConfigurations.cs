using System.Globalization;
using System.Linq;

namespace MMSL.Common.Localization {
    public static class LocalizationConfigurations {

        public const string ROUTE_TEMPLATE = "";

        public static CultureInfo[] SupportedCultures { get; } = new CultureInfo[] {
            //TODO: maybe locale must be in country code format
            new CultureInfo("en-US"),//us
            new CultureInfo("uk")//ua
        };

        public static CultureInfo GetCultureByTwoLetterName(string twoLetterName) {
            CultureInfo foundCulture = SupportedCultures.FirstOrDefault(x => x.Name.StartsWith(twoLetterName));
            //TODO: maybe need distionary
            return foundCulture;
        }

        public static CultureInfo DefaultCulture => SupportedCultures[0];
    }
}
