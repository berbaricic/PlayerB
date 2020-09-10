using Microsoft.AspNetCore.Authorization;
using Moq;
using SessionControl.Exception;
using SessionControl.Models;
using System;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Text;
using Xunit;

namespace XUnitTest
{
    public class MethodTest
    {
        Weather weather;

        [Fact]
       public void Ensure_ArgumentChecking_ExceptionThrown_arg0()
        {
            weather = new Weather();
            Assert.Throws<ArgumentNullException>(() => weather.MethodTest("Zagreb", null));
        }

        [Fact]
        public void Ensure_ArgumentChecking_ExceptionThrown_arg1()
        {
            weather = new Weather();
            Assert.Throws<ArgumentNullException>(() => weather.MethodTest(null, "014164a1cbf071eb1be572e3564ef8f0"));
        }

        [Fact]
        public void Ensure_LocationChecking_ExceptionThrown()
        {
            weather = new Weather();
            Assert.Throws<NotAllowedValueException>(() => weather.MethodTest("Zagreb01", "014164a1" ));
        }

        [Fact]
        public void Ensure_ResultIsNotNull()
        {
            // arrange
            var factory = new Mock<IHttpWebRequestFactory>();
            var httpRequest = new Mock<HttpWebRequest>();
            var httpResponse = new Mock<HttpWebResponse>();

            factory.Setup(c => c.Create(It.IsAny<string>())).Returns(httpRequest.Object);
            httpRequest.Setup(c => c.GetResponse()).Returns(httpResponse.Object);
            httpResponse.Setup(c => c.GetResponseStream()).Returns(new MemoryStream());

            //act
            var weather = new Weather(factory.Object);
            weather.MethodTest("Zagreb", "014164a1cbf071eb1be572e3564ef8f0");

            //assert
            Assert.NotNull(weather.Result);      
        }
        [Fact]
        public void Ensure_Request_ExceptionThrown()
        {
            // arrange
            var factory = new Mock<IHttpWebRequestFactory>();
            var httpRequest = new Mock<HttpWebRequest>();
            var httpResponse = new Mock<HttpWebResponse>();

            //factory.Setup(c => c.Create(It.IsAny<string>())).Returns(httpRequest.Object);
            httpRequest.Setup(c => c.GetResponse()).Returns(httpResponse.Object);
            httpResponse.Setup(c => c.GetResponseStream()).Returns(new MemoryStream());

            //act & assert
            var weather = new Weather(factory.Object);
            Assert.Throws<NotSupportedException>(() => weather.MethodTest("Zagreb", "014164a1cbf071eb1be572e3564ef8f0"));
        }

        [Fact]
        public void Ensure_Response_ExceptionThrown()
        {
            // arrange
            var factory = new Mock<IHttpWebRequestFactory>();
            var httpRequest = new Mock<HttpWebRequest>();
            var httpResponse = new Mock<HttpWebResponse>();

            factory.Setup(c => c.Create(It.IsAny<string>())).Returns(httpRequest.Object);
            //httpRequest.Setup(c => c.GetResponse()).Returns(httpResponse.Object);
            httpResponse.Setup(c => c.GetResponseStream()).Returns(new MemoryStream());

            //act & assert
            var weather = new Weather(factory.Object);
            Assert.Throws<WebException>(() => weather.MethodTest("Zagreb", "014164a1cbf071eb1be572e3564ef8f0"));
        }



    }
}
