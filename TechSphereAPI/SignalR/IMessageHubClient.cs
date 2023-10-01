using System.Collections.Generic;
using System.Threading.Tasks;

namespace TopShopBuyer.SignalR
{
    public interface IMessageHubClient
    {

            Task OnCheckout(string message);
        
    }
}
