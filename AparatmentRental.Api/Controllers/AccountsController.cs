using AparatmentRental.Api.DTOS;
using ApartmentRental.Core.Models;
using ApartmentRental.Core.Models.Identity;
using ApartmentRental.Core.Repository;
using ApartmentRental.Core.Services;
using ApartmentRental.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIS.DTOS;

namespace AparatmentRental.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ITokenService tokenService;
        private readonly IUnitOfWork unitOfWork;

        public AccountsController(UserManager<AppUser> _userManager,
            SignInManager<AppUser> _signInManager,
            ITokenService _tokenService, IUnitOfWork _unitOfWork)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            tokenService = _tokenService;
            unitOfWork = _unitOfWork;
        }
        [HttpGet("ExistsEmail")]
        public async Task<ActionResult<bool>> CheckEmail(string email)
         => await userManager.FindByEmailAsync(email) is not null;
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDTO registerDTO)
        {
            if (CheckEmail(registerDTO.Email).Result.Value)
                return BadRequest("Email is already in use");
           if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = new AppUser
            {
                UserName = registerDTO.Name,
                Email = registerDTO.Email,
                NationalID = registerDTO.NationalID,
                Photo = registerDTO.Photo,
                Phones = registerDTO.PhoneNumber
        .Where(p => !string.IsNullOrWhiteSpace(p))

        .Select(p => new Phones
        {
            PhoneNumber = p
        })
        .ToList()
            };
            var result = await userManager.CreateAsync(user, registerDTO.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);
            if (registerDTO.Role == "Owner")
            {
                await userManager.AddToRoleAsync(user, "Owner");
                await unitOfWork.Repository<Owner>().AddAsync(new Owner { AppUserId = user.Id });
                await unitOfWork.CompleteAsync();
            }
            else if (registerDTO.Role == "Tenant")
            {
                await userManager.AddToRoleAsync(user, "Tenant");
                await unitOfWork.Repository<Tenant>().AddAsync(new Tenant { AppUserId = user.Id , UniversityId = registerDTO.UniversityId.Value });
                await unitOfWork.CompleteAsync();
            }
            else
                await userManager.AddToRoleAsync(user, "Admin");

            var token = await tokenService.CreateToken(user, userManager);
            return Ok(new UserDto
            {
                Name = user.UserName,
                Email = user.Email,
                Token = token
            });
        }
        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDTO loginDTO)
        {
            var user = await userManager.FindByEmailAsync(loginDTO.Email);
            if (user == null) return Unauthorized("Invalid email or password");
            var result = await signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);
            if (!result.Succeeded) return Unauthorized("Invalid email or password");
      
            return Ok(new UserDto
            {
                Name = user.UserName,
                Email = user.Email,
                Token = await tokenService.CreateToken(user, userManager)
            });

        }
    }
}
