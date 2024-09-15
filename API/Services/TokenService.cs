using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;

namespace API.Services
{
    public class TokenService(IConfiguration config) : ITokenService
    {
        public string CreateToken(AppUser user)
        {
            
            var tokenKey = config["TokenKey"] ?? throw new Exception("Can't access tokenKey from appsetting");
            if(tokenKey.Length < 64)  throw new Exception("Token key must be longer than 64");

            var key = 


        }
    }
}