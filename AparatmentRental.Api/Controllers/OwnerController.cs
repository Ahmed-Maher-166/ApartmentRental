using AparatmentRental.Api.DTOS;
using ApartmentRental.Core.Models;
using ApartmentRental.Core.Models.Identity;
using ApartmentRental.Core.Repository;
using ApartmentRental.Core.Services;
using ApartmentRental.Reporisitory.Identity;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AparatmentRental.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ITokenService tokenService;
        private readonly IUnitOfWork unitOfWork;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly AppIdentityDbContext context;
        public OwnerController(UserManager<AppUser> _userManager,
            SignInManager<AppUser> _signInManager,
            ITokenService _tokenService, IUnitOfWork _unitOfWork)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            tokenService = _tokenService;
            unitOfWork = _unitOfWork;
        }
   
    
    }
}
