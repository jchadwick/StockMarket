using System;

namespace Messages
{
    public interface IStockChangeEvent
    {
        Guid Id { get; }
        DateTime Timestamp { get; }
        string Symbol { get; set; }
        decimal DayOpen { get; set; }
        decimal DayLow { get; set; }
        decimal DayHigh { get; set; }
        decimal Price { get; set; }
    }
}