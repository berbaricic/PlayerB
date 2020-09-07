using System.Net;

namespace XUnitTest
{
    public interface IHttpWebRequestFactory
    {
        HttpWebRequest Create(string uri);
    }
}