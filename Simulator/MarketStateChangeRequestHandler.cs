using System;
using Common;
using Messages;
using NServiceBus;
using log4net;

namespace Simulator
{
    public class MarketStateChangeRequestHandler : IHandleMessages<MarketStateChangeRequest>
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(MarketStateChangeRequestHandler));

        public void Handle(MarketStateChangeRequest message)
        {
            Logger.Info("Market State Request: " + message.NewState);
            Console.WriteLine("**** Changing Market State to {0} ****", message.NewState);

            StockMarket.Instance.MarketState = message.NewState;
        }
    }
}
