using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAPIMongo.DTO;
using WebAPIMongo.Services;

namespace WebAPIMongo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DriverController : Controller
    {
        private readonly DriverService _driverService;
        private readonly IMapper _mapper;

        public DriverController(DriverService driverService, IMapper mapper)
        {
            _driverService = driverService;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Driver>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetDrivers()
        {
            var drivers = await _driverService.GetDrivers();
            var driverMaps = _mapper.Map<List<DriverDTO>>(drivers);
            return Ok(driverMaps);
        }

        [HttpGet("{number}")]
        [ProducesResponseType(200, Type = typeof(Driver))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetDriver(int number)
        {
            var driver = await _driverService.GetDriver(number);
            var driverMap = _mapper.Map<DriverDTO>(driver);
            if (driver == null)
                return NotFound();
            return Ok(driverMap);
        }


        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateDriver([FromBody] DriverDTO driverCreate)
        {
            if(driverCreate == null)
            {
                ModelState.AddModelError("", "Something went wrong.");
                BadRequest(ModelState);
            }
           
            else
            {
                Driver driver = await _driverService.GetDriver(driverCreate.Number);
                if (driver != null)
                {
                    ModelState.AddModelError("", "Driver already exists.");
                    return BadRequest(ModelState);
                }
                else
                {
                    var driverMap = _mapper.Map<Driver>(driverCreate);
                    await _driverService.CreateDriver(driverMap);
                }
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok("Driver added.");
        }


        [HttpPut("{number}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateDriver(int number, [FromBody] DriverDTO driverUpdate)
        {
            if (driverUpdate == null)
                return BadRequest(ModelState);
            var existingdriver = await _driverService.GetDriverWithId(number);
            if (existingdriver == null)
                return NotFound();
            if (number != driverUpdate.Number)
                return BadRequest(ModelState);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var driverMap = _mapper.Map<Driver>(driverUpdate);
            driverMap.Id = existingdriver.Id;
            await _driverService.UpdateDriver(driverMap);
            
            return NoContent();
        }

        [HttpDelete("{number}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteDriver(int number)
        {
            var driver = _driverService.GetDriver(number);
            if(driver == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _driverService.DeleteDriver(number);
            return NoContent();
        }


    }
}
