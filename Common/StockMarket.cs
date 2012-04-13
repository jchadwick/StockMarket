using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Common
{
    public class StockMarket
    {
        private readonly IStockRepository _stocks;
        private readonly Random _stockPicker = new Random();
        private readonly Random _priceRandomizer = new Random();

        private CancellationTokenSource _cancellationToken;

        public static StockMarket Instance
        {
            get { return _instance; }
            set { _instance = value; }
        }
        private static volatile StockMarket _instance;

        public MarketState MarketState
        {
            get { return _marketState; }
            set
            {
                if (_marketState == value) return;

                _marketState = value;
                PublishMarketState();
            }
        }
        private volatile MarketState _marketState = MarketState.Closed;

        /// <summary>
        /// Stock prices can go up or down by a percentage of this factor on each change
        /// </summary>
        public double RangePercent { get; set; }


        public event EventHandler<EventArgs<MarketState>> MarketStateChanged;

        public event EventHandler<EventArgs<Stock>> StockChanged;


        public StockMarket()
            : this(new StockRepository())
        {
        }

        public StockMarket(IStockRepository stocks)
        {
            _stocks = stocks;
            RangePercent = .01;
        }


        public void AddStock(string symbol, decimal? price = null)
        {
            var stock = new Stock
            {
                Symbol = symbol,
                Price = price.GetValueOrDefault(_priceRandomizer.Next(10, 700))
            };

            AddStock(stock);
        }

        public void AddStock(Stock stock)
        {
            _stocks.AddStock(stock);
            if (stock == null)
                return;

            stock.Price += GetRandomPriceChange(stock);

            if (StockChanged != null)
                StockChanged(this, new EventArgs<Stock>(stock));
        }

        public void Start()
        {
            if (_cancellationToken != null)
                return;

            _cancellationToken = new CancellationTokenSource();

            // Run a crazy Stock Market simulation
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (_marketState != MarketState.Open)
                        continue;

                    ChangeThePriceOfARandomStock();
                    Thread.Sleep(TimeSpan.FromSeconds(.5));
                }
            }, _cancellationToken.Token);

            // Send out a Market State ping every few seconds
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(3));
                    PublishMarketState();
                }
            }, _cancellationToken.Token);
        }

        public void Stop()
        {
            if (_cancellationToken == null || _cancellationToken.IsCancellationRequested)
                return;

            MarketState = MarketState.Closing;

            _cancellationToken.Cancel();
            _cancellationToken = null;

            MarketState = MarketState.Closed;
        }

        private void ChangeThePriceOfARandomStock()
        {
            var stock = GetRandomStock();

            if (stock == null)
                return;

            var priceChange = GetRandomPriceChange(stock);

            if (priceChange == 0)
                return;

            stock.Price += priceChange;

            if (StockChanged != null)
                StockChanged(this, new EventArgs<Stock>(stock));
        }

        private Stock GetRandomStock()
        {
            var stocks = _stocks.GetStocks().ToArray();

            if (stocks.Length == 0)
                return null;

            var stock = stocks[_stockPicker.Next(stocks.Length)];
            return stock;
        }

        private void PublishMarketState()
        {
            if(MarketStateChanged != null)
                MarketStateChanged(this, new EventArgs<MarketState>(MarketState));
        }

        private decimal GetRandomPriceChange(Stock stock)
        {
            // Update the stock price by a random factor of the range percent
            var random = new Random((int)Math.Floor(stock.Price));
            var percentChange = random.NextDouble() * RangePercent;
            var pos = random.NextDouble() > .51;
            var change = Math.Round(stock.Price * (decimal)percentChange, 2);
            change = pos ? change : -change;

            return change;
        }
    }
}