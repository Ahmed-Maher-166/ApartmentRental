using AparatmentRental.Api.DTOS;
using ApartmentRental.Core.Models;
using ApartmentRental.Core.Models.Identity;
using ApartmentRental.Core.Repository;
using ApartmentRental.Core.Services;
using ApartmentRental.Core.Specification;
using ApartmentRental.Reporisitory.Identity;
using ApartmentRental.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
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
        private readonly AppIdentityDbContext context;
        public AccountsController(
    UserManager<AppUser> _userManager,
    SignInManager<AppUser> _signInManager,
    ITokenService _tokenService,
    IUnitOfWork _unitOfWork,
    AppIdentityDbContext _context)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            tokenService = _tokenService;
            unitOfWork = _unitOfWork;
            context = _context;
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
            var ExistCheckEmail = await  userManager.FindByEmailAsync(registerDTO.Email) is not null;
            if (ExistCheckEmail)
                return BadRequest("Email is already in use");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var user = new AppUser
            {
                UserName = registerDTO.Name,
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

            await using var transaction = await context.Database.BeginTransactionAsync();

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
            Params ??= new ApartmentSpecification();

            var pageIndex = Params.Index <= 0 ? 1 : Params.Index;
            var pageSize = Params.PageSize <= 0 ? 10 : Params.PageSize;

            var query =
                from user in userManager.Users.AsNoTracking().Include(u => u.Phones)
                join userRole in context.UserRoles.AsNoTracking()
                    on user.Id equals userRole.UserId
                join role in context.Roles.AsNoTracking()
                    on userRole.RoleId equals role.Id
                where role.Name == "Owner"
                select user;

            var totalCount = await query.CountAsync();

            var users = await query
                .OrderBy(u => u.UserName)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var appUserIds = users
                .Select(u => u.Id)
                .ToList();

            var ownerSpec = new OwnersByAppUserIdsSpecification(appUserIds);

            var ownerEntities = await unitOfWork.Repository<Owner>()
                .GetAll(ownerSpec);

            var owners = users
                .Select(user =>
                {
                    var ownerEntity = ownerEntities
                        .FirstOrDefault(o => o.AppUserId == user.Id);

                    return new OwnerDto
                    {
                        Id = ownerEntity.Id,
                        Name = user.UserName,
                        Email = user.Email,
                        Photo = user.Photo,
                        NationalID = user.NationalID,
                        Role = "Owner",
                        PhoneNumber = user.Phones
                            .Select(p => p.PhoneNumber)
                            .ToList()
                    };
                })
                .ToList();

            return Ok(new Paginations<OwnerDto>(
                pageIndex,
                pageSize,
                totalCount,
                owners
            ));
        }
        [HttpGet("Owners/{id:int}")]
        public async Task<ActionResult> GetOwnerById(int id)
        {
            var owner = await unitOfWork.Repository<Owner>().GetByIdAsync(id);
            if (owner == null)
                return NotFound("Owner not found");
            return await GetUserProfileAsync(owner.AppUserId, "Owner" ,id);
        }
        [HttpGet("Tenants/{id:int}")]
        public async Task<ActionResult> GetTenantById(int id)
        {
            var Tenant = await unitOfWork.Repository<Tenant>().GetByIdAsync(id);
            if (Tenant == null)
                return NotFound("Tenant not found");

            return await GetUserProfileAsync(Tenant.AppUserId, "Tenant", id);
        }
        private async Task<ActionResult> GetUserProfileAsync(string appUserId, string role, int id)
        {
            var user = await userManager.Users
                .Include(u => u.Phones)
                .FirstOrDefaultAsync(u => u.Id == appUserId);

            if (user == null)
                return NotFound("User not found");

            return Ok(new OwnerDto
            {
                Id = id,
                Name = user.UserName,
                Email = user.Email,
                Photo = user.Photo,
                NationalID = user.NationalID,
                Role = role,
                PhoneNumber = user.Phones.Select(p => p.PhoneNumber).ToList()
            });
        }
   
    }
 }
