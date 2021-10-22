using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Api.Auth.ApiKey
{
    public class ApiKeyRequirement : IAuthorizationRequirement
    {
        public string ApiKeys { get; set; }

        public ApiKeyRequirement(string apiKeys)
        {
            ApiKeys = apiKeys;
        }
    }
}