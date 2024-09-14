using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController(DataContext context) : BaseApiController
    {

        [HttpPost("register")]       
        public async Task<ActionResult<AppUser>> Register(RegisterDto registerDto)
        {
            if(await UserExists(registerDto.Username))return BadRequest("Username already exists");

            using var hmack = new HMACSHA512();

            var user = new AppUser
            {
                UserName = registerDto.Username.ToLower(),
                PasswordHash = hmack.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmack.Key
                
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();
            return user;

        }
        private async Task<bool> UserExists(string username)
        {
            return await context.Users.AnyAsync(x=> x.UserName.ToLower() == username.ToLower());

        }
        
    }
}