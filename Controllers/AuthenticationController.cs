using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace cityInfo.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        /// <summary>
        /// we won't use this outside of this class, so we can scope it to this namespace
        /// </summary>
        public class AuthenticationRequestBody
        {/// <summary>
        /// 
        /// </summary>
            public string? UserName { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string? Password { get; set; }
        }

        private record CityInfoUser(int UserId, string UserName, string FirstName, string LastName, string City);

        private readonly IConfiguration _configuration;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public AuthenticationController(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="authenticationRequestBody"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [HttpPost("authenticate")]
        public ActionResult<string> Authenticate(
          AuthenticationRequestBody authenticationRequestBody)
        {
            // Step 1: validate the username/password
            var user = ValidateUserCredentials(
                authenticationRequestBody.UserName,
                authenticationRequestBody.Password);
            if (user == null)
            {
                return Unauthorized();
            }
            string secretKey = _configuration["Authentication:SecretForKey"] ?? "";
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentNullException("Authentication:SecretForKey", "The secret key for authentication is not configured.");
            }

            // Step 2: create a token
            var securityKey = new SymmetricSecurityKey(
                Convert.FromBase64String(secretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // step 3 Claims
            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("sub", user.UserId.ToString()));
            claimsForToken.Add(new Claim("given_name", user.FirstName));
            claimsForToken.Add(new Claim("family_name", user.LastName));
            claimsForToken.Add(new Claim("city", user.City));

            // step 4 generate token
            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claimsForToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                signingCredentials);

            var tokenToReturn = new JwtSecurityTokenHandler()
               .WriteToken(jwtSecurityToken);

            return Ok(tokenToReturn);
        }
        private CityInfoUser ValidateUserCredentials(string? userName, string? password)
        {
            // we don't have a user DB or table.  If you have, check the passed-through
            // username/password against what's stored in the database.
            //
            // For demo purposes, we assume the credentials are valid

            // return a new CityInfoUser (values would normally come from your user DB/table)
            return new CityInfoUser(
                1,
                userName ?? "",
                "Kevin",
                "Dockx",
                "Antwerp");

        }
    }
}
