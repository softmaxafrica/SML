using lsclient.Server.Controllers;
using lsclient.Server.Controllers.Billing;
using lsclient.Server.Models;
using lsclient.Server.Shared;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations.Schema;

namespace lsclient.Server.Services
{
    public class InvoicingService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private int _delayInSeconds;

        public InvoicingService(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _delayInSeconds = _configuration.GetValue<int>("BackgroundService:InvoicingServiceDelayInSeconds");

            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("Invoice  Service Is Running At " + DateTime.Now.ToLocalTime());

                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    var chargableItemController = new ChargableItemsController(dbContext, _configuration);
                    var jobRequestController = new JobRequestController(dbContext, _configuration);
                    var invoiceController = new InvoiceController(dbContext, _configuration);

                    try
                    {
                        using (var transaction = await dbContext.Database.BeginTransactionAsync())
                        {
                            
                            // Get all chargeable items grouped by JobRequestID
                            var itemsToInvoice = chargableItemController.GetAllChargableItemsToBill()
                                .Where(item=>item.Status== "PENDING")
                                .GroupBy(item => item.JobRequestID)
                                .ToList();

                            if (itemsToInvoice.Any())
                            {
                                foreach (var group in itemsToInvoice)
                                {
                                    var jobRequestId = group.Key;
                                    var serviceCharge = group
                                        .Where(i => i.ItemDescription == "SERVICE CHARGES")
                                        .Sum(i => i.Amount);
                                    var operationalCharge = group
                                        .Where(i => i.ItemDescription == "OPERATIONAL CHARGES")
                                        .Sum(i => i.Amount);

                                    // Skip this job request if no charges are found
                                    if (serviceCharge == 0 && operationalCharge == 0)
                                        continue;

                                    var totalAmount = serviceCharge + operationalCharge;
                                    var firstItem = group.First(); // To get shared attributes like CustomerID, etc.

                                    // Create the invoice
                                    var newInvoice = new Invoice
                                    {
                                        JobRequestID = jobRequestId,
                                        PaymentId = null,
                                        TotalAmount = totalAmount,
                                        ServiceCharge = serviceCharge,
                                        OperationalCharge = operationalCharge,
                                        IssueDate = DateTime.UtcNow.ToLocalTime(),
                                        DueDate = null,
                                        Status = "DRAFT",
                                        CustomerID = firstItem.CustomerID,
                                        CompanyID= firstItem.JobRequest.AssignedCompany,
                                        TotalPaidAmount=0,
                                        OwedAmount= totalAmount
                                    };

                                     var savedInvoice = await invoiceController.CreateInvoice(newInvoice);
                                    int generatedInvoiceNumber = savedInvoice.InvoiceNumber;

                                    //await chargableItemController.UpdateChargableItemsStatus(jobRequestId, "DRAFT");
                                    // Update JobRequest Status
                                    await chargableItemController.UpdateChargableItemsStatusAndInvoiceNumber(jobRequestId, "DRAFT", generatedInvoiceNumber);

                                    await jobRequestController.UpdateRequestStatus(jobRequestId, "DRAFT", generatedInvoiceNumber);

                                }
                            }

                            // Commit the transaction if all operations succeed
                             transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error occurred while processing items: {ex.Message}");

                        // Rollback the transaction in case of an error
                        if (dbContext.Database.CurrentTransaction != null)
                        {
                            await dbContext.Database.CurrentTransaction.RollbackAsync();
                        }
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(_delayInSeconds), stoppingToken);
            }
        }

    }
}
