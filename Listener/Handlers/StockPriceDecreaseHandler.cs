using System;
using MassTransit;
using Messages;

namespace Listener.Handlers
{
    public class StockPriceDecreaseHandler : Consumes<StockPriceDecrease>.All
    {
        public void Consume(StockPriceDecrease message)
        {
            Console.WriteLine("-------  {0}: {1} ({2})  -------", message.Symbol, message.Price, message.Change);
        }
    }
}