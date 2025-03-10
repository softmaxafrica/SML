using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lsclient.Server.Models;
 
using System.Threading.Tasks;
using System.Linq;
using lsclient.Server.Shared;
using lsclient.Server.Models.DataPayloads;

namespace lsclient.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriceAgreementController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PriceAgreementController(AppDbContext context)
        {
            _context = context;
        }



        #region GetAllPriceAgreements
        [HttpGet]
        [Route("GetAllPriceAgreements")]
        public IActionResult GetAllPriceAgreements()
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(GetAllPriceAgreements);

            try
            {
                var priceAgreements = _context.PriceAgreements
                    //.Include(pa => pa.JobRequest) // Include navigation property
                    .ToList();
                executionResult.SetData(priceAgreements);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(PriceAgreementController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion

        #region GetPriceAgreementById
        [HttpGet]
        [Route("GetPriceAgreementById/{id}")]
        public IActionResult GetPriceAgreementById(string id)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(GetPriceAgreementById);

            try
            {
                var priceAgreement = _context.PriceAgreements
                    //.Include(pa => pa.JobRequest) // Include navigation property
                    .FirstOrDefault(pa => pa.PriceAgreementID == id);

                if (priceAgreement == null)
                {
                    return NotFound("Price Agreement not found");
                }

                executionResult.SetData(priceAgreement);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(PriceAgreementController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion

        #region AddPriceAgreement
        [HttpPost]
        [Route("AddPriceAgreement")]
        

        //#region InitialPriceDetails
        public async  Task<ExecutionResult> NewPriceAgreement(PriceAgreementPayload newPriceAgreement)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(NewPriceAgreement);
            RequestWithPayment priceDetails = new RequestWithPayment
            {
                PriceAgreementID = newPriceAgreement.PriceAgreementID,
                CompanyPrice = newPriceAgreement.CompanyPrice,
                AgreedPrice = newPriceAgreement.AgreedPrice,
                CustomerPrice= newPriceAgreement.CustomerPrice,
                CompanyID = newPriceAgreement.CompanyID,
                JobRequestID = newPriceAgreement.JobRequestID,
                CustomerID = newPriceAgreement.CustomerID
  
            };
            try
            {
                _context.PriceAgreements.Add(priceDetails);
                await _context.SaveChangesAsync();

                executionResult.SetData(newPriceAgreement);
                //return Ok(executionResult.GetServerResponse());
                return  executionResult;

            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(PriceAgreementController), functionName, ex);
                return executionResult;
            }
        }

        #endregion
        #region UpdatePriceAgreement
        [HttpPut]
        [Route("UpdatePriceAgreement/{id}")]
        public async Task<IActionResult> UpdatePriceAgreement(string id, [FromBody] RequestWithPayment updatedPriceAgreement)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(UpdatePriceAgreement);

            try
            {
                var priceAgreement = await _context.PriceAgreements.FindAsync(id);

                if (priceAgreement == null)
                {
                    return NotFound("Price Agreement not found");
                }

                priceAgreement.CompanyPrice = updatedPriceAgreement.CompanyPrice;
                priceAgreement.AgreedPrice = updatedPriceAgreement.AgreedPrice;
                //priceAgreement.JobRequestID = updatedPriceAgreement.JobRequestID;

                await _context.SaveChangesAsync();

                executionResult.SetData(priceAgreement);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(PriceAgreementController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion

        #region DeletePriceAgreement
        [HttpDelete]
        [Route("DeletePriceAgreement/{id}")]
        public async Task<IActionResult> DeletePriceAgreement(string id)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(DeletePriceAgreement);

            try
            {
                var priceAgreement = await _context.PriceAgreements.FindAsync(id);

                if (priceAgreement == null)
                {
                    return NotFound("Price Agreement not found");
                }

                _context.PriceAgreements.Remove(priceAgreement);
                await _context.SaveChangesAsync();

                executionResult.SetData(priceAgreement);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(PriceAgreementController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion
    }
}
