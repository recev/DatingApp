using System;
using System.Linq;
using DatingApi.Data.Models;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using DatingApi.Settings;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using DatingApi.Data.DataTransferObjects;
using System.Threading.Tasks;
using DatingApi.Data.OperationResults;
using System.Collections.Generic;

namespace DatingApi.Data.Repositories
{
    public class Authorization : IAuthorization
    {
        AuthenticationSettings _authenticationSettings;
        ILogger<Authorization> _logger;
        IMapper _mapper;

        public Authorization(IOptions<AuthenticationSettings> authenticationSettings, ILogger<Authorization> logger, IMapper mapper)
        {
            this._authenticationSettings = authenticationSettings.Value;
            this._logger = logger;
            this._mapper = mapper;
        }

        public string GenerateToken(User user, IList<string> userRoles)
        {
            var token = "";
            try
            {
                var symmetricSecurityKey = GetSymmetricSecurityKey();
                var signingCredentials = GetSigningCredentials(symmetricSecurityKey);
                var claimsIdenty = GetClaimsIdentity(user, userRoles);
                var tokenDescriptor = GetSecurityTokenDescriptor(signingCredentials, claimsIdenty);

                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                token = tokenHandler.WriteToken(securityToken);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return token;
        }

        private SigningCredentials GetSigningCredentials(SymmetricSecurityKey symmetricSecurityKey)
        {
            return new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha512Signature);
        }

        private SecurityTokenDescriptor GetSecurityTokenDescriptor(SigningCredentials signingCredentials, ClaimsIdentity claimsIdenty)
        {
            return new SecurityTokenDescriptor
            {
                Subject = claimsIdenty,
                Expires = DateTime.Now.Add(new TimeSpan(24, 0, 0)),
                SigningCredentials = signingCredentials
            };
        }

        private ClaimsIdentity GetClaimsIdentity(User user, IList<string> userRoles)
        {
            var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName)
                };

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return new ClaimsIdentity(claims);
        }

        private SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            var secretKeyInBytes = System.Text.Encoding.UTF8.GetBytes(_authenticationSettings.SecretKey);

            var symmetricSecurityKey = new SymmetricSecurityKey(secretKeyInBytes);
            return symmetricSecurityKey;
        }

    }
}