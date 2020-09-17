using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace SessionControl.SignalR
{
    [Route("api/signalr")]
    [ApiController]
    public class SignalRController : ControllerBase
    {
        private readonly IHubContext<SessionHub> sessionHub;
        private readonly IDataManager dataManager;

        public SignalRController(IHubContext<SessionHub> sessionHub, IDataManager dataManager)
        {
            this.sessionHub = sessionHub;
            this.dataManager = dataManager;
        }

        public IActionResult Get()
        {
            var timerManager = new TimerManager(() => 
            sessionHub.Clients.All.SendAsync("ShowNumber", dataManager.GetNumberOfRows()));

            return Ok(new { Message = "Request Completed" });
        }
    }
}