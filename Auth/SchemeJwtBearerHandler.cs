using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;

namespace Api.Auth
{
    public class SchemeJwtBearerHandler : AuthenticationHandler<JwtBearerOptions>
    {
        private OpenIdConnectConfiguration _configuration;

        public SchemeJwtBearerHandler(IOptionsMonitor<JwtBearerOptions> options, ILoggerFactory logger, UrlEncoder encoder, IDataProtectionProvider dataProtection, ISystemClock clock)
            : base(options, logger, encoder, clock)
        { }

        /// <summary>
        /// The handler calls methods on the events which give the application control at certain points where processing is occurring. 
        /// If it is not provided a default instance is supplied which does nothing when the methods are called.
        /// </summary>
        protected new JwtBearerEvents Events
        {
            get { return (JwtBearerEvents)base.Events; }
            set { base.Events = value; }
        }

        protected override Task<object> CreateEventsAsync() => Task.FromResult<object>(new JwtBearerEvents());

        /// <summary>
        /// Searches the 'Authorization' header for a 'Bearer' token. If the 'Bearer' token is found, it is validated using <see cref="TokenValidationParameters"/> set in the options.
        /// </summary>
        /// <returns></returns>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string token = null;
            try
            {
                // Give application opportunity to find from a different location, adjust, or reject token
                var messageReceivedContext = new MessageReceivedContext(Context, Scheme, Options);

                // event can set the token
                await Events.MessageReceived(messageReceivedContext);
                if (messageReceivedContext.Result != null)
                {
                    return messageReceivedContext.Result;
                }

                // If application retrieved token from somewhere else, use that.
                token = messageReceivedContext.Token;

                if (string.IsNullOrEmpty(token))
                {
                    string authorization = Request.Headers["Authorization"];

                    // If no authorization header found, nothing to process further
                    if (string.IsNullOrEmpty(authorization))
                    {
                        return AuthenticateResult.NoResult();
                    }

                    if (authorization.StartsWith($"{Scheme.Name} ", StringComparison.OrdinalIgnoreCase))
                    {
                        token = authorization.Substring($"{Scheme.Name} ".Length).Trim();
                    }

                    // If no token found, no further work possible
                    if (string.IsNullOrEmpty(token))
                    {
                        return AuthenticateResult.NoResult();
                    }
                }

                if (_configuration == null && Options.ConfigurationManager != null)
                {
                    _configuration = await Options.ConfigurationManager.GetConfigurationAsync(Context.RequestAborted);
                }

                var validationParameters = Options.TokenValidationParameters.Clone();
                if (_configuration != null)
                {
                    var issuers = new[] { _configuration.Issuer };
                    validationParameters.ValidIssuers = validationParameters.ValidIssuers?.Concat(issuers) ?? issuers;

                    validationParameters.IssuerSigningKeys = validationParameters.IssuerSigningKeys?.Concat(_configuration.SigningKeys)
                        ?? _configuration.SigningKeys;
                }

                List<Exception> validationFailures = null;
                SecurityToken validatedToken;
                foreach (var validator in Options.SecurityTokenValidators)
                {
                    if (validator.CanReadToken(token))
                    {
                        ClaimsPrincipal principal;
                        try
                        {
                            principal = validator.ValidateToken(token, validationParameters, out validatedToken);
                        }
                        catch (Exception ex)
                        {
                            Logger.LogError(token, ex);

                            // Refresh the configuration for exceptions that may be caused by key rollovers. The user can also request a refresh in the event.
                            if (Options.RefreshOnIssuerKeyNotFound && Options.ConfigurationManager != null
                                && ex is SecurityTokenSignatureKeyNotFoundException)
                            {
                                Options.ConfigurationManager.RequestRefresh();
                            }

                            if (validationFailures == null)
                            {
                                validationFailures = new List<Exception>(1);
                            }
                            validationFailures.Add(ex);
                            continue;
                        }

                        Logger.LogInformation("Token validated");

                        var tokenValidatedContext = new TokenValidatedContext(Context, Scheme, Options)
                        {
                            Principal = principal,
                            SecurityToken = validatedToken
                        };

                        await Events.TokenValidated(tokenValidatedContext);
                        if (tokenValidatedContext.Result != null)
                        {
                            return tokenValidatedContext.Result;
                        }

                        if (Options.SaveToken)
                        {
                            tokenValidatedContext.Properties.StoreTokens(new[]
                            {
                                new AuthenticationToken { Name = "access_token", Value = token }
                            });
                        }

                        tokenValidatedContext.Success();
                        return tokenValidatedContext.Result;
                    }
                }

                if (validationFailures != null)
                {
                    var authenticationFailedContext = new AuthenticationFailedContext(Context, Scheme, Options)
                    {
                        Exception = (validationFailures.Count == 1) ? validationFailures[0] : new AggregateException(validationFailures)
                    };

                    await Events.AuthenticationFailed(authenticationFailedContext);
                    if (authenticationFailedContext.Result != null)
                    {
                        return authenticationFailedContext.Result;
                    }

                    return AuthenticateResult.Fail(authenticationFailedContext.Exception);
                }

                return AuthenticateResult.Fail("No SecurityTokenValidator available for token: " + token ?? "[null]");
            }
            catch (Exception ex)
            {
                Logger.LogError("(HandleAuthenticateAsync) Error processing message", ex);

                var authenticationFailedContext = new AuthenticationFailedContext(Context, Scheme, Options)
                {
                    Exception = ex
                };

                await Events.AuthenticationFailed(authenticationFailedContext);
                if (authenticationFailedContext.Result != null)
                {
                    return authenticationFailedContext.Result;
                }

                throw;
            }
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            var authResult = await HandleAuthenticateOnceSafeAsync();
            var eventContext = new JwtBearerChallengeContext(Context, Scheme, Options, properties)
            {
                AuthenticateFailure = authResult?.Failure
            };

            // Avoid returning error=invalid_token if the error is not caused by an authentication failure (e.g missing token).
            if (Options.IncludeErrorDetails && eventContext.AuthenticateFailure != null)
            {
                eventContext.Error = "invalid_token";
                eventContext.ErrorDescription = CreateErrorDescription(eventContext.AuthenticateFailure);
            }

            await Events.Challenge(eventContext);
            if (eventContext.Handled)
            {
                return;
            }

            Response.StatusCode = 401;

            if (string.IsNullOrEmpty(eventContext.Error) &&
                string.IsNullOrEmpty(eventContext.ErrorDescription) &&
                string.IsNullOrEmpty(eventContext.ErrorUri))
            {
                Response.Headers.Append(HeaderNames.WWWAuthenticate, Options.Challenge);
            }
            else
            {
                // https://tools.ietf.org/html/rfc6750#section-3.1
                // WWW-Authenticate: Bearer realm="example", error="invalid_token", error_description="The access token expired"
                var builder = new StringBuilder(Options.Challenge);
                if (Options.Challenge.IndexOf(" ", StringComparison.Ordinal) > 0)
                {
                    // Only add a comma after the first param, if any
                    builder.Append(',');
                }
                if (!string.IsNullOrEmpty(eventContext.Error))
                {
                    builder.Append(" error=\"");
                    builder.Append(eventContext.Error);
                    builder.Append("\"");
                }
                if (!string.IsNullOrEmpty(eventContext.ErrorDescription))
                {
                    if (!string.IsNullOrEmpty(eventContext.Error))
                    {
                        builder.Append(",");
                    }

                    builder.Append(" error_description=\"");
                    builder.Append(eventContext.ErrorDescription);
                    builder.Append('\"');
                }
                if (!string.IsNullOrEmpty(eventContext.ErrorUri))
                {
                    if (!string.IsNullOrEmpty(eventContext.Error) ||
                        !string.IsNullOrEmpty(eventContext.ErrorDescription))
                    {
                        builder.Append(",");
                    }

                    builder.Append(" error_uri=\"");
                    builder.Append(eventContext.ErrorUri);
                    builder.Append('\"');
                }

                Response.Headers.Append(HeaderNames.WWWAuthenticate, builder.ToString());
            }
        }

        private static string CreateErrorDescription(Exception authFailure)
        {
            IEnumerable<Exception> exceptions;
            if (authFailure is AggregateException)
            {
                var agEx = authFailure as AggregateException;
                exceptions = agEx.InnerExceptions;
            }
            else
            {
                exceptions = new[] { authFailure };
            }

            var messages = new List<string>();

            foreach (var ex in exceptions)
            {
                // Order sensitive, some of these exceptions derive from others
                // and we want to display the most specific message possible.
                if (ex is SecurityTokenInvalidAudienceException)
                {
                    messages.Add("The audience is invalid");
                }
                else if (ex is SecurityTokenInvalidIssuerException)
                {
                    messages.Add("The issuer is invalid");
                }
                else if (ex is SecurityTokenNoExpirationException)
                {
                    messages.Add("The token has no expiration");
                }
                else if (ex is SecurityTokenInvalidLifetimeException)
                {
                    messages.Add("The token lifetime is invalid");
                }
                else if (ex is SecurityTokenNotYetValidException)
                {
                    messages.Add("The token is not valid yet");
                }
                else if (ex is SecurityTokenExpiredException)
                {
                    messages.Add("The token is expired");
                }
                else if (ex is SecurityTokenSignatureKeyNotFoundException)
                {
                    messages.Add("The signature key was not found");
                }
                else if (ex is SecurityTokenInvalidSignatureException)
                {
                    messages.Add("The signature is invalid");
                }
            }

            return string.Join("; ", messages);
        }
    }
}
