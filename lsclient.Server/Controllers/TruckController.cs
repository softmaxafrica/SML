using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lsclient.Server.Models; 
using lsclient.Server.Shared;
using lsclient.Server.Models.DataPayloads;

namespace lsclient.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TruckController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TruckController(AppDbContext context)
        {
            _context = context;
        }

        #region GetAllTrucks
        [HttpGet]
        [Route("GetAllTrucks")]
        public async Task<IActionResult> GetAllTrucks()
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(GetAllTrucks);

            try
            {
                var trucks = await _context.Trucks
                   .Include(t => t.Company)
                   .Include(j => j.JobRequests)
                   .Include(p => p.TruckType)
                   .Include(d => d.Driver)
                 //.Include(t => t.Locations)   // Include Locations
                    .ToListAsync();

                executionResult.SetData(trucks);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(TruckController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion

        #region GetTruckById
        [HttpGet]
        [Route("GetTruckById/{id}")]
        public IActionResult GetTruckById(string id)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(GetTruckById);

            try
            {
                var truck = _context.Trucks
                    .Include(t => t.Company)
                    //.Include(t => t.JobRequests)
                    .FirstOrDefault(t => t.TruckID == id);

                if (truck == null)
                {
                    return NotFound("Truck not found");
                }

                executionResult.SetData(truck);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(TruckController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion

        #region GetAvailableTrucksByTruckType
        [HttpGet]
        [Route("GetAvailableTrucksByTruckType/{truckType}/{companyID}")]
        public async Task<IActionResult> GetAvailableTrucksByTruckType(string truckType, string companyID)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(GetAvailableTrucksByTruckType);

            try
            {
                if (string.IsNullOrWhiteSpace(truckType) || string.IsNullOrWhiteSpace(companyID))
                {
                    return BadRequest("TruckType and CompanyID must not be empty.");
                }

                var trucks = await _context.Trucks
                    .Include(t => t.Company)
                    .Include(t => t.TruckType)
                    .Include(t => t.JobRequests)
                    .Include(t => t.Driver)
                    .Where(t => t.TruckType.TruckTypeID == truckType && t.CompanyID == companyID)
                    .ToListAsync();

                if (!trucks.Any())
                {
                    return NotFound("No trucks are available matching the provided criteria at this time. Please select a different truck type or try again later.");
                }

                executionResult.SetData(trucks);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(TruckController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion

        #region GetTruckByCompanyId
        [HttpGet]
        [Route("GetTruckByCompanyId/{CompanyId}")]
        public IActionResult GetTruckByCompanyId(string CompanyId)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(GetTruckById);

            try
            {
                var truck = _context.Trucks
                    .Include(t => t.Company)
                     .Include(p => p.TruckType)
                    .Include(j => j.JobRequests)
                    .Include(d => d.Driver)
                    .Where(c => c.Company.CompanyID == CompanyId);

                if (truck == null)
                {
                    return NotFound("No Truck Registered For This Company");
                }

                executionResult.SetData(truck);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(TruckController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion

        //#region AddTruck
        //[HttpPost]
        //[Route("AddTruck")]
        //public async Task<IActionResult> AddTruck([FromBody] TrucksPayload newTruck)
        //{
        //    var executionResult = new ExecutionResult();
        //    string functionName = nameof(AddTruck);

        //    Truck data = new Truck();

        //    try
        //    {

        //        data.TruckID = Functions.GenerateTruckId();
        //        data.TruckNumber = newTruck.TruckNumber;
        //        data.Model = newTruck.Model;
        //        data.TruckTypeID = newTruck.TruckTypeID;
        //        data.CompanyID = newTruck.CompanyID;
        //        data.IsTruckAvilableForBooking=true;
        //        data.IsActive = true;

        //        _context.Trucks.Add(data);
        //        await _context.SaveChangesAsync();

        //        executionResult.SetData(newTruck);
        //        return Ok(executionResult.GetServerResponse());
        //    }
        //    catch (Exception ex)
        //    {
        //        executionResult.SetInternalServerError(nameof(TruckController), functionName, ex);
        //        return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
        //    }
        //}
        //#endregion
        #region AddTruck
        [HttpPost]
        [Route("AddTruck")]
        public async Task<IActionResult> AddTruck([FromBody] TrucksPayload newTruck)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(AddTruck);

            Truck data = new Truck();

            try
            {
                data.TruckID = Functions.GenerateTruckId();
                data.TruckNumber = newTruck.TruckNumber;
                data.Model = newTruck.Model;
                data.TruckTypeID = newTruck.TruckTypeID;
                data.CompanyID = newTruck.CompanyID;
                data.IsTruckAvilableForBooking = true;
                data.IsActive = true;

                // Optional fields can be set only if provided
                 data.FuelType = newTruck.FuelType;
                 data.CabinType = newTruck.CabinType;
                 data.Category = newTruck.Category;
                 data.Drive = newTruck.Drive;
                _context.Trucks.Add(data);
                await _context.SaveChangesAsync();

                executionResult.SetData(newTruck);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(TruckController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion

        //#region UpdateTruck
        //[HttpPut]
        //[Route("UpdateTruckByTruckId/{TruckId}")]
        //public async Task<IActionResult> UpdateTruck(string TruckId, [FromBody] TrucksPayload updatedTruck)
        //{
        //    var executionResult = new ExecutionResult();
        //    string functionName = nameof(UpdateTruck);

        //    try
        //    {
        //        var truck =  _context.Trucks.Where(t => t.TruckID == TruckId).FirstOrDefault();

        //        if (truck == null)
        //        {
        //            return NotFound("Truck not found");
        //        }

        //        truck.TruckID = updatedTruck.TruckID;
        //        truck.TruckTypeID = updatedTruck.TruckTypeID;
        //        truck.Model = updatedTruck.Model;
        //        truck.CompanyID = updatedTruck.CompanyID;
        //        truck.DriverID = updatedTruck.DriverID;
        //        truck.IsActive = (bool)updatedTruck.IsActive;
        //        truck.IsTruckAvilableForBooking= (bool)updatedTruck.IsTruckAvilableForBooking;

        //        await _context.SaveChangesAsync();

        //        executionResult.SetData(truck);
        //        return Ok(executionResult.GetServerResponse());
        //    }
        //    catch (Exception ex)
        //    {
        //        executionResult.SetInternalServerError(nameof(TruckController), functionName, ex);
        //        return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
        //    }
        //}
        //#endregion
        #region UpdateTruck
        [HttpPut]
        [Route("UpdateTruckByTruckId/{TruckId}")]
        public async Task<IActionResult> UpdateTruck(string TruckId, [FromBody] TrucksPayload updatedTruck)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(UpdateTruck);

            try
            {
                var truck = _context.Trucks.FirstOrDefault(t => t.TruckID == TruckId);

                if (truck == null)
                {
                    return NotFound("Truck not found");
                }

                // Update only provided fields
                if (!string.IsNullOrEmpty(updatedTruck.TruckNumber))
                {
                    truck.TruckNumber = updatedTruck.TruckNumber;
                }

                if (!string.IsNullOrEmpty(updatedTruck.Model))
                {
                    truck.Model = updatedTruck.Model;
                }

                if (!string.IsNullOrEmpty(updatedTruck.TruckTypeID))
                {
                    truck.TruckTypeID = updatedTruck.TruckTypeID;
                }

                if (!string.IsNullOrEmpty(updatedTruck.CompanyID))
                {
                    truck.CompanyID = updatedTruck.CompanyID;
                }

                if (!string.IsNullOrEmpty(updatedTruck.DriverID))
                {
                    truck.DriverID = updatedTruck.DriverID;
                }

                if (updatedTruck.IsActive.HasValue)
                {
                    truck.IsActive = updatedTruck.IsActive.Value;
                }

                if (updatedTruck.IsTruckAvilableForBooking.HasValue)
                {
                    truck.IsTruckAvilableForBooking = updatedTruck.IsTruckAvilableForBooking.Value;
                }

                // Update optional fields if provided
                if (!string.IsNullOrEmpty(updatedTruck.FuelType))
                {
                    truck.FuelType = updatedTruck.FuelType;
                }

                if (!string.IsNullOrEmpty(updatedTruck.CabinType))
                {
                    truck.CabinType = updatedTruck.CabinType;
                }

                if (!string.IsNullOrEmpty(updatedTruck.Category))
                {
                    truck.Category = updatedTruck.Category;
                }
                if (!string.IsNullOrEmpty(updatedTruck.Drive))
                {
                    truck.Drive = updatedTruck.Drive;
                }

                await _context.SaveChangesAsync();

                executionResult.SetData(truck);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(TruckController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion

        #region DeleteTruck
        [HttpDelete]
        [Route("DeleteTruckByTruckId/{ByTruckId}")]
        public async Task<IActionResult> DeleteTruck(string ByTruckId)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(DeleteTruck);

            try
            {
 
                var truck =  _context.Trucks.Where(t => t.TruckID == ByTruckId).FirstOrDefault();

                if (truck == null)
                {
                    return NotFound("Truck not found");
                }

                _context.Trucks.Remove(truck);
                await _context.SaveChangesAsync();

                executionResult.SetData(truck);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(TruckController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion
    }
}
