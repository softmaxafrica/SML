using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lsclient.Server.Models;
 using lsclient.Server.Shared;
using LogiSync.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace lsclient.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public ContractsController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        #region GetAllContracts
        [HttpGet]
        [Route("GetAllContracts")]
        public IActionResult GetAllContracts()
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(GetAllContracts);
            
            try
            {
                using (var db = new AppDbContext(_config))
                {
                    var Contracts = db.Contracts
                        .Include(j=>j.JobRequest)
                                      .ToList();
                    executionResult.SetData(Contracts);
                    return Ok(executionResult.GetServerResponse());
                }
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(ContractsController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion

 

        #region GetContractById
        [HttpGet]
        [Route("GetContractById/{ContractNumber}")]
        public IActionResult GetContractById(string ContractNumber)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(GetContractById);

            try
            {
                using (var db = new AppDbContext(_config))
                {
                    var Contract = db.Contracts
                                     .Include(j => j.JobRequest)
                                     .FirstOrDefault(i => i.ContractID == ContractNumber);
                    if (Contract == null)
                    {
                        return NotFound("Contract not found");
                    }
                    executionResult.SetData(Contract);
                    return Ok(executionResult.GetServerResponse());
                }
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(ContractsController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion
        #region CreateContract
        [HttpPost]
        [Route("CreateContract")]
        public async Task<ExecutionResult> CreateContract([FromBody] Contract newContract)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(CreateContract);

            try
            {
                // Check if a contract already exists for the given RequestID
                var existingContract = _context.Contracts.FirstOrDefault(i => i.RequestID == newContract.RequestID);

                if (existingContract != null)
                {
                    // If a contract already exists, skip adding and return the existing contract
                    //executionResult.Message("Contract already exists for the given RequestID. Skipping creation.");
                    executionResult.SetData(existingContract);
                    return executionResult;
                }

                // Add the new contract
                _context.Contracts.Add(newContract);
                await _context.SaveChangesAsync();

                executionResult.SetData(newContract);
                return executionResult;
            }
            catch (Exception ex)
            {
                // Handle exceptions and set appropriate error response
                executionResult.SetInternalServerError(nameof(ContractsController), functionName, ex);
                return executionResult;
            }
        }
        #endregion

        #region DeleteContract
        [HttpDelete]
        [Route("DeleteContract/{ContractNumber}")]
        public async Task<IActionResult> DeleteContract(string ContractNumber)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(DeleteContract);

            try
            {
                var Contract = await _context.Contracts.FindAsync(ContractNumber);
                if (Contract == null)
                {
                    return NotFound("Contract not found");
                }

                _context.Contracts.Remove(Contract);
                await _context.SaveChangesAsync();

                executionResult.SetData(Contract);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(ContractsController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion
    }
}
