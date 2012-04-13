using System;
using Messages;
using NServiceBus;

namespace Listener.Handlers
{
    public class StockPriceIncreaseHandler : IHandleMessages<StockPriceIncrease>
    {
        public void Handle(StockPriceIncrease message)
        {
            Console.WriteLine("+++++++  {0}: {1} ({2})  ++++++++", message.Symbol, message.Price, message.Change);
        }
    }
}
