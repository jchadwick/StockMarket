using System;
using MassTransit;
using Messages;

namespace Listener.Handlers
{
    public class StockPriceIncreaseHandler : Consumes<StockPriceIncrease>.All
    {
        public void Consume(StockPriceIncrease message)
        {
            Console.WriteLine("+++++++  {0}: {1} ({2})  ++++++++", message.Symbol, message.Price, message.Change);
        }
    }
}
