using ApartmentRental.Core.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AparatmentRental.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private readonly IUnitOfWork UnitOfWork;
        private readonly IMapper Mapper;
        public OwnerController(IUnitOfWork _unitOfWork, IMapper _Mapper)
        {
            UnitOfWork = _unitOfWork;
            Mapper = _Mapper;
        }
 
    
    }
}
