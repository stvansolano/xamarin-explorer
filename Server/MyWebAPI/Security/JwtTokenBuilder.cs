using System;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;

namespace MyWebAPI.Security
{
    public sealed class JwtTokenBuilder
    {
        private SecurityKey _securityKey;
        private string _subject;
        private string _issuer;
        private string _audience;
		private int _expireMinutes;

		private static Dictionary<string, string> _claims = new Dictionary<string, string>();

        public const string CONFIGURATION_AUTHENTICATION_AUDIENCE_KEY = "AUTHENTICATION_AUDIENCE";
        public const string CONFIGURATION_AUTHENTICATION_ISSUER_KEY = "AUTHENTICATION_ISSUER";
        public const string CONFIGURATION_AUTHENTICATION_SHARED_SECRET_KEY = "AUTHENTICATION_SHARED_SECRET";
        public const string CONFIGURATION_AUTHENTICATION_SUBJECT_KEY = "AUTHENTICATION_SUBJECT";

		public const int ONE_MONTH = 24 * 60 * 30;

		public const string BEARER_TOKEN_SCHEME = JwtBearerDefaults.AuthenticationScheme;

        public JwtTokenBuilder AddSecurityKey(SecurityKey securityKey)
        {
            _securityKey = securityKey;
            return this;
        }

        public static IEnumerable<Claim> GetClaims(string name)
        {
            var basicClaims = new Dictionary<string, string>();
            basicClaims.Add(ClaimTypes.Name, name);

            return _claims.Union(basicClaims)
                        .Select(item => new Claim(item.Key, item.Value))
                        .ToArray();
        }

        public JwtTokenBuilder AddSubject(string subject)
        {
            _subject = subject;
            return this;
        }

        internal static JwtSecurityToken GetSecuredToken(IConfiguration configuration)
        {
            var token = new JwtTokenBuilder()
                .AddSecurityKey(JwtSecurityKey.Create(configuration[CONFIGURATION_AUTHENTICATION_SHARED_SECRET_KEY]))
                                .AddSubject(configuration[CONFIGURATION_AUTHENTICATION_SUBJECT_KEY])
                                .AddIssuer(configuration[CONFIGURATION_AUTHENTICATION_ISSUER_KEY])
                                .AddAudience(configuration[CONFIGURATION_AUTHENTICATION_AUDIENCE_KEY])
                                .AddExpiry(ONE_MONTH)
                                .Build();

            return token;
        }

        public JwtTokenBuilder AddIssuer(string issuer)
        {
            _issuer = issuer;
            return this;
        }

        public JwtTokenBuilder AddAudience(string audience)
        {
            _audience = audience;
            return this;
        }

        public JwtTokenBuilder AddClaim(string type, string value)
        {
            _claims.Add(type, value);
            return this;
        }

        public JwtTokenBuilder AddClaims(Dictionary<string, string> claims)
        {
            _claims.Union(claims);
            return this;
        }

        public JwtTokenBuilder AddExpiry(int expiryInMinutes)
        {
            _expireMinutes = expiryInMinutes;
            return this;
        }

        public JwtSecurityToken Build()
        {
            EnsureArguments();

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, this._subject),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }
                .Union(_claims.Select(item => new Claim(item.Key, item.Value)));

            var token = new JwtSecurityToken(
                            issuer: _issuer,
                            audience: _audience,
                            claims: claims,
                            expires: DateTime.UtcNow.AddMinutes(_expireMinutes),
                            signingCredentials: new SigningCredentials(
                                                        _securityKey,
                                                        SecurityAlgorithms.HmacSha256));
            return token;
        }

        public static bool ValidateToken(string token, string authorizationKey)
        {
            SecurityToken validatedToken;
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            if (handler.CanReadToken(token))
            {
                return false;
            }
            var options = new TokenValidationParameters
            {
                IssuerSigningKey = JwtSecurityKey.Create(authorizationKey),
                ValidateIssuerSigningKey = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuer = false
            };

            try
            {
                var user = handler.ValidateToken(token.Replace($"{BEARER_TOKEN_SCHEME} ", string.Empty), options, out validatedToken);

                return user != null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid Bearer token \"{0}\". Exception: {1}", token, ex);
                return false;
            }
        }

        #region Private Methods

        private void EnsureArguments()
        {
            if (this._securityKey == null)
                throw new ArgumentNullException($"{nameof(JwtTokenBuilder)}:Security Key");

            if (string.IsNullOrEmpty(this._subject))
                throw new ArgumentNullException($"{nameof(JwtTokenBuilder)}:Subject");

            if (string.IsNullOrEmpty(this._issuer))
                throw new ArgumentNullException($"{nameof(JwtTokenBuilder)}:Issuer");

            if (string.IsNullOrEmpty(this._audience))
                throw new ArgumentNullException($"{nameof(JwtTokenBuilder)}:Audience");
        }

        #endregion

        public static class JwtSecurityKey
        {
            public static SymmetricSecurityKey Create(string secret)
            {
                return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
            }
        }
    }
}
