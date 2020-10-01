using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ;
using RabbitMqEventBus;

namespace RabbitMqEventBus
{
    public class CacheSizeChangedIntegrationEvent : IntegrationEvent
    {
        public long NumberOfRows { get; set; }

        public CacheSizeChangedIntegrationEvent(long number)
        {
            NumberOfRows = number;
        }
    }
}
