using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lsclient.Server.Models;
using System.Threading.Tasks;
using System.Linq;
using lsclient.Server.Shared;
using lsclient.Server.Models.DataPayloads;

namespace lsclient.Server.Controllers.Billing
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly AppDbContext _context; 
        private readonly IConfiguration _config;

        public PaymentController(AppDbContext context)
        {
            _context = context;
        }

        #region GetAllPayments
        [HttpGet]
        [Route("GetAllPayments")]
        public IActionResult GetAllPayments()
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(GetAllPayments);

            try
            {
                var payments = _context.Payments
                    //.Include(p => p.Invoice) // Include navigation property
                    .ToList();
                executionResult.SetData(payments);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(PaymentController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion

        #region GetPaymentById
        [HttpGet]
        [Route("GetPaymentById/{id}")]
        public IActionResult GetPaymentById(string id)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(GetPaymentById);

            try
            {
                var payment = _context.Payments
                    //.Include(p => p.Invoice) // Include navigation property
                    .FirstOrDefault(p => p.PaymentID == id);

                if (payment == null)
                {
                    return NotFound("Payment not found");
                }

                executionResult.SetData(payment);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(PaymentController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion

        #region GetPaymentByInvoiceNumber
        [HttpGet]
        [Route("GetPaymentByInvoiceNumber/{invoiceNumber}")]
        public IActionResult GetPaymentByInvoiceNumber(int invoiceNumber)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(GetPaymentById);

            try
            {
                var payments = _context.Payments
                    .Where(p => p.InvoiceNumber == invoiceNumber).ToList();

                if (payments == null)
                {
                    return NotFound("No Payment not found");
                }

                executionResult.SetData(payments);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(PaymentController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion


        #region GetCompanyPayments
        [HttpGet]
        [Route("GetCompanyPayments/{companyId}")]
        public IActionResult GetCompanyPayments(string companyId)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(GetCompanyPayments);

            try
            {
                var payment = _context.Payments
                    .Include(p => p.Invoice)
                    .Include(C => C.Invoice.CustomerDetails)
                     .Where(p => p.Invoice.CompanyID == companyId).ToList();

                if (payment == null)
                {
                    return NotFound("Sorry ! You Do Not have Any Payment Record");
                }

                executionResult.SetData(payment);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(PaymentController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion
        #region GetCustomerPayments
        [HttpGet]
        [Route("GetCustomerPayments/{CustomerId}")]
        public IActionResult GetCustomerPayments(string CustomerId)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(GetCustomerPayments);

            try
            {
                var payment = _context.Payments
                    .Include(p => p.Invoice)
                    .Include(C => C.Invoice.CustomerDetails)
                    .Include(C => C.Invoice.CompanyDetails)
                    .Where(p => p.Invoice.CustomerID == CustomerId).ToList();

                if (payment == null)
                {
                    return NotFound("Sorry ! You Do Not have Any Payment Record");
                }

                executionResult.SetData(payment);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(PaymentController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion
        #region AddPayment
        [HttpPost]
        [Route("AddPayment")]
        public async Task<IActionResult> AddPayment([FromBody] PaymentDto dtoPYMT)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(AddPayment);
            InvoiceController invController = new InvoiceController(_context, _config);

            Payment newPayment = new Payment();

            newPayment.AmountPaid = dtoPYMT.AmountPaid;
            newPayment.PaymentMethod = dtoPYMT.PaymentMethod;
            newPayment.PaymentDate = dtoPYMT.PaymentDate;
            newPayment.ReferenceNumber = dtoPYMT.ReferenceNumber;
            newPayment.Currency = dtoPYMT.ReferenceNumber;
            newPayment.InvoiceNumber = dtoPYMT.InvoiceNumber;
            newPayment.PaymentID = dtoPYMT.PaymentID;
            

            try
            {

                // Check if the invoice exists
                var invoiceToBePaid = await _context.Invoices
                    .FirstOrDefaultAsync(i => i.InvoiceNumber == newPayment.InvoiceNumber);
                if (invoiceToBePaid == null)
                {
                    return NotFound(executionResult.GetServerResponse());
                }

                newPayment.Invoice = invoiceToBePaid;
                // Check if the invoice's customer ID matches the one provided in the payment
                if (invoiceToBePaid.CustomerID != newPayment.Invoice.CustomerID)
                {
                    executionResult.SetValidationError("Customer ID does not match the invoice.");
                    return BadRequest(executionResult.GetServerResponse());
                }
                if (newPayment.AmountPaid <= 0)
                {
                    executionResult.SetValidationError("INVALID AMAOUNT \n The Amount Paid must be greater than zero.");
                    return BadRequest(executionResult.GetServerResponse());
                }

                if (newPayment.AmountPaid > invoiceToBePaid.OwedAmount)
                {
                    executionResult.SetValidationError("Invalid Amount.");
                    return BadRequest(executionResult.GetServerResponse());
                }

                // Generate PaymentID and add payment
                newPayment.PaymentID = Functions.GeneratePaymentId();
                await _context.Payments.AddAsync(newPayment);

                // Update invoice payment details
                await invController.UpdateInvoicePaymentDetails(newPayment.InvoiceNumber);

                // Save changes
                await _context.SaveChangesAsync();

                // Set success data and return
                executionResult.SetData(newPayment);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(PaymentController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse());
            }
        }
        #endregion
         

        #region UpdatePayment
        [HttpPut]
        [Route("UpdatePayment/{id}")]
        public async Task<IActionResult> UpdatePayment(string id, [FromBody] Payment updatedPayment)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(UpdatePayment);

            try
            {
                var payment = await _context.Payments.FindAsync(id);

                if (payment == null)
                {
                    return NotFound("Payment not found");
                }

                payment.InvoiceNumber = updatedPayment.InvoiceNumber;
                payment.AmountPaid = updatedPayment.AmountPaid;
                payment.PaymentDate = updatedPayment.PaymentDate;
                payment.PaymentMethod = updatedPayment.PaymentMethod;
                payment.ReferenceNumber = updatedPayment.ReferenceNumber;

                await _context.SaveChangesAsync();

                executionResult.SetData(payment);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(PaymentController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion

        #region DeletePayment
        [HttpDelete]
        [Route("DeletePayment/{id}")]
        public async Task<IActionResult> DeletePayment(string id)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(DeletePayment);
            InvoiceController invController = new InvoiceController(_context, _config);

            try
            {
                var payment = await _context.Payments.FindAsync(id);

                if (payment == null)
                {
                    return NotFound("Payment not found");
                }
                   await invController.RemovePaymentFromInvoice(payment.InvoiceNumber,payment.AmountPaid);

                _context.Payments.Remove(payment);
                await _context.SaveChangesAsync();

                //executionResult.SetData(payment);
                executionResult.SetSuccess("Payment Deleted Successfully");
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(PaymentController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion
    }
}
