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
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController(DataContext context,ITokenService tokenService) : BaseApiController
    {

        [HttpPost("register")]       
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
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
            return new UserDto{
                Username = user.UserName,
                TokenKey = tokenService.CreateToken(user)
            };

        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await context.Users.FirstOrDefaultAsync(x=> x.UserName == loginDto.Username.ToLower());

            if(user == null) return Unauthorized("Invalid Username");

            using var hmack = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmack.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if(computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }
            return new UserDto{
                Username = user.UserName,
                TokenKey = tokenService.CreateToken(user)
            };

        }
        private async Task<bool> UserExists(string username)
        {
            return await context.Users.AnyAsync(x=> x.UserName.ToLower() == username.ToLower());

        }
        
    }
}