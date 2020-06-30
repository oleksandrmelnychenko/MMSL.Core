using System.Globalization;

namespace MMSL.Common.Helpers {
    public static class NumericParsingHelper {
        public static bool TryParsePrice(string price, out decimal priceValue) {
            return decimal.TryParse(price.Replace(',', '.'), NumberStyles.Currency, CultureInfo.InvariantCulture, out priceValue) 
                && priceValue != default(decimal);
        }

        public static bool TryParseFloat(string price, out float priceValue) {
            return float.TryParse(price.Replace(',', '.'), NumberStyles.Currency, CultureInfo.InvariantCulture, out priceValue)
                && priceValue != default(float);
        }
    }
}
