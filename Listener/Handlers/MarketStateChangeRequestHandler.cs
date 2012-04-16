using System;
using MassTransit;
using Messages;

namespace Listener.Handlers
{
    public class MarketStateChangeHandler : Consumes<MarketStateChange>.All
    {
        public void Consume(MarketStateChange message)
        {
            Console.WriteLine("[=======  Market {0}  =======]", message.CurrentState);
        }
    }
}
