using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lsclient.Server.Models;

using lsclient.Server.Shared;
using lsclient.Server.Models.DataPayloads;
using lsclient.Server.Controllers.SecurityUser;

namespace lsclient.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public CompanyController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
         #region CompanyInfo
        #region GetAllCompanies
        [HttpGet]
        [Route("GetAllCompanies")]
        public IActionResult GetAllCompanies()
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(GetAllCompanies);

            try
            {
                using (var db = new AppDbContext(_config))
                {
                    var companies = db.Companies.ToList();
                    executionResult.SetData(companies);
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


        #region GetCompanyById
        [HttpGet]
        [Route("GetCompanyById/{CompanyId}")]
        public IActionResult GetCompanyById(string CompanyId)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(GetCompanyById);

            try
            {
                using (var db = new AppDbContext(_config))
                {
                    var company = db.Companies.FirstOrDefault(c => c.CompanyID == CompanyId);
                    if (company == null)
                    {
                        return NotFound("Company not found");
                    }
                    executionResult.SetData(company);
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
        #region GetCompanyById
        [HttpGet]
        [Route("GetCompanyByTinNumber/{TinNumber}")]
        public IActionResult GetCompanyByTinNumber(string TinNumber)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(GetCompanyById);

            try
            {
                using (var db = new AppDbContext(_config))
                {
                    var company = db.Companies.FirstOrDefault(c => c.CompanyTinNumber == TinNumber);
                    if (company == null)
                    {
                        return NotFound("Company not found");
                    }
                    executionResult.SetData(company);
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
        #region RegisterCompany
        [HttpPost]
        [Route("RegisterCompany")]
        public async Task<IActionResult> RegisterCompany([FromBody] CompanyPayload newCompany)
        {
            var functionName = nameof(RegisterCompany);
            var executionResult = new ExecutionResult();

            Company data = new Company();
            try
            {
                data.CompanyID = Functions.GenerateCompanyId();
                data.CompanyName = newCompany.CompanyName;
                data.CompanyDescription = newCompany.CompanyDescription;
                data.CompanyTinNumber = newCompany.CompanyTinNumber;
                data.AdminEmail = newCompany.AdminEmail;
                data.AdminFullName = newCompany.AdminFullName;
                data.CompanyAddress = newCompany.CompanyAddress;
                data.AdminContact = newCompany.AdminContact;
                data.CompanyLatitude = newCompany.CompanyLatitude;
                data.CompanyLongitude = newCompany.CompanyLongitude;


                _context.Companies.Add(data);

                var secUser = new SecUser
                {
                    UserID = data.CompanyID,
                    Email = data.AdminEmail,
                    PasswordHash = Functions.HashPassword("defaultPassword@123"),
                    Role = "COMPANY",
                    Status = "ACTIVE"
                };

                _context.SecUsers.Add(secUser);
                await _context.SaveChangesAsync();

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
        

        #region UpdateCompany
        [HttpPut]
        [Route("UpdateCompany/{companyId}")]
        public IActionResult UpdateCompany(string companyId, [FromBody] Company updatedCompany)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(UpdateCompany);
            try
            {
                using (var db = new AppDbContext(_config))
                {
                    var existingCompany = db.Companies.FirstOrDefault(c => c.CompanyID == companyId);
                    if (existingCompany == null)
                    {
                        return NotFound("Company not found");
                    }

                    existingCompany.CompanyName = updatedCompany.CompanyName;
                    existingCompany.AdminFullName = updatedCompany.AdminFullName;
                    existingCompany.AdminEmail = updatedCompany.AdminEmail;
                    existingCompany.AdminContact = updatedCompany.AdminContact;
                    existingCompany.CompanyAddress = updatedCompany.CompanyAddress;
                    existingCompany.CompanyDescription = updatedCompany.CompanyDescription;
                    existingCompany.CompanyLatitude = updatedCompany.CompanyLatitude;
                    existingCompany.CompanyLongitude = updatedCompany.CompanyLongitude;

                    db.SaveChanges();

                    executionResult.SetData(existingCompany);
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

        #region DeleteCompany
        [HttpDelete]
        [Route("DeleteCompany/{companyId}")]
        public async Task<IActionResult> DeleteCompany(string companyId)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(DeleteCompany);

            try
            {
                var company = await _context.Companies.FindAsync(companyId);
                if (company == null)
                {
                    return NotFound("Company not found");
                }

                _context.Companies.Remove(company);

                var secUser = await _context.SecUsers.FindAsync(companyId);
                if (secUser != null)
                {
                    _context.SecUsers.Remove(secUser);
                }

                await _context.SaveChangesAsync();

                executionResult.SetData(company);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(CompanyController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion
        #endregion

        #region TruckTypesInfo

        #region GetAllTruckTypes
        [HttpGet]
        [Route("GetAllTruckTypes")]
        public IActionResult GetAllTruckTypes()
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(GetAllTruckTypes);

            try
            {
                var truckTypes = _context.TruckTypes.ToList();
                executionResult.SetData(truckTypes);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(CompanyController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion

        #region GetTruckTypeById
        [HttpGet]
        [Route("GetTruckTypeById/{id}")]
        public IActionResult GetTruckTypeById(string id)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(GetTruckTypeById);

            try
            {
                var truckType = _context.TruckTypes.FirstOrDefault(tt => tt.TruckTypeID == id);

                if (truckType == null)
                {
                    return NotFound("TruckType not found");
                }

                executionResult.SetData(truckType);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(CompanyController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion

        #region AddTruckType
        [HttpPost]
        [Route("AddTruckType")]
        public async Task<IActionResult> AddTruckType([FromBody] VehicleTypesPayload newTruckType)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(AddTruckType);

            try
            {
                // Generate TruckTypeID
                newTruckType.TruckTypeID = Functions.GenerateTruckTypeId();

                // Map payload to model
                var truckType = new TruckType
                {
                    TruckTypeID = newTruckType.TruckTypeID,
                    TypeName = newTruckType.TypeName,
                    Description = newTruckType.Description,
                    SampleImageUrl = newTruckType.SampleImageUrl
                };

                _context.TruckTypes.Add(truckType);
                await _context.SaveChangesAsync();

                executionResult.SetData(truckType);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(CompanyController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion

        #region UpdateTruckType
        [HttpPut]
        [Route("UpdateTruckType/{id}")]
        public async Task<IActionResult> UpdateTruckType(string id, [FromBody] VehicleTypesPayload updatedTruckType)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(UpdateTruckType);

            try
            {
                var truckType = await _context.TruckTypes.FindAsync(id);

                if (truckType == null)
                {
                    return NotFound("TruckType not found");
                }

                truckType.TypeName = updatedTruckType.TypeName;
                truckType.Description = updatedTruckType.Description;
                truckType.SampleImageUrl = updatedTruckType.SampleImageUrl;

                await _context.SaveChangesAsync();

                executionResult.SetData(truckType);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(CompanyController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion

        #region DeleteTruckType
        [HttpDelete]
        [Route("DeleteTruckType/{id}")]
        public async Task<IActionResult> DeleteTruckType(string id)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(DeleteTruckType);

            try
            {
                var truckType = await _context.TruckTypes.FindAsync(id);

                if (truckType == null)
                {
                    return NotFound("TruckType not found");
                }

                _context.TruckTypes.Remove(truckType);
                await _context.SaveChangesAsync();

                executionResult.SetData(truckType);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(CompanyController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion

        #endregion

        #region DriverInfo

        #region GetDriversAwaitingApprovalByCompanyId
        [HttpGet]
        [Route("GetDriversAwaitingApprovalByCompanyId/{CompanyId}")]
        public IActionResult GetDriversAwaitingApprovalByCompanyId(string CompanyId)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(GetDriversAwaitingApprovalByCompanyId);

            try
            {
                using (var db = new AppDbContext(_config))
                {
                    var drivers = db.Drivers
                                    .Include(d => d.Company)
                                    .Where(d => d.Status == "PENDING" && d.Company.CompanyID == CompanyId)
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

        #region GetDriversByStatus
        [HttpGet]
        [Route("GetDriversByStatus/{status}")]
        public IActionResult GetDriversByStatus(string status)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(GetDriversByStatus);

            try
            {
                using (var db = new AppDbContext(_config))
                {
                    var drivers = db.Drivers.Where(d => d.Status == status.ToUpper()).ToList();

                    if (!drivers.Any())
                    {
                        executionResult.SetNotFound(status);
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


        #region ApproveDriver
        [HttpPost]
        [Route("ApproveDriver")]
        public IActionResult ApproveDriver([FromBody] ApprovalPayload driverApproval)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(ApproveDriver);

            try
            {
                using (var db = new AppDbContext(_config))
                {
                    var driver = db.Drivers.FirstOrDefault(d => d.DriverID == driverApproval.UserID);

                    if (driver == null)
                    {
                        executionResult.SetNotFound(driverApproval.UserID);
                        return NotFound(executionResult.GetServerResponse());
                    }

                    // Approve the driver
                    
                    driver.Status = driverApproval.Status;
                    driver.RegstrationComment = driverApproval.RegstrationComment;

                    db.SaveChanges();
                    if (driverApproval.Status != "REJECTED")
                    {

                    
                     var newSecUser = new SecUser
                    {
                        UserID = driver.DriverID,
                        Email = driver.Email,
                        PasswordHash = Functions.HashPassword("defaultPassword@123"),
                        Role = "DRIVER",
                        Status = "ACTIVE"
                    };
                    db.SecUsers.Add(newSecUser);
                    db.SaveChanges();
                    }
                    executionResult.SetData(driver);
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


        #endregion
    }

}
