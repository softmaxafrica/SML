using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using lsclient.Server.Models;
 
using System.Threading.Tasks;
using System.Linq;
using lsclient.Server.Shared;
using lsclient.Server.Controllers.SecurityUser;
using lsclient.Server.Models.DataPayloads;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic;
using System.Net;
using System.Numerics;

namespace lsclient.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CustomerController(AppDbContext context)
        {
            _context = context;
        }
        //#region RegisterCustomer
        //[HttpPost]
        //[Route("RegisterCustomer")]
        //public async Task<IActionResult> RegisterCustomer([FromBody] CustomerPayload customerPayload)
        //{
        //    var functionName = nameof(RegisterCustomer);
        //    var executionResult = new ExecutionResult();
        //    var payload = new Customer();  

        //    try
        //    {
        //        // Setting the customer properties from the payload
        //        payload.CustomerID = Functions.GenerateCustomerId(); 
        //        payload.FullName = string.IsNullOrEmpty(customerPayload.FullName) ? null : customerPayload.FullName;
        //        payload.Email = string.IsNullOrEmpty(customerPayload.Email) ? null : customerPayload.Email;
        //        payload.Phone = string.IsNullOrEmpty(customerPayload.Phone) ? null : customerPayload.Phone;
        //        payload.Address = string.IsNullOrEmpty(customerPayload.Address) ? null : customerPayload.Address;
        //        payload.PaymentMethod = string.IsNullOrEmpty(customerPayload.PaymentMethod) ? null : customerPayload.PaymentMethod;
        //        payload.CardNumber = string.IsNullOrEmpty(customerPayload.CardNumber) ? null : customerPayload.CardNumber;
        //        payload.CardType = string.IsNullOrEmpty(customerPayload.CardType) ? null : customerPayload.CardType;
        //        payload.BillingAddress = string.IsNullOrEmpty(customerPayload.BillingAddress) ? null : customerPayload.BillingAddress;
        //        payload.ExpiryDate = string.IsNullOrEmpty(customerPayload.ExpiryDate) ? null : customerPayload.ExpiryDate;
        //        payload.BankName = string.IsNullOrEmpty(customerPayload.BankName) ? null : customerPayload.BankName;
        //        payload.BankAccountNumber = string.IsNullOrEmpty(customerPayload.BankAccountNumber) ? null : customerPayload.BankAccountNumber;
        //        payload.BankAccountHolder = string.IsNullOrEmpty(customerPayload.BankAccountHolder) ? null : customerPayload.BankAccountHolder;
        //        payload.MobileNetwork = string.IsNullOrEmpty(customerPayload.MobileNetwork) ? null : customerPayload.MobileNetwork;
        //        payload.MobileNumber = string.IsNullOrEmpty(customerPayload.MobileNumber) ? null : customerPayload.MobileNumber;

        //         _context.Customers.Add(payload);


        //        foreach (var companyId in customerPayload.Companies)
        //        {
        //            var companyCustomer = new CompanyCustomer
        //            {
        //                CustomerId = payload.CustomerID,
        //                CompanyId = companyId,
        //                CreationDate = DateTime.UtcNow.ToLocalTime(),
        //            };

        //            _context.CompanyCustomers.Add(companyCustomer);
        //        }

        //        // Create the SecUser for the new customer
        //        var secUser = new SecUser
        //        {
        //            UserID = payload.CustomerID, // Use payload for the UserID
        //            Email = payload.Email, // Use payload for the email
        //            PasswordHash = Functions.HashPassword("defaultPassword@123"), // Default password
        //            Role = "CUSTOMER",
        //            Status = "ACTIVE"
        //        };

        //        // Add SecUser to the context
        //        _context.SecUsers.Add(secUser);
        //        await _context.SaveChangesAsync();

        //        // Send the default password to the user via email  (pending service logic to implement)

        //        // Return success with customer data
        //        executionResult.SetData(payload);
        //        return Ok(executionResult.GetServerResponse());
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle error
        //        executionResult.SetInternalServerError(nameof(CompanyController), functionName, ex);
        //        return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
        //    }
        //}
        //#endregion

        #region RegisterCustomer
        [HttpPost]
        [Route("RegisterCustomer")]
        public async Task<IActionResult> RegisterCustomer([FromForm] CustomerRegistrationModel customerRegistration)
        {
            var functionName = nameof(RegisterCustomer);
            var executionResult = new ExecutionResult();
            var payload = new Customer();

            try
            {
                // Validate password and confirm password
                if (customerRegistration.Password != customerRegistration.ConfirmPassword)
                {
                    executionResult.SetBadRequestError("Password and Confirm Password do not match.");
                    return BadRequest(executionResult.GetServerResponse());
                }

                // Save the profile image to the directory
                string profileImagePath = null;
                if (customerRegistration.ProfileImage != null)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "images", "customers");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + customerRegistration.ProfileImage.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await customerRegistration.ProfileImage.CopyToAsync(fileStream);
                    }

                    profileImagePath = Path.Combine("assets", "images", "customers_profile", uniqueFileName);
                }

                // Map the CustomerRegistrationModel to the Customer entity
                payload.CustomerID = Functions.GenerateCustomerId();
                payload.FullName = customerRegistration.FullName;
                payload.Email = customerRegistration.Email;
                payload.Phone = customerRegistration.Phone;
                payload.Address = customerRegistration.Address;
                payload.PaymentMethod = customerRegistration.PaymentMethod;
                payload.CardNumber = customerRegistration.CardNumber;
                payload.CardType = customerRegistration.CardType;
                payload.BillingAddress = customerRegistration.BillingAddress;
                payload.ExpiryDate = customerRegistration.ExpiryDate;
                payload.BankName = customerRegistration.BankName;
                payload.BankAccountNumber = customerRegistration.BankAccountNumber;
                payload.BankAccountHolder = customerRegistration.BankAccountHolder;
                payload.MobileNetwork = customerRegistration.MobileNetwork;
                payload.MobileNumber = customerRegistration.MobileNumber;
                payload.ProfileImagePath = profileImagePath; // Save the profile image path

                _context.Customers.Add(payload);

                // Add company associations
                foreach (var companyId in customerRegistration.Companies)
                {
                    var companyCustomer = new CompanyCustomer
                    {
                        CustomerId = payload.CustomerID,
                        CompanyId = companyId,
                        CreationDate = DateTime.UtcNow.ToLocalTime(),
                    };

                    _context.CompanyCustomers.Add(companyCustomer);
                }

                // Create the SecUser for the new customer
                var secUser = new SecUser
                {
                    UserID = payload.CustomerID, // Use CustomerID as UserID
                    Email = payload.Email, // Use customer's email
                    PasswordHash = Functions.HashPassword(customerRegistration.Password), // Use customer-supplied password
                    Role = "CUSTOMER",
                    Status = "ACTIVE"
                };

                // Add SecUser to the context
                _context.SecUsers.Add(secUser);
                await _context.SaveChangesAsync();

                // Return success with customer data
                executionResult.SetData(payload);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                // Handle error
                executionResult.SetInternalServerError(nameof(CompanyController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion

        #region GetAllCustomers
        [HttpGet]
        [Route("GetAllCustomers")]
        public IActionResult GetAllCustomers()
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(GetAllCustomers);

            try
            {
                var customers = _context.Customers
                     //.Include(c => c.JobRequests)
                    .ToList();
                executionResult.SetData(customers);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(CustomerController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion

        #region GetCompanyCustomers
        [HttpGet]
        [Route("GetCustomersByCompany/{companyId}")]
        public async Task<IActionResult> GetCustomersByCompany(string companyId)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(GetCustomersByCompany);

            try
            {
                // Get customer IDs for the specified company
                var customerIds = await _context.CompanyCustomers
                    .Where(cc => cc.CompanyId == companyId)
                    .Select(cc => cc.CustomerId)
                    .ToListAsync();

                if (customerIds == null || customerIds.Count == 0)
                {
                    return NotFound("No customers found for this company.");
                }

                // Retrieve customers based on customer IDs
                var customers = await _context.Customers
                    .Where(c => customerIds.Contains(c.CustomerID))
                    .ToListAsync();

                executionResult.SetData(customers);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(CustomerController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion

        #region GetCustomerById
        [HttpGet]
        [Route("GetCustomerById/{id}")]
        public IActionResult GetCustomerById(string id)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(GetCustomerById);

            try
            {
                var customer = _context.Customers
                     //.Include(c => c.JobRequests)
                    .FirstOrDefault(c => c.CustomerID == id);

                if (customer == null)
                {
                    return NotFound("Customer not found");
                }

                executionResult.SetData(customer);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(CustomerController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion


        #region UpdateCustomer
        [HttpPut]
        [Route("UpdateCustomer/{id}")]
        public async Task<IActionResult> UpdateCustomer(string id, [FromBody] CustomerPayload updatedCustomerPayload)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(UpdateCustomer);

            try
            {
                var customer = await _context.Customers.FindAsync(id);

                if (customer == null)
                {
                    return NotFound("Customer not found");
                }

                // Update the customer's properties from the payload, check for null or empty values
                customer.FullName = string.IsNullOrEmpty(updatedCustomerPayload.FullName) ? customer.FullName : updatedCustomerPayload.FullName;
                customer.Email = string.IsNullOrEmpty(updatedCustomerPayload.Email) ? customer.Email : updatedCustomerPayload.Email;
                customer.Phone = string.IsNullOrEmpty(updatedCustomerPayload.Phone) ? customer.Phone : updatedCustomerPayload.Phone;
                customer.Address = string.IsNullOrEmpty(updatedCustomerPayload.Address) ? customer.Address : updatedCustomerPayload.Address;

                // Update payment info if provided (nullable fields)
                if (!string.IsNullOrEmpty(updatedCustomerPayload.PaymentMethod))
                {
                    customer.PaymentMethod = updatedCustomerPayload.PaymentMethod;
                }
                if (!string.IsNullOrEmpty(updatedCustomerPayload.CardNumber))
                {
                    customer.CardNumber = updatedCustomerPayload.CardNumber;
                }
                if (!string.IsNullOrEmpty(updatedCustomerPayload.CardType))
                {
                    customer.CardType = updatedCustomerPayload.CardType;
                }
                if (!string.IsNullOrEmpty(updatedCustomerPayload.BillingAddress))
                {
                    customer.BillingAddress = updatedCustomerPayload.BillingAddress;
                }
                if (!string.IsNullOrEmpty(updatedCustomerPayload.ExpiryDate))
                {
                    customer.ExpiryDate = updatedCustomerPayload.ExpiryDate;
                }
                if (!string.IsNullOrEmpty(updatedCustomerPayload.BankName))
                {
                    customer.BankName = updatedCustomerPayload.BankName;
                }
                if (!string.IsNullOrEmpty(updatedCustomerPayload.BankAccountNumber))
                {
                    customer.BankAccountNumber = updatedCustomerPayload.BankAccountNumber;
                }
                if (!string.IsNullOrEmpty(updatedCustomerPayload.BankAccountHolder))
                {
                    customer.BankAccountHolder = updatedCustomerPayload.BankAccountHolder;
                }
                if (!string.IsNullOrEmpty(updatedCustomerPayload.MobileNetwork))
                {
                    customer.MobileNetwork = updatedCustomerPayload.MobileNetwork;
                }
                if (!string.IsNullOrEmpty(updatedCustomerPayload.MobileNumber))
                {
                    customer.MobileNumber = updatedCustomerPayload.MobileNumber;
                }

                // Save changes to the database
                await _context.SaveChangesAsync();

                // Return the updated customer
                executionResult.SetData(customer);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                // Handle any exceptions and return an error
                executionResult.SetInternalServerError(nameof(CustomerController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion

        #region DeleteCustomer
        [HttpDelete]
        [Route("DeleteCustomer/{id}")]
        public async Task<IActionResult> DeleteCustomer(string id)
        {
            var executionResult = new ExecutionResult();
            string functionName = nameof(DeleteCustomer);

            try
            {
                var customer = await _context.Customers.FindAsync(id);

                if (customer == null)
                {
                    return NotFound("Customer not found");
                }

                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();

                executionResult.SetData(customer);
                return Ok(executionResult.GetServerResponse());
            }
            catch (Exception ex)
            {
                executionResult.SetInternalServerError(nameof(CustomerController), functionName, ex);
                return StatusCode(executionResult.GetStatusCode(), executionResult.GetServerResponse().Message);
            }
        }
        #endregion
    }
}
