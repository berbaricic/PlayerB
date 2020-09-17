using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SessionControl.SignalR
{
    public class SessionHub : Hub
    {
        //one-way communication (from  server to client) -> so we don’t need any methods inside it, YET
    }
}
