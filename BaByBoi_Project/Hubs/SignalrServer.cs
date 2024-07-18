using Microsoft.AspNetCore.SignalR;

namespace FUMiniHotelManagement.Hubs
{
    public class SignalrServer : Hub
    {
        public async Task UpdateRoomData()
        {
            await Clients.All.SendAsync("LoadRooms");
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
