using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using RabbitMqEventBus;

namespace SignalrService
{
    public class CacheSizeChangedIntegrationEventHandler : IIntegrationEventHandler<CacheSizeChangedIntegrationEvent>
    {
        private readonly IHubContext<SessionHub> sessionHub;

        public CacheSizeChangedIntegrationEventHandler(IHubContext<SessionHub> sessionHub)
        {
            this.sessionHub = sessionHub;
        }
        public async Task Handle(CacheSizeChangedIntegrationEvent @event)
        {
            await sessionHub.Clients.All.SendAsync("ShowNumber", @event.NumberOfRows.ToString());
        }
    }
}
