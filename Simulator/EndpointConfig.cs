using NServiceBus;

namespace Simulator
{
    class EndpointConfig :  IConfigureThisEndpoint, AsA_Publisher, IWantCustomInitialization
    {
        public void Init()
        {
            Configure.With()
                //this overrides the NServiceBus default convention of IEvent
                .DefiningEventsAs(t => t.Namespace != null && t.Namespace.StartsWith("Messages"));}
    }
}
