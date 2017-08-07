using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;

namespace MessageBus
{
    public class MassTransitMessageBus : IMessageBus
    {
        private readonly IBusControl _busControl;

        public MassTransitMessageBus(IBusControl busControl)
        {
            _busControl = busControl;
        }

        public void Start()
        {
            _busControl.Start();
        }

        public void Stop()
        {
            _busControl.Stop();
        }

        public Task Publish<TMessage>(TMessage message) where TMessage : class
        {
            return _busControl.Publish<TMessage>(message);
        }
    }
}
