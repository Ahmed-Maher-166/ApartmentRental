using SmartRental.Core.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartRental.Core.Models.Identity;

namespace SmartRental.Core.Services
{
    public interface ITokenService
    {
       public Task<string> CreateToken(AppUser appUser , UserManager<AppUser> userManager);
    }
}
