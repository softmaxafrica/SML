using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lsclient.Server.Models; 
using lsclient.Server.Shared;
using lsclient.Server.Controllers.SecurityUser;
using lsclient.Server.Models.DataPayloads;
using System.ComponentModel.Design;
using System.Reflection.Metadata;

namespace lsclient.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _environment; // Add this field

        public DriverController(AppDbContext context, IConfiguration config, IWebHostEnvironment environment)
        {
            _context = context;
            _config = config;
            _environment = environment;
        }
      
        #region RegisterDriver
        [HttpPost]
        [Route("RegisterDriver")]
        public async Task<IActionResult> RegisterDriver([FromForm] DriverPayload newDriver, [FromForm] IFormFile? file)
        {
            var functionName = nameof(RegisterDriver);
            var executionResult = new ExecutionResult();
            Driver data = new Driver();

            try
            {
                // Check if email already exists
                var existingDriver = await _context.Drivers
                                                   .FirstOrDefaultAsync(d => d.Email == newDriver.Email);

                if (existingDriver != null)
                {
                    executionResult.SetFailure("Email is already registered. Please use a different email.");
                    return BadRequest(executionResult.GetServerResponse().Message);
                }

                data.DriverID = Functions.GenerateDriverId();
                data.FullName = newDriver.FullName;
                data.LicenseNumber = newDriver.LicenseNumber;
                data.Status = newDriver.Status;
                data.Email = newDriver.Email;
                data.CompanyID = newDriver.CompanyID;
                data.Phone = newDriver.Phone;
                data.RegstrationComment = newDriver.RegstrationComment;
                data.LicenseClasses = newDriver.LicenseClasses;
                data.LicenseExpireDate = newDriver.LicenseExpireDate;
                data.isAvilableForBooking = false;

                // Save the image file if provided
                if (file != null)
                {
                    var folderPath = Path.Combine(_environment.WebRootPath, "assets", "images", "drivers_profile");

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    // Get the file extension
                    var fileExtension = Path.GetExtension(file.FileName);
                    var fileName = $"{data.DriverID}{fileExtension}";  // Save the file with correct extension
                    var filePath = Path.Combine(folderPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // Set the ImageUrl in the Driver entity
                    data.ImageUrl = Path.Combine("assets", "images", "drivers_profile", fileName).Replace("\\", "/");
                }

                _context.Drivers.Add(data);
                await _context.SaveChangesAsync();

                // Send the default password to the user via email
                executionResult.SetData(data);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(CompanyController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion


        #region GetAllDrivers
        [HttpGet]
        [Route("GetAllDrivers")]
        public IActionResult GetAllDrivers()
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(GetAllDrivers);

            try
            {
                var drivers = _context.Drivers
                    .Include(d => d.Company)
                    .Include(d => d.TruckTypes)
                    .ToList();
                executionResult.SetData(drivers);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(DriverController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion

        #region GetCompanyDrivers
        [HttpGet]
        [Route("GetCompanyDrivers/{CompanyId}")]
        public IActionResult GetCompanyDrivers(string CompanyId)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(GetCompanyDrivers);

            try
            {
                using (var db = new AppDbContext(_config))
                {
                    var drivers = db.Drivers
                                    .Include(d => d.Company)
                                    .Include(d => d.TruckTypes)
                                    .Where(d => d.Company.CompanyID == CompanyId)
                                    .ToList();

                    if (!drivers.Any())
                    {
                        executionResult.SetNotFound(CompanyId);
                        return NotFound(executionResult.GetServerResponse());
                    }

                    executionResult.SetData(drivers);
                    return Ok(executionResult.GetServerResponse());
                }
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(CompanyController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion

        #region GetAvilableCompanyDrivers
        [HttpGet]
        [Route("GetAvilableCompanyDrivers/{CompanyId}")]
        public IActionResult GetAvilableCompanyDrivers(string CompanyId)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(GetCompanyDrivers);

            try
            {
                using (var db = new AppDbContext(_config))
                {
                    var drivers = db.Drivers
                                    .Include(d => d.Company)
                                    .Include(d => d.TruckTypes)
                                    .Where(d => d.Company.CompanyID == CompanyId && d.isAvilableForBooking==true && d.Status=="ACTIVE")
                                    .ToList();

                    if (!drivers.Any())
                    {
                        executionResult.SetNotFound(CompanyId);
                        return NotFound(executionResult.GetServerResponse());
                    }

                    executionResult.SetData(drivers);
                    return Ok(executionResult.GetServerResponse());
                }
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(CompanyController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }

        #endregion
        #region GetDriverById
        [HttpGet]
        [Route("GetDriverById/{id}")]
        public IActionResult GetDriverById(string id)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(GetDriverById);

            try
            {
                var driver = _context.Drivers
                    .Include(d => d.Company)
                    .Include(d => d.TruckTypes)
                    .FirstOrDefault(d => d.DriverID == id);

                if (driver == null)
                {
                    return NotFound("Driver not found");
                }

                executionResult.SetData(driver);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(DriverController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion

        #region AddDriver
        [HttpPost]
        [Route("AddDriver")]
        public async Task<IActionResult> AddDriver([FromBody] Driver newDriver)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(AddDriver);

            try
            {
                newDriver.DriverID = Functions.GenerateDriverId();
                _context.Drivers.Add(newDriver);
                await _context.SaveChangesAsync();

                executionResult.SetData(newDriver);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(DriverController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion

        #region UpdateDriver
        [HttpPut]
        [Route("UpdateDriver/{id}")]
        public async Task<IActionResult> UpdateDriver(string id,   DriverPayload updatedDriver)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(UpdateDriver);

            try
            {
                var driver = await _context.Drivers.FindAsync(id);

                if (driver == null)
                {
                    return NotFound("Driver not found");
                }

                 if (!string.IsNullOrWhiteSpace(updatedDriver.FullName))
                {
                    driver.FullName = updatedDriver.FullName;
                }
                if (!string.IsNullOrWhiteSpace(updatedDriver.Email))
                {
                    driver.Email = updatedDriver.Email;
                }
                if (!string.IsNullOrWhiteSpace(updatedDriver.Phone))
                {
                    driver.Phone = updatedDriver.Phone;
                }
                if (!string.IsNullOrWhiteSpace(updatedDriver.LicenseNumber))
                {
                    driver.LicenseNumber = updatedDriver.LicenseNumber;
                }
                if (!string.IsNullOrWhiteSpace(updatedDriver.Status))
                {
                    driver.Status = updatedDriver.Status;
                }
                if (!string.IsNullOrWhiteSpace(updatedDriver.RegstrationComment))
                {
                    driver.RegstrationComment = updatedDriver.RegstrationComment;
                }
                if (!string.IsNullOrWhiteSpace(updatedDriver.LicenseClasses))
                {
                    driver.LicenseClasses = updatedDriver.LicenseClasses;
                }
                if (updatedDriver.LicenseExpireDate.HasValue)
                {
                    driver.LicenseExpireDate = updatedDriver.LicenseExpireDate.Value;
                }
                if (updatedDriver.isAvilableForBooking.HasValue)
                {
                    driver.isAvilableForBooking = updatedDriver.isAvilableForBooking.Value;
                }
                if (!string.IsNullOrWhiteSpace(updatedDriver.ImageUrl))
                {
                    driver.ImageUrl = updatedDriver.ImageUrl;
                }
                if (!string.IsNullOrWhiteSpace(updatedDriver.CompanyID))
                {
                    driver.CompanyID = updatedDriver.CompanyID;
                }

                // Save changes to the database
                await _context.SaveChangesAsync();

                // Return the updated driver information
                executionResult.SetData(driver);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(DriverController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion


        #region DeleteDriver
        [HttpDelete]
        [Route("DeleteDriver/{id}")]
        public async Task<IActionResult> DeleteDriver(string id)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(DeleteDriver);

            try
            {
                var driver = await _context.Drivers.FindAsync(id);

                if (driver == null)
                {
                    return NotFound("Driver not found");
                }

                _context.Drivers.Remove(driver);
                await _context.SaveChangesAsync();

                executionResult.SetData(driver);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(DriverController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion

        #region Assign TruckTypes To Driver
        [HttpPost("assignTruckTypesToDrive/{driverId}")]
        public async Task<IActionResult> AssignTruckTypes(string driverId, [FromBody] List<string> truckTypeIds)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(AssignTruckTypes);

            try
            {
                // Validate input
                if (truckTypeIds == null || !truckTypeIds.Any())
                {
                    return BadRequest(new { Message = "Truck type IDs are required." });
                }

                var driver = await _context.Drivers
                    .Include(d => d.TruckTypes) // Include current TruckTypes
                    .FirstOrDefaultAsync(d => d.DriverID == driverId);

                if (driver == null)
                {
                    return NotFound(new { Message = "Driver not found." });
                }

                // Fetch the TruckTypes by IDs
                var truckTypes = await _context.TruckTypes
                    .Where(t => truckTypeIds.Contains(t.TruckTypeID))
                    .ToListAsync();

                if (!truckTypes.Any())
                {
                    return BadRequest(new { Message = "No valid truck types found." });
                }

                // Remove existing truck types assigned to the driver
                driver.TruckTypes.Clear();

                // Assign new truck types to the driver
                driver.TruckTypes = truckTypes;

                // Update driver's status to ACTIVE
                driver.Status = "ACTIVE";

                await _context.SaveChangesAsync();

                executionResult.SetData(new { Message = "Truck types assigned and driver status updated to ACTIVE." });
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(DriverController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion


    }
}
