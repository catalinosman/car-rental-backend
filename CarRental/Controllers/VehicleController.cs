using AutoMapper;
using CarRental.Dto;
using CarRental.Models;
using CarRental.Services;
using CarRental.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace CarRental.Controllers
{
    public class VehicleController : ApiBaseController
    {
        private readonly IVehicleService _vehicleService;
        private readonly IMapper _mapper;

        public VehicleController(IVehicleService vehicleService, IMapper mapper)
        {
            _vehicleService = vehicleService;
            _mapper = mapper;
        }
      

        [HttpGet("all")]
        public async Task<IActionResult> GetVehicles(string? categoryId = null, string? orderBy = null, string? name = null)
        {
            ICollection<Vehicle> vehicles;

            if (!string.IsNullOrEmpty(categoryId))
            {
                if (!int.TryParse(categoryId, out int categoryIdValue))
                {
                    return BadRequest($"Invalid categoryId value: {categoryId}");
                }

                vehicles = await _vehicleService.GetVehiclesByCategory(categoryIdValue);
            }
            else
            {
                vehicles = await _vehicleService.GetVehicles();
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                vehicles = vehicles.Where(p => p.Name.ToLower().Contains(name.ToLower())).ToList();
            }

            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                if (orderBy.ToLower() == "a-z")
                {
                    vehicles = vehicles.OrderBy(p => p.Name).ToList();
                }
                else if (orderBy.ToLower() == "z-a")
                {
                    vehicles = vehicles.OrderByDescending(p => p.Name).ToList();
                }
                else if (orderBy.ToLower() == "highestprice")
                {
                    vehicles = vehicles.OrderByDescending(p => p.Price).ToList();
                }
                else if (orderBy.ToLower() == "lowestprice")
                {
                    vehicles = vehicles.OrderBy(p => p.Price).ToList();
                }
            }

            var vehiclesDto = _mapper.Map<List<VehicleDto>>(vehicles);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(vehiclesDto);
        }

        [HttpGet("{vehicleId}")]
        public async Task<IActionResult>GetVehicle(int vehicleId)
        {
            var vehicleExists = await _vehicleService.VehicleExists(vehicleId);
            if(!vehicleExists)
            {
                return NotFound();
            }

            var vehicle = await _vehicleService.GetVehicle(vehicleId);
            var vehicleDto = _mapper.Map<VehicleDto>(vehicle);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(vehicleDto);
        }


        [HttpPost, Authorize( Roles = "Admin")]
        public async Task<IActionResult> CreateVehicle([FromBody] VehicleDto vehicleCreate, [FromQuery] int categoryId) 
        {
            var vehicles = await _vehicleService.GetVehicles();
            var vehicle = vehicles.
                Where(v => v.Name.Trim().ToUpper() == vehicleCreate.Name.ToUpper()).
                FirstOrDefault();

            if(vehicle != null) 
            {
                ModelState.AddModelError("", "Vehicle already exists");
                return StatusCode(422, ModelState);
            }

            var vehicleMap = _mapper.Map<Vehicle>(vehicleCreate);

            if(!await _vehicleService.CreateVehicle(vehicleMap, categoryId)) 
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok(vehicleMap);
        }

        [HttpPut("{vehicleId}"), Authorize( Roles = "Admin" )]
        public async Task<IActionResult> UpdateVehicle(VehicleDto updatedVehicle, int vehicleId)
        {
         
            if ( updatedVehicle == null )
            {
                return BadRequest(ModelState);
            }

            if (!await _vehicleService.VehicleExists(vehicleId)) 
            {
                return NotFound();
            }

            var vehicleMap = _mapper.Map<VehicleDto>(updatedVehicle);

            if (!await _vehicleService.UpdateVehicle(vehicleMap, vehicleId)) 
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }
            
            return Ok(vehicleMap);
        }

        [HttpDelete("{vehicleId}"), Authorize (Roles = "Admin")]
        public async Task<IActionResult> DeteleVehicle(int vehicleId) 
        { 
            if(!await _vehicleService.VehicleExists(vehicleId)) 
            {
                return NotFound();
            }

            var vehicleDelete = await _vehicleService.GetVehicle(vehicleId);

            if (!await _vehicleService.DeleteVehicle(vehicleDelete)) 
            {
                ModelState.AddModelError("","Something went wrong");
            }
            return Ok("Success");
        }
    }
}

