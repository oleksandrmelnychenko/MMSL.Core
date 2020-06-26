using System.Globalization;

namespace MMSL.Common.Helpers {
    public static class PriceParsingHelper {
        public static bool TryParsePrice(string price, out decimal priceValue) {
            return decimal.TryParse(price.Replace(',', '.'), NumberStyles.Currency, CultureInfo.InvariantCulture, out priceValue) 
                && priceValue != default(decimal);
        }

    }
}
