using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.Security.Claims;

using AttributeRouting.Web.Http;

using BadgeAPI._Security;
using BadgeAPI.Data;

using System.Security.Cryptography;
using System.Text;

namespace BadgeAPI.Controllers
{
    public class SigninController : ApiController
    {
        [Route("api/Signin")]
        [HttpPost]
        public LoginResult PostSignIn([FromBody] LoginCredential credentials)
        {

            var auth = new LoginResult() { Authenticated = false };

            var userRoles = QueryableDependencies.GetLoginUserRoles(credentials.UserName, credentials.Password);
            if (userRoles.Count > 0)
            //if (userRoles.Where(r => r == "CredentialSystem").Any())
            {
                auth.Authenticated = true;

                var allClaims = userRoles.Select(r => new Claim(ClaimTypes.Role, r.ToString())).ToList();
                allClaims.Add(new Claim(ClaimTypes.Name, credentials.UserName));
                allClaims.Add(new Claim(ClaimTypes.Role, userRoles[0].ToString()));

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(allClaims),

                    AppliesToAddress = ConfigurationManager.AppSettings["JwtAllowedAudience"],
                    TokenIssuerName = ConfigurationManager.AppSettings["JwtValidIssuer"],
                    SigningCredentials = new SigningCredentials(new InMemorySymmetricSecurityKey(JwtTokenValidationHandler.SymmetricKey), "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256", "http://www.w3.org/2001/04/xmlenc#sha256")
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                auth.Token = tokenString;
            }

            return auth;
        }

    }
}