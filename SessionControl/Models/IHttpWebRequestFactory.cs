using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SessionControl.Models
{
    public interface IHttpWebRequestFactory
    {
        HttpWebRequest Create(string uri);
    }
}
