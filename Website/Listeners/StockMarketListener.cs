using Common;
using Messages;
using SignalR;
using SignalR.Hosting.AspNet;
using SignalR.Infrastructure;
using Website.Controllers;

namespace Website.Listeners
{
    public class StockMarketListener
    {
        private readonly IStockRepository _stocks;


        public StockMarketListener()
            : this(MvcApplication.StockRepository)
        {
        }

        public StockMarketListener(IStockRepository stocks)
        {
            _stocks = stocks;
        }


        public void Handle(IStockChangeEvent message)
        {
            var symbol = message.Symbol;
            var stock = _stocks.GetStockBySymbol(symbol);

            if (stock == null)
            {
                stock = new Stock { Symbol = symbol, Price = message.Price };
                _stocks.AddStock(stock);

                GetClients().addStock(message);
            }
            else
            {
                stock.Price = message.Price;
                GetClients().updateStockPrice(message);
            }
        }

        public void Handle(MarketStateChange message)
        {
            GetClients().updateMarketState(message.CurrentState.ToString());
        }

        private static dynamic GetClients()
        {
            return AspNetHost.DependencyResolver.Resolve<IConnectionManager>().GetClients<StockTickerHub>();
        }
    }
}