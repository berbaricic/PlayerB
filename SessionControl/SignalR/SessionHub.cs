using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using SessionControl.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SessionControl.SignalR
{
    public class SessionHub : Hub
    {
        //public delegate void CacheChangeHandler(object cache, CacheChangedEventArgs args);
        //public CacheChangeHandler OnCacheChange;
        //public SessionHub()
        //{
        //    OnCacheChange += c_CacheChanged;
        //}
        ////one-way communication (from  server to client) -> so we don’t need any methods inside it, YET
        //public void c_CacheChanged(object sender, CacheChangedEventArgs e)
        //{
        //    var test = e.NumberOfRows;
        //    this.Clients.All.SendAsync("ShowNumber", test);
        //}
    }
}
