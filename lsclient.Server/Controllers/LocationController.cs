using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lsclient.Server.Models; 
using lsclient.Server.Shared;

namespace lsclient.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LocationController(AppDbContext context)
        {
            _context = context;
        }

        #region GetAllLocations
        [HttpGet]
        [Route("GetAllLocations")]
        public IActionResult GetAllLocations()
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(GetAllLocations);

            try
            {
                var locations = _context.Locations
                    .Include(l => l.Truck)
                    .ToList();
                executionResult.SetData(locations);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(LocationController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion

        #region GetLocationById
        [HttpGet]
        [Route("GetLocationById/{id}")]
        public IActionResult GetLocationById(string id)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(GetLocationById);

            try
            {
                var location = _context.Locations
                    .Include(l => l.Truck)
                    .FirstOrDefault(l => l.LocationID == id);

                if (location == null)
                {
                    return NotFound("Location not found");
                }

                executionResult.SetData(location);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(LocationController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion

        #region AddLocation
        [HttpPost]
        [Route("AddLocation")]
        public async Task<IActionResult> AddLocation([FromBody] Location newLocation)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(AddLocation);

            try
            {
                newLocation.LocationID = Functions.GenerateLocationId();
                _context.Locations.Add(newLocation);
                await _context.SaveChangesAsync();

                executionResult.SetData(newLocation);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(LocationController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion

        #region UpdateLocation
        [HttpPut]
        [Route("UpdateLocation/{id}")]
        public async Task<IActionResult> UpdateLocation(string id, [FromBody] Location updatedLocation)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(UpdateLocation);

            try
            {
                var location = await _context.Locations.FindAsync(id);

                if (location == null)
                {
                    return NotFound("Location not found");
                }

                location.TruckID = updatedLocation.TruckID;
                location.Latitude = updatedLocation.Latitude;
                location.Longitude = updatedLocation.Longitude;
                location.Timestamp = updatedLocation.Timestamp;

                await _context.SaveChangesAsync();

                executionResult.SetData(location);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(LocationController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion

        #region DeleteLocation
        [HttpDelete]
        [Route("DeleteLocation/{id}")]
        public async Task<IActionResult> DeleteLocation(string id)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(DeleteLocation);

            try
            {
                var location = await _context.Locations.FindAsync(id);

                if (location == null)
                {
                    return NotFound("Location not found");
                }

                _context.Locations.Remove(location);
                await _context.SaveChangesAsync();

                executionResult.SetData(location);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(LocationController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion
    }
}
