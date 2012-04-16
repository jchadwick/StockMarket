using Common;

namespace Messages
{
    public class StockChangeEventFactory
    {
        public static IStockChangeEvent Create(Stock stock)
        {
            StockPriceChange change;

            if (stock.LastChange > 0)
                change = new StockPriceIncrease();
            else if (stock.LastChange < 0)
                change = new StockPriceDecrease();
            else
            {
                return null;
            }

            change.Change = stock.Change;
            change.DayHigh = stock.DayHigh;
            change.DayLow = stock.DayLow;
            change.DayOpen = stock.DayOpen;
            change.LastChange = stock.LastChange;
            change.PercentChange = stock.PercentChange;
            change.Price = stock.Price;
            change.Symbol = stock.Symbol;

            return change;
        }
    }
}