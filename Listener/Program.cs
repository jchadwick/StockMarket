using System;
using Listener.Handlers;
using MassTransit;

public class Program
{
    public static void Main()
    {
        Console.WriteLine("Initializing...");
        
        Bus.Initialize(sbc =>
        {
            sbc.UseRabbitMqRouting();
            sbc.ReceiveFrom("rabbitmq://localhost/StockMarket_Listener");

            sbc.Subscribe(subs => subs.Consumer<MarketStateChangeHandler>());
            sbc.Subscribe(subs => subs.Consumer<StockPriceDecreaseHandler>());
            sbc.Subscribe(subs => subs.Consumer<StockPriceIncreaseHandler>());
        });

        Console.WriteLine("Initialized");
        Console.ReadLine();
    }
}