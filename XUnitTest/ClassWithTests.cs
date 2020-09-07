using Microsoft.AspNetCore.Authorization;
using Moq;
using SessionControl.Models;
using System;
using System.IO;
using System.Net;
using System.Text;
using Xunit;

namespace XUnitTest
{
    public class ClassWithTests
    {
        private readonly Weather weather;

        public ClassWithTests()
        {
            weather = new Weather();
        }
        [Fact]
       public void MethodTest_ArgumentChecking()
        {
            weather.MethodTest("Zagreb", null);
        }

        [Fact]
        public void MethodTest_LocationChecking()
        {
            weather.MethodTest("Lond4on", "ebfthg3z3535");
        }

        [Fact]
        public void MethodTest_RequestTesting()
        {
            // arrange
            var expected = "Lijepo suncano";
            var expectedBytes = Encoding.UTF8.GetBytes(expected);

            var responseStream = new MemoryStream();
            responseStream.Write(expectedBytes, 0, expectedBytes.Length);
            responseStream.Seek(0, SeekOrigin.Begin);

            var response = new Mock<HttpWebResponse>();
            response.Setup(c => c.GetResponseStream()).Returns(responseStream);

            var request = new Mock<HttpWebRequest>();
            request.Setup(c => c.GetResponse()).Returns(response.Object);

            var factory = new Mock<IHttpWebRequestFactory>();
            factory.Setup(c => c.Create(It.IsAny<string>()))
                .Returns(request.Object);

            // act
            var actualRequest = factory.Object.Create("http://api.openweathermap.org/data/2.5/weather?q=" +
                    "London&apikey=014164a1cbf071eb1be572e3564ef8f0");
            actualRequest.Method = WebRequestMethods.Http.Get;

            string actual;

            using (var httpWebResponse = (HttpWebResponse)actualRequest.GetResponse())
            {
                using (var streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    actual = streamReader.ReadToEnd();
                }
            }
            // assert
            Assert.Equal(expected, actual);
        }
    }
}
