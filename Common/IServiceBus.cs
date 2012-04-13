using System;
using System.Collections.Generic;
using System.Linq;

namespace Common
{
    public interface IServiceBus
    {
        void Publish<T>(T message);
    }

    public class ServiceBus : IServiceBus
    {
        private readonly IList<KeyValuePair<Type, Action<dynamic>>> _subscribers = 
            new List<KeyValuePair<Type, Action<dynamic>>>();

        public void Publish<T>(T message)
        {
            var subscribers = _subscribers.Where(x => x.Key == typeof (T)).Select(x => x.Value);

            foreach (var subscriber in subscribers)
            {
                subscriber(message);
            }
        }

        public void Subscribe<T>(Action<dynamic> action)
        {
            _subscribers.Add(new KeyValuePair<Type, Action<dynamic>>(typeof(T), action));
        }
    }
}
