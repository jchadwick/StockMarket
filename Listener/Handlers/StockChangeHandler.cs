using System;
using MassTransit;
using Messages;

namespace Listener.Handlers
{
    public class StockChangeHandler : Consumes<IStockChangeEvent>.All
    {
        public void Consume(IStockChangeEvent message)
        {
            Console.WriteLine("****************** {0} Changed ******************", message.Symbol);
        }
    }
}
