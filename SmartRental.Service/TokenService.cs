using SmartRental.Core.Models;
using SmartRental.Core.Models.Identity;
using SmartRental.Core.Repository;
using SmartRental.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentRental.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public TokenService(IConfiguration configuration,
            IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

       public async Task<string> CreateToken(AppUser appUser, UserManager<AppUser> userManager)
        {
            var Auth = new List<Claim>
            {
                new Claim(ClaimTypes.Name, appUser.UserName),
                new Claim(ClaimTypes.Email, appUser.Email)
            };
            var UserRoles = await userManager.GetRolesAsync(appUser);
            foreach (var Role in UserRoles)
            {
                Auth.Add(new Claim(ClaimTypes.Role, Role));
            }
            if (UserRoles.Contains("Owner"))
            {
                var owner = await _unitOfWork.Repository<Owner>()
                    .GetFirstOrDefaultAsync(o => o.AppUserId == appUser.Id);

                if (owner != null)
                {
                    Auth.Add(new Claim("OwnerId", owner.Id.ToString()));
                }
            }
            var AuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var Token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(double.Parse(_configuration["JWT:DurationInDays"])),
                claims: Auth,
                signingCredentials: new SigningCredentials(AuthKey, SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
    }
} 
