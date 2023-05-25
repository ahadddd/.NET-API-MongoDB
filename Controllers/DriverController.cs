using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAPIMongo.Services;

namespace WebAPIMongo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DriverController : Controller
    {
        private readonly DriverService _driverService;
        

        public DriverController(DriverService driverService)
        {
            _driverService = driverService;
            
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Driver>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetDrivers()
        {
            var drivers = await _driverService.GetDrivers();
            return Ok(drivers);
        }

        [HttpGet("{number}")]
        [ProducesResponseType(200, Type = typeof(Driver))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetDriver(int number)
        {
            var driver = await _driverService.GetDriver(number);
            if (driver == null)
                return NotFound();
            return Ok(driver);
        }


        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateDriver([FromBody] Driver driverCreate)
        {
            if(driverCreate == null)
            {
                ModelState.AddModelError("", "Something went wrong.");
                BadRequest(ModelState);
            }
            Driver driver = await _driverService.GetDriver(driverCreate.Number);
            if (driver != null)
            {
                Console.WriteLine(driver);
                ModelState.AddModelError("", "Driver already exists.");
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
                BadRequest(ModelState);

            await _driverService.CreateDriver(driverCreate);
            return Ok("Driver added.");
        }
    }
}
