using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AccountController(DataContext context) : BaseApiController
    {
        // public AccountController()
        // {
        // }

        [HttpPost("register")]
        
        public async Task<ActionResult<AppUser>> Register(string username, string password)
        {
            using var hmack = new HMACSHA512();

            var user = new AppUser
            {
                UserName = username,
                PasswordHash = hmack.ComputeHash(Encoding.UTF8.GetBytes(password)),
                PasswordSalt = hmack.Key
                
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();
            return user;

        }
    }
}