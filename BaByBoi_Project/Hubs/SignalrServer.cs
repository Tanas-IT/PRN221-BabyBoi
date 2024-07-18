using Microsoft.AspNetCore.SignalR;

namespace FUMiniHotelManagement.Hubs
{
    public class SignalrServer : Hub
    {
        public async Task UpdateOrderData()
        {
            await Clients.All.SendAsync("ReceiveNewOrder");
        }

        public async Task UpdateCustomerData()
        {
            await Clients.All.SendAsync("LoadCustomers");
        }

        public async Task SendNewOrderCount(int count)
        {
            await Clients.All.SendAsync("ReceiveNewOrderCount", count);
        }
    }
}
