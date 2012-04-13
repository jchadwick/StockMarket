using System.Web.Mvc;
using System.Web.Routing;
using Common;
using Messages;
using Website.Listeners;

namespace Website
{
    public class MvcApplication : System.Web.HttpApplication
    {
        // If we had an IoC container we wouldn't need these...
        public static IServiceBus ServiceBus { get; private set; }
        public static StockRepository StockRepository { get; private set; }
        public static StockMarket StockMarket { get; private set; }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "StockMarket", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            StockRepository = new StockRepository();

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            StartStockMarketSimulator();

            InitializeServiceBus();
        }

        private void InitializeServiceBus()
        {
            var serviceBus = new ServiceBus();

            serviceBus.Subscribe<MarketStateChangeRequest>(
                changeRequest => StockMarket.MarketState = changeRequest.NewState);

            ServiceBus = serviceBus;
        }

        private void StartStockMarketSimulator()
        {
            StockMarket = new StockMarket(StockRepository);
            StockMarket.AddStock("MSFT", 26.31m);
            StockMarket.AddStock("APPL", 404.18m);
            StockMarket.AddStock("GOOG", 596.30m);
            StockMarket.AddStock("SUN", 596.30m);
            StockMarket.AddStock("CSCO", 300);
            StockMarket.AddStock("AMZN", 170);

            var stockMarketListener = new StockMarketListener();
            
            StockMarket.MarketStateChanged += (sender, args) =>
                stockMarketListener.Handle(new MarketStateChange(args.Data));
            
            StockMarket.StockChanged += (sender, args) =>
                stockMarketListener.Handle(StockChangeEventFactory.Create(args.Data));

            
            StockMarket.Start();
        }
    }
}