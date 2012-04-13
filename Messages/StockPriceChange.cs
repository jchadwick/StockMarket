using System;

namespace Messages
{
    [Serializable]
    public class StockPriceChange : StockChangeEvent
    {
        public decimal LastChange { get; set; }
        public decimal Change { get; set; }
        public double PercentChange { get; set; }
    }
}