using System.Collections.Generic;
using System.Linq;

namespace Common
{
    public interface IStockRepository
    {
        void AddStock(Stock stock);
        Stock GetStockBySymbol(string symbol);
        IEnumerable<Stock> GetStocks();
    }

    public class StockRepository : IStockRepository
    {
        private readonly IDictionary<string, Stock> _stocks;

        public StockRepository(IEnumerable<Stock> stocks = null)
        {
            _stocks = new Dictionary<string, Stock>();
            new List<Stock>(stocks ?? Enumerable.Empty<Stock>())
                .ForEach(x => _stocks.Add(x.Symbol.ToUpper(), x));
        }

        public void AddStock(Stock stock)
        {
            _stocks.Add(stock.Symbol, stock);
        }

        public IEnumerable<Stock> GetStocks()
        {
            return _stocks.Values;
        }

        public Stock GetStockBySymbol(string symbol)
        {
            Stock stock;
            _stocks.TryGetValue(symbol.ToUpper(), out stock);
            return stock;
        }
    }
}