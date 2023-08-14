using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using TopShopBuyer.DataLayer;
using TopShopBuyer.Models;



namespace TopShopBuyer.SignalR
{
    [Authorize]
    public class MessageHub : Hub<IMessageHubClient>
    {
        private readonly IProducts _productDA;
        private static Dictionary<int, string> connectionIDs = new Dictionary<int, string>();
        private static List<string> requiredConnectionIDs = new List<string>();

        public MessageHub(IProducts productDA)
        {
            _productDA = productDA;
        }
        public override Task OnConnectedAsync()
        {
            connectionIDs[Convert.ToInt32(Context.User.FindFirstValue(ClaimTypes.SerialNumber))] = Context.ConnectionId;
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            connectionIDs.Remove(Convert.ToInt32(Context.User.FindFirstValue(ClaimTypes.SerialNumber)));
            return base.OnDisconnectedAsync(exception);
        }

        public async Task OnCheckout(List<int> products)
        {
            List<Cart> allActiveCarts = await _productDA.GetAllActiveCarts();

            foreach (Cart cart in allActiveCarts)
            {
                for (int i = 0; i < cart.Products.Count; i++)
                {
                    bool present = products.Contains(cart.Products[i].Id);
                    if (present && connectionIDs.ContainsKey(Convert.ToInt32(cart.BuyerId)) && connectionIDs[cart.BuyerId] != Context.ConnectionId)
                    {
                        requiredConnectionIDs.Add(connectionIDs[cart.BuyerId]);
                        break;
                    }
                }
            }

            await Clients.Clients(requiredConnectionIDs).OnCheckout("Someone just purchased the same products as you. Hurry Up!");

            requiredConnectionIDs.Clear();

            //await Clients.Others.onCheckout("Someone just completed a purchase and left a five star review");
        }
    }
}