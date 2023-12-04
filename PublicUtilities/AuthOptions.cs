using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace PublicUtilities
{
    public class AuthOptions
    {
        public const string Issuer = "AuthServer";
        public const string Audience = "AuthClient";
        const string Key = "1FDh$3tkd!32hjHjdf";
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
    }
}
