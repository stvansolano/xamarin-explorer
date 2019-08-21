using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace MyWebAPI.Security
{
   public class AuthorizedResources
    {
        public const string AUTHENTICATION_SCHEMES = JwtBearerDefaults.AuthenticationScheme;
        public const string AUTHENTICATION_POLICY = "CustomBearerToken";
    }
}