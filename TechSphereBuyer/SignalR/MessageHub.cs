using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TopShopBuyer.SignalR
{
    public class MessageHub:Hub<IMessageHubClient>
    {
        public async Task onCheckout(int message)
        {
            await Clients.Others.onCheckout("Someone just completed a purchase and left a five star review");
        }
    }
}
