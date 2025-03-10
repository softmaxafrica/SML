using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
 using NuGet.Protocol.Plugins;
using System;
using System.Security.Principal;
using System.Threading.Tasks;
using lsclient.Server.Controllers;
using lsclient.Server.Models;
using lsclient.Server.Shared;
 
namespace WakalaPlus.Hubs
{
    public class SignalHub : Hub
    {
        public static readonly Dictionary<string, string> DeviceConnections = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _connectionIdMap = new Dictionary<string, string>();

        public string _connectionId;
        private static IConfiguration config;
        public readonly AppDbContext dbContext;



        private static string className = "SignalHub";

        public SignalHub(IConfiguration _config, AppDbContext dbContext)
        {
            config = _config;
            this.dbContext = dbContext;

        }
        //public JobRequestController _requestController = new JobRequestController(dbContext, config);
 


        //public override async Task OnConnectedAsync()
        //{
        //    var httpContext = Context.GetHttpContext();
        //    if (httpContext == null) return;

        //    // Extract the device details from the query string or headers
        //    var deviceId = httpContext.Request.Query["deviceId"];
        //    var identity = httpContext.Request.Query["identity"]; // e.g., "agent" or "customer"
        //    var connectionId = Context.ConnectionId;

        //    if (string.IsNullOrWhiteSpace(deviceId) || string.IsNullOrWhiteSpace(identity))
        //    {
        //        await Clients.Caller.SendAsync("ErrorMessage", "Device ID and Identity are required.");
        //        return;
        //    }

        //    // Prepare device connection details
        //    var deviceDetail = new TruckDeviceLocationDetails
        //    {
        //        deviceId = deviceId,
        //        Identity = identity,
        //        connectionId = connectionId,
        //        LastAction = "Insert",
        //        createdDate = DateTime.Now.ToLocalTime(),
        //    };

        //    using (var context = new AppDbContext(config))
        //    {
        //        // Check if the device already exists
        //        var existingDevice = context.TruckDeviceLocationDetails.FirstOrDefault(d => d.deviceId == deviceId);

        //        if (existingDevice != null)
        //        {
        //            // Update existing device details
        //            existingDevice.connectionId = connectionId;
        //            existingDevice.LastAction = "Update";
        //            existingDevice.LastConnectionDate = DateTime.Now.ToLocalTime();

                     
        //        }
        //        else
        //        {
        //            // Insert new device details
        //            context.TruckDeviceLocationDetails.Add(deviceDetail);
        //        }

        //        // Retrieve pending ticket and send it asynchronously
        //        //await Task.Run(async () =>
        //        //{
        //        //    CustomerTickets ongoingTicket = null;
        //        //    if (identity == "agent")
        //        //    {
        //        //        ongoingTicket = context.CustomerTickets
        //        //            .LastOrDefault(t => t.ticketStatus == "ASSIGNED" && t.agentCode == deviceId);
        //        //    }
        //        //    else if (identity == "customer")
        //        //    {
        //        //        ongoingTicket = context.CustomerTickets
        //        //            .LastOrDefault(t => t.ticketStatus == "ASSIGNED" && t.phoneNumber == deviceId);
        //        //    }

        //        //    if (ongoingTicket != null)
        //        //    {
        //        //        var preparedOngoingTicket = new PreparedCustomerTicket
        //        //        {
        //        //            transactionId = ongoingTicket.transactionId,
        //        //            phoneNumber = ongoingTicket.phoneNumber,
        //        //            description = ongoingTicket.description,
        //        //            network = ongoingTicket.network,
        //        //            serviceRequested = ongoingTicket.serviceRequested,
        //        //            custLatitude = ongoingTicket.customerLatitude ?? 0,
        //        //            custLongitude = ongoingTicket.customerLongitude ?? 0,
        //        //            agentCode = ongoingTicket.agentCode,
        //        //            agentLatitude = ongoingTicket.agentLatitude,
        //        //            agentLongitude = ongoingTicket.agentLongitude,
        //        //            createdDate = ongoingTicket.ticketCreationDateTime,
        //        //            LastResponseDateTime = ongoingTicket.ticketLastResponseDateTime
        //        //        };

        //        //        await Clients.Client(connectionId).SendAsync("ReceiveOnGoingTicket", preparedOngoingTicket);
        //        //    }
        //        //});
            

        //    await context.SaveChangesAsync();
        //    }

        //    // Notify all clients (optional)
        //    await Clients.All.SendAsync("ReceiveMessage", $"Device {deviceId} connected with identity {identity}.");
        //}


        public async Task SendMessage(string message)
        {
            // Broadcasting the message to all connected clients
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

       

       
        //private async Task<string> GetConnectionIdById(string Id)
        //{
        //    try
        //    {
        //        var deviceDetails = await _dbContext.DeviceDetails
        //            .FirstOrDefaultAsync(d => d.deviceId == phoneNumber);

        //        if (deviceDetails != null)
        //        {
        //            return deviceDetails.connectionId;
        //        }
        //        else
        //        {
        //            Console.WriteLine("Device details not found for the given phone number.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error retrieving connection ID: {ex.Message}");
        //    }

        //    return null;
        //}

      
   

    }
}

