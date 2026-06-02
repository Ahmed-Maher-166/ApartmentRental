using AparatmentRental.Api.DTOS;
using ApartmentRental.Core.Models;
using ApartmentRental.Core.Repository;
using ApartmentRental.Core.Specification;
using ApartmentRental.Reporisitory;
using AutoMapper;
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
        public async Task<ActionResult<IReadOnlyList<ApartmentDTO>>> GetApartment([FromQuery] ApartmentSpecification Params)
        {
            var Specification =  new ApartmentWithPhotoSpecification(Params);
            var apartment = await UnitOfWork.Repository<Apartment>().GetAll(Specification);
            var apartmentDTO = Mapper.Map<IReadOnlyList<Apartment>, IReadOnlyList<ApartmentDTO>>(apartment);
            return Ok(apartmentDTO);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ApartmentDTO>> GetApartmentById(int id)
        {
            var Specification = new ApartmentWithPhotoSpecification(id);
            var apartment = await UnitOfWork.Repository<Apartment>().GetEntitybySpec(Specification);
            if (apartment == null) return NotFound();
            var apartmentDTO = Mapper.Map<Apartment, ApartmentDTO>( apartment);
            return Ok(apartmentDTO);
        }


    }
}
