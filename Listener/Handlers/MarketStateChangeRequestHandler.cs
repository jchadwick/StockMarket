using System;
using Messages;
using NServiceBus;

namespace Listener.Handlers
{
    public class MarketStateChangeHandler : IHandleMessages<MarketStateChange>
    {
        public void Handle(MarketStateChange message)
        {
            Console.WriteLine("[=======  Market {0}  =======]", message.CurrentState);
        }
    }
}
