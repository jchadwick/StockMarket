using System;
using Messages;
using NServiceBus;

namespace Listener.Handlers
{
    public class StockPriceDecreaseHandler : IHandleMessages<StockPriceDecrease>
    {
        public void Handle(StockPriceDecrease message)
        {
            Console.WriteLine("-------  {0}: {1} ({2})  -------", message.Symbol, message.Price, message.Change);
        }
    }
}