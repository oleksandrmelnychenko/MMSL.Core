using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace MMSL.Common.IdentityConfiguration
{
    public class AuthOptions
    {
        public const string ISSUER = "MMSL";

        public const string AUDIENCE_LOCAL = "http://localhost:4200/";
        public const string AUDIENCE_REMOTE = "http://localhost:4200/";

        public static SymmetricSecurityKey GetSymmetricSecurityKey(string key)
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
        }
    }
}
