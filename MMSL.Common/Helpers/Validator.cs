using System.Text.RegularExpressions;

namespace MMSL.Common.Helpers
{
    public class Validator
    {
        public static bool IsEmailValid(string email) =>
            Regex.IsMatch(
                email,
                @"^(([^<>()\[\]\\.,;:\s@""]+(\.[^<>()\[\]\\.,;:\s@""]+)*)|("".+""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$"
            );
    }
}
