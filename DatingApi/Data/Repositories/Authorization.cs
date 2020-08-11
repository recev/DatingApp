using System;
using System.Linq;
using Data;
using DatingApi.Data.Models;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace DatingApi.Data.Repositories
{
    public class Authorization : IAuthorization
    {
        IConfiguration _configuration;
        public Authorization(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            var token = "";
            try
            {
                var symmetricSecurityKey = GetSymmetricSecurityKey();
                var signingCredentials = GetSigningCredentials(symmetricSecurityKey);
                var claimsIdenty = GetClaimsIdentity(user);
                var tokenDescriptor = GetSecurityTokenDescriptor(signingCredentials, claimsIdenty);

                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                token = tokenHandler.WriteToken(securityToken);
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex.Message);
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

        private ClaimsIdentity GetClaimsIdentity(User user)
        {
            return new ClaimsIdentity(
                            new Claim[]{
                                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                                new Claim(ClaimTypes.Name, user.Username)
                        });
        }

        private SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            var secretKey = _configuration.GetSection("AppSettings:AuthenticationSecretKey").Value;
            var secretKeyInBytes = System.Text.Encoding.UTF8.GetBytes(secretKey);

            var symmetricSecurityKey = new SymmetricSecurityKey(secretKeyInBytes);
            return symmetricSecurityKey;
        }

        public bool AreCredentialsValid(User user, string password)
        {
            var result = false;
            try
            {
                using (var hmac = new HMACSHA512(user.PasswordSaltKey))
                {
                    var passwordInBytes = System.Text.Encoding.UTF8.GetBytes(password);

                    var passwordHash = hmac.ComputeHash(passwordInBytes);

                    if (passwordHash.SequenceEqual(user.PasswordHash))
                        result = true;
                }   
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }

            return result;
        }

        public User CreateUser(string username, string Password)
        {
            User user = null;
            try
            {
                using (var hcmac = new HMACSHA512())
                {
                    var passwordInBytes = System.Text.Encoding.UTF8.GetBytes(Password);
                    var passwordHash = hcmac.ComputeHash(passwordInBytes);
                    
                    user = new User {
                        Username = username,
                        PasswordSaltKey = hcmac.Key,
                        PasswordHash = passwordHash
                    };
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }

            return user;
        }
    }
}