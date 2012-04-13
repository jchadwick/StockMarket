using System.Collections.Generic;
using System.Web.Mvc;
using Common;
using Messages;
using SignalR.Hubs;

namespace Website.Controllers
{
    [HubName("stockTicker")]
    public class StockTickerHub : Hub
    {
        private readonly IServiceBus _bus;
        private readonly IStockRepository _stocks;


        public StockTickerHub()
            : this(MvcApplication.ServiceBus, MvcApplication.StockRepository)
        {
        }

        public StockTickerHub(IServiceBus bus, IStockRepository stocks)
        {
            _bus = bus;
            _stocks = stocks;
        }

        public IEnumerable<Stock> GetAllStocks()
        {
            return _stocks.GetStocks();
        }

        public string GetMarketState()
        {
            return MarketState.Closed.ToString();
        }

        public void OpenMarket()
        {
            _bus.Publish(new MarketStateChangeRequest(MarketState.Open));
        }

        public void CloseMarket()
        {
            _bus.Publish(new MarketStateChangeRequest(MarketState.Closed));
        }

        public void Reset()
        {
            _bus.Publish(new MarketStateChangeRequest(MarketState.Reset));
        }
    }
}