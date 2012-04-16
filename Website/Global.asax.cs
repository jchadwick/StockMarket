using System.Web.Mvc;
using System.Web.Routing;
using Common;
using MassTransit;
using Messages;
using Website.Listeners;
using IServiceBus = Common.IServiceBus;

namespace Website
{
    public class MvcApplication : System.Web.HttpApplication
    {
        // If we had an IoC container we wouldn't need these...
        public static IServiceBus ServiceBus { get; private set; }
        public static StockRepository StockRepository { get; private set; }

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

            InitializeServiceBus();
        }

        private void InitializeServiceBus()
        {
            var listener = new StockMarketListener();

            var bus = ServiceBusFactory.New(sbc => {
                sbc.UseRabbitMqRouting();
                sbc.ReceiveFrom("rabbitmq://localhost/StockMarket_Website");

                sbc.Subscribe(subs => subs.Handler<StockPriceChange>(listener.Handle));
                sbc.Subscribe(subs => subs.Handler<MarketStateChange>(listener.Handle));
            });
        }
    }
}