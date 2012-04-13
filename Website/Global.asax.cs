using System.Web.Mvc;
using System.Web.Routing;
using Common;
using NServiceBus;

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
            var configuration =
               Configure.WithWeb()
                   .DefiningEventsAs(t => t.Namespace != null && t.Namespace.StartsWith("Messages"))
                   .DefaultBuilder()
                   .XmlSerializer()
                   .Log4Net()
                   .MsmqSubscriptionStorage("Simulator")
                   .MsmqTransport()
                       .IsTransactional(false)
                       .PurgeOnStartup(true)
                   .DefineEndpointName("Simulator")
                   .UnicastBus()
                       .ImpersonateSender(false);

            var bus = 
                configuration
                    .CreateBus()
                    .Start(() => Configure.Instance.ForInstallationOn<NServiceBus.Installation.Environments.Windows>().Install());

            ServiceBus = new NServiceBusAdapter(bus);
        }


        class NServiceBusAdapter : IServiceBus
        {
            private readonly IBus _bus;

            public NServiceBusAdapter(IBus bus)
            {
                _bus = bus;
            }

            public void Publish<T>(T message)
            {
                _bus.Publish(message);
            }
        }
    }
}