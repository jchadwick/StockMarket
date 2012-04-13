using System;

namespace Messages
{
    [Serializable]
    public abstract class StockChangeEvent : IStockChangeEvent
    {
        public Guid Id { get; private set; }
        
        public DateTime Timestamp { get; private set; }

        public string Symbol { get; set; }

        public decimal DayOpen { get; set; }

        public decimal DayLow { get; set; }

        public decimal DayHigh { get; set; }

        public decimal Price { get; set; }


        protected StockChangeEvent()
        {
            Id = Guid.NewGuid();
            Timestamp = DateTime.Now;
        }
    }
}