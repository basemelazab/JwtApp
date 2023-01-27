using JwtApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JwtApp.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public AuthService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<AuthModel> RegisterAsync(RegisterModel model)
        {
            if (!(await _userManager.FindByEmailAsync(model.Email) is null))
            {
                return new AuthModel { Message= "Email is already registered!" };
            }

            if (!(await _userManager.FindByNameAsync(model.UserName) is null))
            {
                return new AuthModel { Message = "UserName is already registered!" };
            }

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email
            };

            var result=await _userManager.CreateAsync(user,model.Password);
            if(!result.Succeeded)
            {
                var errors = string.Empty;
                foreach(var error in result.Errors)
                {
                    errors += $"{error.Description},";
                }
                return new AuthModel { Message = errors };
            }
             await _userManager.AddToRoleAsync(user,"User");
        }

        public async Task<AuthModel> CreateJwtToken(ApplicationUser user)
        {
            var userClaims= await _userManager.GetClaimsAsync(user);
            var roles= await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles",role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim("uId",user.Id),
            }
            .Union(userClaims)
            .Union(roleClaims);

           var SymmetricSecurityKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            new SigningCredentials(SymmetricSecurityKey,SecurityAlgorithms.Hb);

        }
    }
}
