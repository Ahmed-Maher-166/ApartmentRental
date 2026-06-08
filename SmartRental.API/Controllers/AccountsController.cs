
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using SmartRental.API;
using SmartRental.API.DTOS;
using SmartRental.Core.Models;
using SmartRental.Core.Models.Identity;
using SmartRental.Core.Repository;
using SmartRental.Core.Services;
using SmartRental.Core.Specification;
using SmartRental.Reporisitory.Data;
using System.Data;
using System.Numerics;

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
        private readonly SmartRentalContext context;

        public AccountsController(
    UserManager<AppUser> _userManager,
    SignInManager<AppUser> _signInManager,
    ITokenService _tokenService,
    IUnitOfWork _unitOfWork,
 SmartRentalContext Context)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            tokenService = _tokenService;
            unitOfWork = _unitOfWork;
            context = Context;
        }


        [HttpGet("ExistsEmail")]
        public async Task<ActionResult<bool>> CheckEmail(string email)
         => await userManager.FindByEmailAsync(email) is not null;
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDTO registerDTO)
        {
            if (registerDTO.Role != "Owner" && registerDTO.Role != "Tenant")
                return BadRequest("Invalid role");
            if (registerDTO.Role == "Tenant" && registerDTO.UniversityId is null)
                return BadRequest("UniversityId is required for Tenant");
            var ExistCheckEmail = await userManager.FindByEmailAsync(registerDTO.Email) is not null;
            if (ExistCheckEmail)
                return BadRequest("Email is already in use");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = new AppUser
            {
                Name = registerDTO.Name,
                UserName = registerDTO.Email.Split('@')[0],
                Email = registerDTO.Email,
                NationalID = registerDTO.NationalID,
                Photo = registerDTO.Photo,
                Phones = registerDTO.PhoneNumber?
        .Where(p => !string.IsNullOrWhiteSpace(p))

        .Select(p => new Phones
        {
            PhoneNumber = p.Trim()
        })
        .ToList() ?? new List<Phones>()
            };
        await using var transaction =
        await context.Database.BeginTransactionAsync();
            try
            {
                var result2 = await userManager.CreateAsync(user, registerDTO.Password);
                if (!result2.Succeeded)
                    return BadRequest(result2.Errors);
                var roleResult = await userManager.AddToRoleAsync(user, registerDTO.Role);
                if (!roleResult.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return BadRequest(roleResult.Errors);
                }
                if (registerDTO.Role == "Owner")
                {
                    await unitOfWork.Repository<Owner>().AddAsync(new Owner
                    {
                        AppUserId = user.Id
                    });
                }
                else
                {
                    await unitOfWork.Repository<Tenant>().AddAsync(new Tenant
                    {
                        AppUserId = user.Id,
                        UniversityId = registerDTO.UniversityId.Value
                    });
                }
                await unitOfWork.CompleteAsync();
                await transaction.CommitAsync();
                var token = await tokenService.CreateToken(user, userManager);
                return Ok(new UserDto
                {
                    Name = user.UserName,
                    Email = user.Email,
                    Token = token
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest("Registration failed. Please try again.");
            }
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
        [HttpGet("Owners")]
        public async Task<ActionResult<Paginations<OwnerDto>>> GetOwners(
         [FromQuery] ApartmentSpecification Params)
        {
            var spec = new OwnersByAppUserIdsSpecification(Params);
            var OwnersQuery =await unitOfWork.Repository<Owner>().GetAll(spec);
            var totalCount = await unitOfWork.Repository<Owner>().GetCount(spec);
            var data = OwnersQuery.Select(o => new OwnerDto
            {
                Id = o.Id,
                Name = o.AppUser.Name,
                Email = o.AppUser.Email,
                PhoneNumber = o.AppUser.Phones.Select(p => p.PhoneNumber).ToList(),
                Photo = o.AppUser.Photo,
                NationalID = o.AppUser.NationalID
            }).ToList();




            return Ok(new Paginations<OwnerDto>(
                Params.Index,
                Params.PageSize,
                totalCount,
              data
            ));
        }
        [HttpGet("Owners/{id:int}")]
        public async Task<ActionResult> GetOwnerById(int id)
        {
           var spec = new OwnersByAppUserIdsSpecification(id);
            var owner = await unitOfWork.Repository<Owner>().GetEntitybySpec(spec);
            if (owner == null)
                return NotFound("Owner not found");
            var data = new OwnerDto
            {
                Id = owner.Id,
                Name = owner.AppUser.Name,
                Email = owner.AppUser.Email,
                PhoneNumber = owner.AppUser.Phones.Select(p => p.PhoneNumber).ToList(),
                Photo = owner.AppUser.Photo,
                NationalID = owner.AppUser.NationalID
            };

            return Ok(data);
        }
        [HttpGet("Tenants/{id:int}")]
        public async Task<ActionResult> GetTenantById(int id)
        {
            var spec = new TenantByAppUserIdsSpecification(id);
            var owner = await unitOfWork.Repository<Tenant>().GetEntitybySpec(spec);
            if (owner == null)
                return NotFound("Owner not found");
            var data = new TenantDto
            {
                Id = owner.Id,
                Name = owner.AppUser.Name,
                Email = owner.AppUser.Email,
                PhoneNumber = owner.AppUser.Phones.Select(p => p.PhoneNumber).ToList(),
                Photo = owner.AppUser.Photo,
                NationalID = owner.AppUser.NationalID,
                Role = "Tenant",
                UniversityId = owner.UniversityId
            };
            return Ok(data);
        }

        [HttpPut("Owners/{id:int}")]
        [Authorize(Roles = "Owner")]
        public async Task<ActionResult> UpdateOwnerProfile(int id, UpdateOwnerDTO updateOwnerDTO)
        {
            var ownerIdClaim = User.FindFirst("OwnerId")?.Value;
            if (ownerIdClaim == null || !int.TryParse(ownerIdClaim, out var currentOwnerId))
                return Unauthorized();

            var spec = new OwnersByAppUserIdsSpecification(id);
            var owner = await unitOfWork.Repository<Owner>().GetEntitybySpec(spec);
            if (owner == null)
                return NotFound("Owner not found");
            if (currentOwnerId != owner.Id)
                return Forbid();

            var user = owner.AppUser;

            if (!string.IsNullOrWhiteSpace(updateOwnerDTO.Name))
                user.Name = updateOwnerDTO.Name.Trim();

            if (!string.IsNullOrWhiteSpace(updateOwnerDTO.Photo))
                user.Photo = updateOwnerDTO.Photo.Trim();
         
            if (!string.IsNullOrWhiteSpace(updateOwnerDTO.Email) &&
                updateOwnerDTO.Email != user.Email)
            {
                var ExistCheckEmail = await userManager.FindByEmailAsync(updateOwnerDTO.Email) is not null;
                if (ExistCheckEmail)
                    return BadRequest("Email is already in use");

                var emailResult = await userManager.SetEmailAsync(user, updateOwnerDTO.Email.Trim());

                if (!emailResult.Succeeded)
                    return BadRequest(emailResult.Errors);

                var userNameResult = await userManager.SetUserNameAsync(user, updateOwnerDTO.Email.Trim());

                if (!userNameResult.Succeeded)
                    return BadRequest(userNameResult.Errors);
            }
            if (updateOwnerDTO.PhoneNumber != null)
            {
                owner.AppUser.Phones = updateOwnerDTO.PhoneNumber
                    .Where(p => !string.IsNullOrWhiteSpace(p))
                    .Select(p => new Phones { PhoneNumber = p.Trim() })
                    .ToList();
            }
           
            var result = await userManager.UpdateAsync(owner.AppUser);
             unitOfWork.Repository<Owner>().Update(owner);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            return NoContent();
        }

        [HttpDelete("Owners/{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteOwner(int id)
        {
            var spec = new OwnersByAppUserIdsSpecification(id);
            var owner = await unitOfWork.Repository<Owner>().GetEntitybySpec(spec);
            if (owner == null)
                return NotFound("Owner not found");
            if (owner.Apartments != null && owner.Apartments.Any())
                return BadRequest("Cannot delete owner because this owner has apartments.");

            unitOfWork.Repository<Owner>().Delete(owner);
            await unitOfWork.CompleteAsync();

            var result = await userManager.DeleteAsync(owner.AppUser);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return NoContent();
        }

        [HttpPost("AddUniversity")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UniversityDTO>> AddUNversity([FromBody] UniversityDTO NameU)
        {

            var university = new University
            {
                Name = NameU.Name,
                City = NameU.City,
                Area = NameU.Area,
            };

            await unitOfWork.Repository<University>().AddAsync(university);

            await unitOfWork.CompleteAsync(); // or SaveChangesAsync()

            return Ok(university);
        }
    }
}

 
