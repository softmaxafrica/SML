using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lsclient.Server.Models; 
using lsclient.Server.Shared;
using lsclient.Server.Models.DataPayloads;
using System.Data.Common;
using System.Transactions;
using System.ComponentModel.Design;
using LogiSync.Models;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using Microsoft.IdentityModel.Tokens;

namespace lsclient.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobRequestController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        public DateTime SysDate = DateTime.Now.ToLocalTime();

        public JobRequestController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        #region GetAllJobRequests
        [HttpGet]
        [Route("GetAllJobRequests")]
        public IActionResult GetAllJobRequests()
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(GetAllJobRequests);

            try
            {
                using (var db = new AppDbContext(_config))
                {
                    var jobRequests = db.JobRequests
                        .Include(jr => jr.Negotiations)
                        .Include(jr => jr.PriceAgreement)
                        .Include(jr => jr.Truck)
                        .Include(jr => jr.Customer)
                        .Include(jr=>jr.InvoiceDetails)
                        .Where(jr => jr.Status !="CANCELLED")
                        .ToList();

                    foreach (var job in jobRequests)
                    {
                        var priceAgreement = db.PriceAgreements
                            .Where(pa =>   pa.JobRequestID == job.JobRequestID).ToList();


                        job.Negotiations = priceAgreement;

                    }

                    executionResult.SetData(jobRequests);
                    return Ok(executionResult.GetServerResponse());
                }
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(JobRequestController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion

        #region GetJobRequestById
        [HttpGet]
        [Route("GetJobRequestById/{jobRequestID}")]
        public IActionResult GetJobRequestById(string jobRequestID)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(GetJobRequestById);

            try
            {
                using (var db = new AppDbContext(_config))
                {
                    var jobRequest = db.JobRequests
                        .Include(jr => jr.PriceAgreement)
                        .Include(jr => jr.Truck)
                        .Include(jr => jr.Customer)
                        .Include(jr => jr.InvoiceDetails)
                         .Where(jr => jr.Status != "CANCELLED")
                        .FirstOrDefault(jr => jr.JobRequestID == jobRequestID);

                   
                        var priceAgreement = db.PriceAgreements
                            .Where(pa =>  pa.JobRequestID == jobRequest.JobRequestID).ToList();
                            jobRequest.Negotiations = priceAgreement;
 
                    if (jobRequest == null)
                    {
                        return NotFound("Job Request not found");
                    }
                    executionResult.SetData(jobRequest);
                    return Ok(executionResult.GetServerResponse());
                }
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(JobRequestController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion

        #region GetCompanyJobRequest
        [HttpGet]
        [Route("GetCompanyJobRequest/{CompanyID}")]
        public IActionResult GetCompanyJobRequest(string CompanyID)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(GetCompanyJobRequest);
            try
            {
                using (var db = new AppDbContext(_config))
                {
                    // Fetch all job requests that are relevant to the company
                    var jobRequests = db.JobRequests
                        .Include(jr => jr.PriceAgreement)
                        .Include(jr => jr.Negotiations)
                        .Include(jr => jr.Truck)
                        .Include(jr => jr.Customer)
                        .Include(jr => jr.InvoiceDetails)
                        .Where(jr =>
                            (jr.Status == "CREATED") || (jr.Status == "ON AGREEMENT") ||
                            jr.Status != "CANCELLED" &&
                            //(jr.Status != "READY FOR INVOICE" || 
                            (((jr.Status == "READY FOR INVOICE") || (jr.Status == "INVOICE PAID READY TO SERVE") || (jr.Status == "INVOICE PARTIAL READY TO SERVE") || (jr.Status == "ONGOING INVOICE GENERATION")|| (jr.Status == "READY TO SERVE") || (jr.Status == "TRUCK DRIVER ASSIGNMENT") || (jr.Status== "INCOMPLETE ADVANCE PAYMENT") && jr.AssignedCompany == CompanyID)))
                        .ToList();

                    // Check if any job requests exist
                    if (jobRequests == null || jobRequests.Count == 0)
                    {
                        return NotFound("No Job Requests Available For This Company");
                    }


                    // Map PriceAgreement specific to the CompanyID and JobRequestID
                    foreach (var job in jobRequests)
                    {
                        var negotiationPrices= db.PriceAgreements
                            .Where(pa => pa.CompanyID == CompanyID&& pa.JobRequestID == job.JobRequestID).ToList();

                        var priceAgreement = db.PriceAgreements
                        .FirstOrDefault(pa => pa.CompanyID == CompanyID && pa.JobRequestID == job.JobRequestID);

                        // If no matching PriceAgreement exists, assign a default instance
                        if (priceAgreement == null)
                        {
                            priceAgreement = new RequestWithPayment
                            {
                                PriceAgreementID = Functions.GeneratePriceAgreementId(),
                                CompanyID = CompanyID,
                                JobRequestID = job.JobRequestID,
                                CustomerPrice = 0,
                                CompanyPrice = 0,
                                AgreedPrice = 0
                            };
                        }

                        job.PriceAgreement = priceAgreement;
                        job.Negotiations = negotiationPrices;

                    }
                
                     
                    // Return the processed list of job requests
                    executionResult.SetData(jobRequests);
                    return Ok(executionResult.GetServerResponse());
                }
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(JobRequestController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion


       


        #region GetCustomerJobRequest
        [HttpGet]
        [Route("GetCustomerJobRequest/{CustomerID}")]
        public IActionResult GetCustomerJobRequest(string CustomerID)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(GetCustomerJobRequest);
            try
            {
                using (var db = new AppDbContext(_config))
                {
                    // Fetch all job requests that are relevant to the company
                    var jobRequests = db.JobRequests
              .Include(jr => jr.PriceAgreement)
              .Include(jr => jr.Negotiations)
              .Include(jr => jr.Truck)
              .Include(jr => jr.Customer)
              .Include(jr => jr.InvoiceDetails)
              .Where(jr =>
                  jr.CustomerID == CustomerID &&
                  jr.Status != "CANCELLED" &&
                  (jr.Status == "CREATED" ||
                   jr.Status == "ON AGREEMENT" ||
                   jr.Status == "READY FOR INVOICE" ||
                   jr.Status == "ONGOING INVOICE GENERATION" ||
                   jr.Status == "INVOICE PAID READY TO SERVE" ||
                   jr.Status == "INVOICE PARTIAL READY TO SERVE" ||
                   jr.Status == "READY TO SERVE" ||
                   jr.Status == "TRUCK DRIVER ASSIGNMENT" ||
                   jr.Status == "INCOMPLETE ADVANCE PAYMENT"))
              .OrderByDescending(jr => jr.Status == "ON AGREEMENT") // Prioritize "ON AGREEMENT"
              .ThenBy(jr => jr.Cdate) // Sort by creation date
              .ToList();

                    // Check if any job requests exist
                    if (jobRequests == null || jobRequests.Count == 0)
                    {
                        return NotFound("No Job Requests Available For This Company");
                    }


                    // Map PriceAgreement specific to the CompanyID and JobRequestID
                    foreach (var job in jobRequests)
                    {
                        var priceAgreement = db.PriceAgreements
                            .Where(pa => pa.CustomerID == CustomerID && pa.JobRequestID == job.JobRequestID).ToList();
 

                        job.Negotiations=priceAgreement;
                  
                    }

                    // Return the processed list of job requests
                    executionResult.SetData(jobRequests);
                    return Ok(executionResult.GetServerResponse());
                }
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(JobRequestController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion

        #region CreateJobRequest
        [HttpPost]
        [Route("CreateJobRequest")]

        public async Task<IActionResult> CreateJobRequest([FromBody] RequestWithPaymentPayload newJobRequest)
        {

            var executionResult = new ExecutionResult();
            string functionName = nameof(CreateJobRequest);
          
            JobRequest payload = new JobRequest
            {
                JobRequestID = Functions.GenerateJobRequestId()
            };

            _context.JobRequests.Add(payload);
            await _context.SaveChangesAsync();

          
            using (var transaction = _context.Database.BeginTransaction()) // Start transaction
            {
                try
                {
               

                // Convert empty string fields to null for nullable fields
                payload.PickupLocation = string.IsNullOrEmpty(newJobRequest.PickupLocation) ? null : newJobRequest.PickupLocation;
                payload.DeliveryLocation = string.IsNullOrEmpty(newJobRequest.DeliveryLocation) ? null : newJobRequest.DeliveryLocation;
                payload.CargoDescription = string.IsNullOrEmpty(newJobRequest.CargoDescription) ? null : newJobRequest.CargoDescription;
                payload.ContainerNumber = string.IsNullOrEmpty(newJobRequest.ContainerNumber) ? null : newJobRequest.ContainerNumber;
                payload.Status = string.IsNullOrEmpty(newJobRequest.Status) ? "CREATED" : newJobRequest.Status;
                payload.TruckType = string.IsNullOrEmpty(newJobRequest.TruckType) ? null : newJobRequest.TruckType;
                payload.TruckID = string.IsNullOrEmpty(newJobRequest.TruckID) ? null : newJobRequest.TruckID;
                payload.DriverID = string.IsNullOrEmpty(newJobRequest.DriverID) ? null : newJobRequest.DriverID;
                payload.RequestType = string.IsNullOrEmpty(newJobRequest.RequestType) ? null : newJobRequest.RequestType;

                payload.CustomerID = string.IsNullOrEmpty(newJobRequest.CustomerID) ? "NOT SET" : newJobRequest.CustomerID;

               
                payload.Cdate = SysDate;


                    // Create PriceAgreement object
                    PriceAgreementController PriceCntrl = new PriceAgreementController(_context);
                    PriceAgreementPayload priceDetails = new PriceAgreementPayload
                    {
                        PriceAgreementID = Functions.GeneratePriceAgreementId(),
                        CompanyPrice = newJobRequest.RequestedPrice,
                        AgreedPrice = newJobRequest.AcceptedPrice,
                        CustomerPrice = newJobRequest.CustomerPrice,
                        CompanyID = newJobRequest.CompanyID,
                        JobRequestID = payload.JobRequestID,
                        CustomerID = newJobRequest.CustomerID
                    };
                    var priceInsertResult = await PriceCntrl.NewPriceAgreement(priceDetails);
                    var priceDataResult = priceInsertResult.GetData();

                     // Set PriceAgreementID
                     payload.PriceAgreementID = priceDetails.PriceAgreementID;

                    // Save the new JobRequest
                    //_context.JobRequests.Add(payload);
                    await _context.SaveChangesAsync();

                    transaction.Commit();
                    executionResult.SetData(payload);
                    return Ok(executionResult.GetServerResponse());
                }
                catch (Exception ex)
                {
                    executionResult.SetInternalServerError(nameof(JobRequestController), functionName, ex);
                    return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
                }
            }
        }

        #endregion

        #region UpdateJobRequest
        [HttpPut]
        [Route("UpdateJobRequest/{jobRequestID}")]
        public async Task<IActionResult> UpdateJobRequestAsync(string jobRequestID, [FromBody] RequestWithPaymentPayload updatedJobRequest)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(UpdateJobRequestAsync);
            ContractsController contractContr = new ContractsController(_context, _config);



            try
            {
                using (var db = new AppDbContext(_config))
                {
                    using (var transaction = db.Database.BeginTransaction()) // Start transaction
                    {
                        var existingJobRequest = db.JobRequests.FirstOrDefault(jr => jr.JobRequestID == jobRequestID);
                        if (existingJobRequest == null)
                        {
                            return NotFound("Job Request not found");
                        }
                     

                        if (((existingJobRequest.Status == "READY TO SERVE") || (existingJobRequest.Status == "ONGOING INVOICE GENERATION") || (existingJobRequest.Status== "TRUCK DRIVER ASSIGNMENT")) && (!updatedJobRequest.TruckID.IsNullOrEmpty() || !updatedJobRequest.DriverID.IsNullOrEmpty()))
                        //if ((!updatedJobRequest.TruckID.IsNullOrEmpty() || !updatedJobRequest.DriverID.IsNullOrEmpty()))
                        {
                            var existingInvoice = db.Invoices.FirstOrDefault(j => j.JobRequestID == updatedJobRequest.JobRequestID);
                            existingJobRequest.TruckID = updatedJobRequest.TruckID;
                            existingJobRequest.DriverID = updatedJobRequest.DriverID;

                            if (existingInvoice != null)
                            {
                                existingJobRequest.Status = "INVOICE "+existingInvoice.Status+" READY TO SERVE";
                            }
                            else
                            existingJobRequest.Status = "READY TO SERVE";
                        }
                        else
                        {

                            #region Updating Existing JobRequest 
                            // Update JobRequest fields if provided
                            existingJobRequest.PickupLocation = string.IsNullOrEmpty(updatedJobRequest.PickupLocation) ? existingJobRequest.PickupLocation : updatedJobRequest.PickupLocation;
                            existingJobRequest.DeliveryLocation = string.IsNullOrEmpty(updatedJobRequest.DeliveryLocation) ? existingJobRequest.DeliveryLocation : updatedJobRequest.DeliveryLocation;
                            existingJobRequest.CargoDescription = string.IsNullOrEmpty(updatedJobRequest.CargoDescription) ? existingJobRequest.CargoDescription : updatedJobRequest.CargoDescription;
                            existingJobRequest.ContainerNumber = string.IsNullOrEmpty(updatedJobRequest.ContainerNumber) ? existingJobRequest.ContainerNumber : updatedJobRequest.ContainerNumber;


                            if (updatedJobRequest.CompanyAdvanceAmountRequred != null && updatedJobRequest.CompanyAdvanceAmountRequred > 0)
                                existingJobRequest.CompanyAdvanceAmountRequred = updatedJobRequest.CompanyAdvanceAmountRequred.Value;

                            if (updatedJobRequest.FirstDepositAmount.HasValue && updatedJobRequest.FirstDepositAmount != existingJobRequest.FirstDepositAmount)
                                existingJobRequest.FirstDepositAmount = updatedJobRequest.FirstDepositAmount.Value;

                            if ((updatedJobRequest.RequestedPrice > 0) && (updatedJobRequest.AcceptedPrice == 0 || updatedJobRequest.AcceptedPrice == null))
                            {
                                updatedJobRequest.Status = "ON AGREEMENT";
                            }

                            //Check if customer first Deposit Not Supplied But company Already Agree on price then set first deposit to 30 Percent of AgreedPRICE
                            //if ((updatedJobRequest.AcceptedPrice > 0 || existingJobRequest.PriceAgreementID != null) && updatedJobRequest.CompanyAdvanceAmountRequred > 0 && (updatedJobRequest.CompanyAdvanceAmountRequred <= updatedJobRequest.FirstDepositAmount || existingJobRequest.FirstDepositAmount >= updatedJobRequest.CompanyAdvanceAmountRequred))
                            //{
                            //    if (existingJobRequest.InvoiceNumber == null)
                            //    {
                            //        updatedJobRequest.Status = "READY FOR INVOICE";
                            //    }
                            //    existingJobRequest.AssignedCompany = updatedJobRequest.CompanyID;
                            //    existingJobRequest.PriceAgreementID = updatedJobRequest.PriceAgreementID;
                            //}

                            //update The Request Status if all agreement done( Price and (Temporary Removed FirstDeposit Checking)  / after assigned price agreement id)
                            //if ((updatedJobRequest.AcceptedPrice > 0) && (updatedJobRequest.CompanyAdvanceAmountRequred > 0) || (existingJobRequest.CompanyAdvanceAmountRequred > 0))
                            if ((updatedJobRequest.AcceptedPrice > 0) && (updatedJobRequest.CompanyAdvanceAmountRequred > 0 || existingJobRequest.CompanyAdvanceAmountRequred > 0))

                            {
                                if (existingJobRequest.InvoiceNumber == null)
                                {
                                    updatedJobRequest.Status = "READY FOR INVOICE";
                                }
                                existingJobRequest.AssignedCompany = updatedJobRequest.CompanyID;
                                existingJobRequest.PriceAgreementID = updatedJobRequest.PriceAgreementID;
                            }

                            existingJobRequest.Status = string.IsNullOrEmpty(updatedJobRequest.Status) ? existingJobRequest.Status : updatedJobRequest.Status;


                            existingJobRequest.TruckID = string.IsNullOrEmpty(updatedJobRequest.TruckID) ? existingJobRequest.TruckID : updatedJobRequest.TruckID;
                            existingJobRequest.DriverID = string.IsNullOrEmpty(updatedJobRequest.DriverID) ? existingJobRequest.DriverID : updatedJobRequest.DriverID;

                            existingJobRequest.CustomerID = string.IsNullOrEmpty(updatedJobRequest.CustomerID) ? existingJobRequest.CustomerID : updatedJobRequest.CustomerID;


                            existingJobRequest.Udate = SysDate;

                            #endregion

                            var existingPriceAgreement = db.PriceAgreements.FirstOrDefault(pa =>
                                                         (pa.CompanyID == updatedJobRequest.CompanyID || pa.CustomerID == updatedJobRequest.CustomerID)
                                                         && pa.JobRequestID == updatedJobRequest.JobRequestID);

                            if (existingPriceAgreement == null)
                            {
                                PriceAgreementController PriceCntrl = new PriceAgreementController(_context);

                                PriceAgreementPayload priceDetails = new PriceAgreementPayload
                                {
                                    PriceAgreementID = Functions.GeneratePriceAgreementId(),
                                    CompanyPrice = updatedJobRequest.RequestedPrice,
                                    AgreedPrice = updatedJobRequest.AcceptedPrice,
                                    CustomerPrice = updatedJobRequest.CustomerPrice,
                                    JobRequestID = updatedJobRequest.JobRequestID,
                                    CustomerID = updatedJobRequest.CustomerID,
                                    CompanyID = updatedJobRequest.CompanyID,
                                };
                                var priceInsertResult = await PriceCntrl.NewPriceAgreement(priceDetails);

                                var CreatedPriceAgreement = db.PriceAgreements.FirstOrDefault(pa => pa.PriceAgreementID == priceDetails.PriceAgreementID);

                                CreatedPriceAgreement.CompanyPrice = updatedJobRequest.RequestedPrice;
                                CreatedPriceAgreement.AgreedPrice = updatedJobRequest.AcceptedPrice;
                                CreatedPriceAgreement.CustomerPrice = updatedJobRequest.CustomerPrice;
                                CreatedPriceAgreement.JobRequestID = updatedJobRequest.JobRequestID;
                                CreatedPriceAgreement.CustomerID = updatedJobRequest.CustomerID;
                                CreatedPriceAgreement.CompanyID = updatedJobRequest.CompanyID;

                            }
                            // Find the associated PriceAgreement using the PriceAgreementID
                            else
                            {
                                existingJobRequest.PriceAgreementID = existingPriceAgreement.PriceAgreementID;
                                // Update PriceAgreement fields if provided
                                if (updatedJobRequest.RequestedPrice.HasValue && updatedJobRequest.RequestedPrice > 0)
                                    existingPriceAgreement.CompanyPrice = updatedJobRequest.RequestedPrice.Value;
                                if (updatedJobRequest.AcceptedPrice.HasValue && updatedJobRequest.AcceptedPrice > 0)
                                {

                                    existingPriceAgreement.AgreedPrice = updatedJobRequest.AcceptedPrice.Value;

                                    var existingContract = await _context.Contracts.FirstOrDefaultAsync(c => c.RequestID == updatedJobRequest.JobRequestID);
                                    if (existingContract == null)
                                    {
                                        Contract newContr = new Contract();
                                        newContr.RequestID = updatedJobRequest.JobRequestID;
                                        newContr.CompanyID = updatedJobRequest.CompanyID;
                                        newContr.CustomerID = updatedJobRequest.CustomerID;
                                        newContr.AdvancePayment = updatedJobRequest.FirstDepositAmount;
                                        newContr.ContractDate = DateTime.UtcNow.ToLocalTime();
                                        newContr.AgreedPrice = updatedJobRequest.AcceptedPrice;
                                        newContr.ContractID = Functions.GenerateContractId();

                                        await contractContr.CreateContract(newContr);
                                        existingJobRequest.ContractId = newContr.ContractID;

                                    }
                                    else
                                    {
                                        // Update the existing contract's details
                                        existingContract.CompanyID = updatedJobRequest.CompanyID;
                                        //existingContract.CustomerID = updatedJobRequest.CustomerID;
                                        //existingContract.AdvancePayment = updatedJobRequest.FirstDepositAmount;
                                        existingContract.ContractDate = DateTime.UtcNow.ToLocalTime();
                                        existingContract.AgreedPrice = updatedJobRequest.AcceptedPrice.Value;

                                        // Save changes to the updated contract
                                        _context.Contracts.Update(existingContract);
                                        //  await _context.SaveChangesAsync();

                                        // Update the contract ID in the job request if necessary
                                        existingJobRequest.ContractId = existingContract.ContractID;

                                        Console.WriteLine($"Updated existing contract for JobRequestID: {updatedJobRequest.JobRequestID}");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"Contract already exists for JobRequestID: {updatedJobRequest.JobRequestID}");
                                }

                                //if (updatedJobRequest.CustomerPrice.HasValue && updatedJobRequest.CustomerPrice > 0)
                                //    existingPriceAgreement.CustomerPrice = updatedJobRequest.CustomerPrice.Value;
                                //existingPriceAgreement.CompanyID = updatedJobRequest.CompanyID;
                                //existingPriceAgreement.JobRequestID = updatedJobRequest.JobRequestID;
                                //existingPriceAgreement.CustomerID = updatedJobRequest.CustomerID;

                                //if ((((updatedJobRequest.AcceptedPrice > 0) || existingJobRequest.PriceAgreement.AgreedPrice > 0) && ((updatedJobRequest.FirstDepositAmount == 0) || (existingJobRequest.FirstDepositAmount) == 0)))

                                if (existingPriceAgreement != null)
                                {
                                    if (updatedJobRequest.CustomerPrice.HasValue && updatedJobRequest.CustomerPrice > 0)
                                        existingPriceAgreement.CustomerPrice = updatedJobRequest.CustomerPrice.Value;

                                    existingPriceAgreement.CompanyID = updatedJobRequest.CompanyID;
                                    existingPriceAgreement.JobRequestID = updatedJobRequest.JobRequestID;
                                    existingPriceAgreement.CustomerID = updatedJobRequest.CustomerID;
                                }

                                if (existingJobRequest?.PriceAgreement != null &&
                                    ((updatedJobRequest.AcceptedPrice > 0 || existingJobRequest.PriceAgreement.AgreedPrice > 0) &&
                                    (updatedJobRequest.FirstDepositAmount == 0 || existingJobRequest.FirstDepositAmount == 0)))
                               {
                                    if (updatedJobRequest.AcceptedPrice < 1)
                                    {
                                        existingJobRequest.FirstDepositAmount = existingJobRequest.PriceAgreement.AgreedPrice * 0.3;
                                    }
                                    else { existingJobRequest.FirstDepositAmount = updatedJobRequest.AcceptedPrice * 0.3; }
                                }
                                else if (updatedJobRequest.FirstDepositAmount.HasValue && (updatedJobRequest.FirstDepositAmount > 0))
                                {
                                    existingJobRequest.FirstDepositAmount = updatedJobRequest.FirstDepositAmount;
                                }
                            }


                        }
                        // Save changes to both JobRequest and PriceAgreement
                        db.SaveChanges();
                        transaction.Commit();
                        executionResult.SetData(existingJobRequest);
                        return Ok(executionResult.GetServerResponse());
                    }
                }
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(JobRequestController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion
         

        #region DeleteJobRequest
        [HttpDelete]
        [Route("DeleteJobRequest/{jobRequestID}")]
        public async Task<IActionResult> DeleteJobRequest(string jobRequestID)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(DeleteJobRequest);
            InvoiceController invController = new InvoiceController(_context, _config);

            try
            {
 
                var jobRequest = await _context.JobRequests.FindAsync(jobRequestID);
                if (jobRequest == null)
                {
                    return NotFound("Job Request not found");
                }
                // Check if the invoice exists

                var invoiceOnRequest = await _context.Invoices
                      .FirstOrDefaultAsync(i => i.InvoiceNumber == jobRequest.InvoiceNumber);

                if (invoiceOnRequest != null)
                {
                    if (invoiceOnRequest.Status != "PAID" && invoiceOnRequest.Status != "PARTIAL")
                    {
                        invoiceOnRequest.Status = "DISCARDED";
                    }
                }

                //_context.JobRequests.Remove(jobRequest);
                jobRequest.Status = "CANCELLED";

                await _context.SaveChangesAsync();

                executionResult.SetData(jobRequest);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(JobRequestController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion

        #region BillingRequests

        #region GetPendingChargableItems
        [HttpGet]  // Explicit HTTP method binding
        [Route("GetPendingChargableItems")]
        public List<JobRequest> GetPendingChargableItems()
        {
            using (var db = new AppDbContext(_config))
            {
                return db.JobRequests
                    .Include(jr => jr.PriceAgreement)
                    .Include(jr => jr.Truck)
                    .Include(jr => jr.Customer)
                    .Where(jr => jr.Status == "READY FOR INVOICE")
                    .ToList();
            }
        }
        #endregion


        #region UpdateRequestStatus
        [HttpPut]
        [Route("UpdateRequestStatus/{jobRequestID}")]
        public async Task<IActionResult> UpdateRequestStatus(string jobRequestID, string newStatus,int? InvoiceNumber)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(UpdateRequestStatus);

            try
            {
                using (var db = new AppDbContext(_config))
                {
                    var existingJobRequest = db.JobRequests.FirstOrDefault(jr => jr.JobRequestID == jobRequestID);
                    if (existingJobRequest == null)
                    {
                        return NotFound("Job Request not found");
                    }
                   

                    #region Updating Existing JobRequest
                    existingJobRequest.Status = newStatus;
                    existingJobRequest.InvoiceNumber = InvoiceNumber;
                    existingJobRequest.Udate = DateTime.UtcNow.ToLocalTime();
                    #endregion

                    // Save changes to JobRequest
                    db.SaveChanges();

                    executionResult.SetData(existingJobRequest);
                    return Ok(executionResult.GetServerResponse());
                }
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(JobRequestController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion
        #endregion
    }
}
