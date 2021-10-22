using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Api.Auth
{
    public class AuthenticationSchemes
    {
        public const string SSOScheme = "CustomScheme";
        public const string APIScheme = JwtBearerDefaults.AuthenticationScheme;
    }
}
