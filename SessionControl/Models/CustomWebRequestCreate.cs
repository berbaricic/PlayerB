using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SessionControl.Models
{
    public class CustomWebRequestCreate
    {
        private static WebRequest nextRequest;

        private static object lockObject = new object();

        public static WebRequest NextRequest
        {
            get
            {
                return nextRequest;
            }

            set
            {
                lock (lockObject)
                {
                    nextRequest = value;
                }
            }
        }

        public WebRequest Create(Uri uri)
        {
            return nextRequest;
        }

        /// <summary>
        /// Creates a Mock Http Web request
        /// </summary>
        /// <returns>The mocked HttpRequest object</returns>
        public static HttpWebRequest CreateMockHttpWebRequestWithGivenResponseCode(HttpStatusCode httpStatusCode)
        {
            var response = new Mock<HttpWebResponse>(MockBehavior.Loose);
            response.Setup(c => c.StatusCode).Returns(httpStatusCode);

            var request = new Mock<HttpWebRequest>();
            request.Setup(s => s.GetResponse()).Returns(response.Object);
            NextRequest = request.Object;
            return request.Object;
        }
    }
}
