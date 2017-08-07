using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Models;

namespace MessageBus
{
    public static class MessageBusFactory
    {
        public static IMessageBus GetMassTransitMessageBus(MessageHandler<ExecutingContext> responseHandler = null)
        {
                var messageBusUrl = ConfigurationManager.AppSettings["messageBusUrl"];
                var messageBusUserName = ConfigurationManager.AppSettings["messageBusUserName"];
                var messageBusUserPassword = ConfigurationManager.AppSettings["messageBusUserPassword"];
                var messageQueueName = ConfigurationManager.AppSettings["messageQueueName"];

                var busControl = Bus.Factory.CreateUsingRabbitMq(config =>
                {
                    var host = config.Host(new Uri(messageBusUrl), h =>
                    {
                        h.Username(messageBusUserName);
                        h.Password(messageBusUserPassword);
                    });

                    config.PurgeOnStartup = true;
                    config.Durable = true;
                    if (responseHandler != null)
                    {
                        config.ReceiveEndpoint(host, messageQueueName, ep =>
                        {
                            ep.Handler<ExecutingContext>(responseHandler);
                        });
                    }
                });

                return new MassTransitMessageBus(busControl);
        }
    }
}
