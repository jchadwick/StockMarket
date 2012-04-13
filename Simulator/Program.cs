using System;
using System.Threading;
using Common;
using Messages;

namespace Simulator
{
    public class Program
    {
        private static IServiceBus _bus;
        private static StockMarket _stockMarket;

        public static void Main()
        {
            new Program().Run();
        }

        public void Run()
        {
            Console.WriteLine("******  Stock Market Simulator Ultimate Developer Edition Alpha ******\r\n");

            Console.Write("Initializing...  ");

            Initialize();

            Console.WriteLine("initialized.");

        
            Console.Write("Starting Stock Market...");
            _stockMarket.Start();
            Console.WriteLine("started.");


            Console.WriteLine("Adding some stocks...");
            _stockMarket.AddStock("MSFT", 26.31m);
            _stockMarket.AddStock("APPL", 404.18m);
            _stockMarket.AddStock("GOOG", 596.30m);
            _stockMarket.AddStock("SUN", 596.30m);
            _stockMarket.AddStock("CSCO", 300);
            _stockMarket.AddStock("AMZN", 170);


            ShowMenu();
        }

        private void Initialize()
        {
            _stockMarket = StockMarket.Instance = new StockMarket();
            _stockMarket.StockChanged += (sender, args) =>
                Console.WriteLine("  ==== {0} {1} ({2}) ====",
                                  args.Data.Symbol, args.Data.Price, args.Data.LastChange);

            _bus = new ServiceBus();
        }

        private void ShowMenu()
        {
            Console.WriteLine("=====  Virtual Stock Market  =====");

            ConsoleKey keyPress;
            do
            {
                Console.WriteLine();
                Console.WriteLine("Stock market is " + _stockMarket.MarketState);
                Console.Write("[A]dd Stock; [O]pen or [C]lose the market (X to exit): ");
                keyPress = Console.ReadKey().Key;

                Console.WriteLine();

                switch (keyPress)
                {
                    case ConsoleKey.A:
                        Console.Write("Symbol: ");
                        var symbol = Console.ReadLine();
                        _stockMarket.AddStock(symbol);
                        break;
                    case ConsoleKey.C:
                        _stockMarket.MarketState = MarketState.Closed;
                        break;
                    case ConsoleKey.O:
                        _stockMarket.MarketState = MarketState.Open;
                        break;
                    case ConsoleKey.R:
                        _stockMarket.MarketState = MarketState.Reset;
                        break;
                    case ConsoleKey.X:
                        Console.WriteLine("Shutting down...");
                        _bus.Publish(new MarketStateChange(MarketState.Closed));
                        Thread.Sleep(2);
                        Environment.Exit(0);
                        break;
                }
            } while (keyPress != ConsoleKey.X);
        }
    }
}