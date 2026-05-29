using ApartmentRental.Core.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApartmentRental.Core.Services
{
    public interface ITokenService
    {
       public Task<string> CreateToken(AppUser appUser , UserManager<AppUser> userManager);
    }
}
