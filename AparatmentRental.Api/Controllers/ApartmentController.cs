using AparatmentRental.Api.DTOS;
using ApartmentRental.Core.Models;
using ApartmentRental.Core.Repository;
using ApartmentRental.Core.Specification;
using ApartmentRental.Reporisitory;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AparatmentRental.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApartmentController : ControllerBase
    {
        private readonly IUnitOfWork UnitOfWork;
        private readonly IMapper Mapper;
        public ApartmentController(IUnitOfWork _unitOfWork, IMapper _Mapper)
        {
            UnitOfWork = _unitOfWork;
            Mapper = _Mapper;
        }
        [HttpGet]
        public async Task<ActionResult<Paginations<ApartmentDTO>>> GetApartment([FromQuery] ApartmentSpecification Params)
        {
            var specification = new ApartmentWithPhotoSpecification(Params);
            var apartments = await UnitOfWork.Repository<Apartment>().GetAll(specification);
            var apartmentDTO = Mapper.Map<IReadOnlyList<Apartment>, IReadOnlyList<ApartmentDTO>>(apartments);
            var countSpecification = new ApartmentWithFilteration(Params);
            var count = await UnitOfWork.Repository<Apartment>().GetCount(countSpecification);
            return Ok(new Paginations<ApartmentDTO>(
                Params.Index,
                Params.PageSize,
                count,
                apartmentDTO
            ));
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ApartmentDTO>> GetApartmentById(int id)
        {
            var Specification = new ApartmentWithPhotoSpecification(id);
            var apartment = await UnitOfWork.Repository<Apartment>().GetEntitybySpec(Specification);
            if (apartment == null) return NotFound();
            var apartmentDTO = Mapper.Map<Apartment, ApartmentDTO>(apartment);
            return Ok(apartmentDTO);
        }
        [HttpPost("AddApartment")]
        [Authorize(Roles = "Owner")]
        public async Task<ActionResult<ApartmentDTO>> CreateApartment(CreateApartmentDTO apartmentDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var ownerIdClaim = User.FindFirst("OwnerId")?.Value;

            if (ownerIdClaim == null)
                return Unauthorized("OwnerId not found in token.");

            if (!int.TryParse(ownerIdClaim, out var currentOwnerId))
                return Unauthorized("Invalid OwnerId in token.");

            var apartment = Mapper.Map<Apartment>(apartmentDTO);
            apartment.OwnerId = currentOwnerId;
            await UnitOfWork.Repository<Apartment>().AddAsync(apartment);
            var result = await UnitOfWork.CompleteAsync();
            if (result <= 0)
                return BadRequest("No changes were saved.");
            var createdApartment = Mapper.Map<Apartment, ApartmentDTO>(apartment);
            return CreatedAtAction(
                  nameof(GetApartmentById),
                  new { id = createdApartment.Id },
                  createdApartment
              );

        }
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Owner")]
        public async Task<ActionResult<ApartmentDTO>> UpdateApartment([FromRoute] int id,
         [FromBody] UpdateApartmentDTO apartmentDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var ownerIdClaim = User.FindFirst("OwnerId")?.Value;
            if (ownerIdClaim == null)
                return Unauthorized("OwnerId not found in token.");
            if (!int.TryParse(ownerIdClaim, out var currentOwnerId))
                return Unauthorized("Invalid OwnerId in token.");
            var existingApartment = await UnitOfWork.Repository<Apartment>().GetByIdAsync(id);
            if (existingApartment is null)
                return NotFound();
            if (existingApartment.OwnerId != currentOwnerId)
                return Forbid();
            Mapper.Map(apartmentDTO, existingApartment);
            UnitOfWork.Repository<Apartment>().Update(existingApartment);
            await UnitOfWork.CompleteAsync();

            var updatedApartment = Mapper.Map<Apartment, ApartmentDTO>(existingApartment);
            return Ok(updatedApartment);
        }
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Owner")]
        public async Task<ActionResult> DeleteApartment([FromRoute] int id)
        {
            var ownerIdClaim = User.FindFirst("OwnerId")?.Value;

            if (ownerIdClaim == null)
                return Unauthorized("OwnerId not found in token.");

            if (!int.TryParse(ownerIdClaim, out var currentOwnerId))
                return Unauthorized("Invalid OwnerId in token.");
            var existingApartment = await UnitOfWork.Repository<Apartment>().GetByIdAsync(id);
            if (existingApartment is null)
                return NotFound();
            if (existingApartment.OwnerId != currentOwnerId)
                return Forbid();

            UnitOfWork.Repository<Apartment>().Delete(existingApartment);
            var result = await UnitOfWork.CompleteAsync();

            if (result <= 0)
                return BadRequest("No changes were saved.");
            return NoContent();
        }
        [HttpPost("{id:int}/Photos")]
        [Authorize(Roles = "Owner")]

        public async Task<ActionResult<List<string>>> GetApartmentPhotos([FromRoute] int id, [FromBody] List<string> photoUrls)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var ownerIdClaim = User.FindFirst("OwnerId")?.Value;
            if (ownerIdClaim == null)
                return Unauthorized("OwnerId not found in token.");
            if (!int.TryParse(ownerIdClaim, out var currentOwnerId))
                return Unauthorized("Invalid OwnerId in token.");
            var apartment = await UnitOfWork.Repository<Apartment>().GetByIdAsync(id);
            if (apartment is null)
                return NotFound();
            if (apartment.OwnerId != currentOwnerId)
                return Forbid();
            foreach (var url in photoUrls)
            {
                apartment.Photos.Add(new Photos
                {
                    PhotoUrl = url,
                    ApartmentId = id
                });
            }

            await UnitOfWork.CompleteAsync();

            return Ok(apartment.Photos.Select(p => p.PhotoUrl).ToList());

        }

        [HttpDelete("{id:int}/Photos/{photoId}")]
        [Authorize(Roles = "Owner")]
        public async Task<ActionResult> DeletePhoto(
        [FromRoute] int id, [FromRoute] int photoId)
        {
            var ownerIdClaim = User.FindFirst("OwnerId")?.Value;
            if (ownerIdClaim == null || !int.TryParse(ownerIdClaim, out var currentOwnerId))
                return Unauthorized();

            var apartment = await UnitOfWork.Repository<Apartment>().GetByIdAsync(id);
            if (apartment == null) return NotFound();

            if (apartment.OwnerId != currentOwnerId) return Forbid();

            var photo = await UnitOfWork.Repository<Photos>().GetByIdAsync(photoId);
            if (photo == null) return NotFound();
            if (photo.ApartmentId != id)
                return BadRequest("This photo does not belong to this apartment.");
            UnitOfWork.Repository<Photos>().Delete(photo);
            await UnitOfWork.CompleteAsync();

            return NoContent();
        }
 
    }
}
