using lsclient.Server.Models;
using lsclient.Server.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

    namespace lsclient.Server.Controllers.Billing
{
    public class ChargableItemsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public ChargableItemsController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }


        #region GetAllChargableItemsToBill
        [HttpGet]
        [Route("GetAllChargableItemsToBill")]
         public List<ChargableItem> GetAllChargableItemsToBill()
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(GetAllChargableItemsToBill);
 
                using (var db = new AppDbContext(_config))
                {
                    return db.ChargableItems
                    .Where(i=>i.Status=="PENDING")
                    .Include(jr => jr.JobRequest)
                                      .ToList();
                }
            
             
        }
        #endregion

        #region AddChargableItem
        [HttpPost("AddChargableItem")]
        public async Task<IActionResult> AddChargableItem([FromBody] ChargableItemPayload payload)
        {
            if (payload == null)
            {
                return BadRequest(new { message = "Invalid payload data." });
            }

            try
            {
                // Map payload to ChargableItem entity
                var item = new ChargableItem
                {
                    JobRequestID = payload.JobRequestID,
                    PriceAgreementID = payload.PriceAgreementID,
                    Status = payload.Status,
                    CustomerID = payload.CustomerID,
                    Amount = payload.Amount,
                    ItemDescription=payload.ItemDescription,
                    IssueDate = DateTime.UtcNow.ToLocalTime(),
                    InvoiceNumber = null // InvoiceNumber will be null by default
                };

                await _context.ChargableItems.AddAsync(item);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Chargable item added successfully.",
                    item
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while adding the item.",
                    error = ex.Message
                });
            }
        }
        #endregion

        #region UpdateChargableItemsStatusByJobRequest
        [HttpPut("UpdateChargableItemsStatus/{jobRequestId}")]
        public async Task<IActionResult> UpdateChargableItemsStatusAndInvoiceNumber(string jobRequestId,string Invoicestatus,int InvoiceNumber)
        {
            try
            {
                // Fetch all items with the given JobRequestID and status "PENDING"
                var itemsToUpdate = await _context.ChargableItems
                    .Where(item => item.JobRequestID == jobRequestId)
                    .ToListAsync();

                if (!itemsToUpdate.Any())
                {
                    return NotFound($"No chargeable items found for JobRequestID {jobRequestId} with status 'PENDING'.");
                }

                // Update the status 
                foreach (var item in itemsToUpdate)
                {
                    item.Status = Invoicestatus;
                    item.InvoiceNumber = InvoiceNumber;
                }

                await _context.SaveChangesAsync();

                return Ok($"Chargeable items for JobRequestID {jobRequestId} have been updated to 'DRAFT'.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
        #endregion

    }
}
