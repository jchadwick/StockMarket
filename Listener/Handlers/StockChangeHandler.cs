using System;
using NServiceBus;
using Messages;

namespace Listener.Handlers
{
    public class StockChangeHandler : IHandleMessages<IStockChangeEvent>
    {
        public void Handle(IStockChangeEvent message)
        {
            Console.WriteLine("****************** {0} Changed ******************", message.Symbol);
        }
    }
}
