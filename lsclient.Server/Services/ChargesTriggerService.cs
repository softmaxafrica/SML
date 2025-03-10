using lsclient.Server.Controllers;
using lsclient.Server.Controllers.Billing;
using lsclient.Server.Models;
using lsclient.Server.Shared;
using Microsoft.Extensions.Configuration;

namespace lsclient.Server.Services
{
    public class ChargesTriggerService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private int _delayInSeconds;

        public ChargesTriggerService(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _delayInSeconds = _configuration.GetValue<int>("BackgroundService:DelayInSeconds");

            while (!stoppingToken.IsCancellationRequested)
            {
 
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    var chargableItemController = new ChargableItemsController(dbContext, _configuration);

                    try
                    {
                        using (var transaction = await dbContext.Database.BeginTransactionAsync())
                        {
                            var jobRequestController = new JobRequestController(dbContext, _configuration);
                            var jrItemsToBill = jobRequestController.GetPendingChargableItems();
                            if (jrItemsToBill != null && jrItemsToBill.Any())
                            {
                                foreach (var item in jrItemsToBill)
                                {
 
                                    var newItem1 = new ChargableItemPayload
                                    {
                                        JobRequestID = item.JobRequestID,
                                        PriceAgreementID = item.PriceAgreementID,
                                        Status = "PENDING",
                                        CustomerID = item.CustomerID,
                                        Amount = item.PriceAgreement.AgreedPrice * (0.9),
                                        ItemDescription = "SERVICE CHARGES",
                                        IssueDate = DateTime.UtcNow,
                                        InvoiceNumber = null,
                                    };

                                    var newItem2 = new ChargableItemPayload
                                    {
                                        JobRequestID = item.JobRequestID,
                                        PriceAgreementID = item.PriceAgreementID,
                                        Status = "PENDING",
                                        CustomerID = item.CustomerID,
                                        Amount = item.PriceAgreement.AgreedPrice * (0.1),
                                        ItemDescription = "OPERATIONAL CHARGES",
                                        IssueDate = DateTime.UtcNow,
                                        InvoiceNumber = null,
                                    };

                                    await chargableItemController.AddChargableItem(newItem1);
                                    await chargableItemController.AddChargableItem(newItem2);

                                    // Update JobRequest Status
                                    await jobRequestController.UpdateRequestStatus(item.JobRequestID, "ONGOING INVOICE GENERATION",item.InvoiceNumber);
                                }
                            }

                            // Commit the transaction if all operations succeed
                            await transaction.CommitAsync();
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
