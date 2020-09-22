using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SessionControl.Controllers;
using StackExchange.Redis;

namespace SessionControl.SignalR
{
    [Route("api/signalr")]
    [ApiController]
    public class SignalRController : ControllerBase
    {
        private readonly IHubContext<SessionHub> sessionHub;
        private readonly SessionsController cntrl;
        public long RowsNum;

        public SignalRController(IHubContext<SessionHub> sessionHub, SessionsController cntrl)
        {
            this.sessionHub = sessionHub;
            this.cntrl = cntrl;
        }

        public IActionResult Get()
        {
            cntrl.CacheChanged += c_CacheChanged;
            sessionHub.Clients.All.SendAsync("ShowNumber", RowsNum);
            return Ok(new { Message = "Request Completed" });
        }

        //event handler
        public static void c_CacheChanged(object sender, CacheChangedEventArgs e)
        {

        }



    }
}