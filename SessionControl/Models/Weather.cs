using SessionControl.Exception;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SessionControl.Models
{
    public class Weather
    {
		public string Result;
		private HttpWebRequest request;
		private HttpWebResponse response;
		private Stream stream;

		public Weather()
		{

		}

		public Weather(HttpWebRequest request, HttpWebResponse response, Stream stream)
		{
			this.request = request;
			this.response = response;
			this.stream = stream;
		}

        public void MethodTest(string location, string apikey)
        {
			if (string.IsNullOrEmpty(location))
			{
				throw new ArgumentNullException();
			}
			if (string.IsNullOrEmpty(apikey))
			{
				throw new ArgumentNullException();
			}
			
			CheckLocation(location);

			var currentWeatherUrl = "http://api.openweathermap.org/data/2.5/weather?q=" +
				location + "&apikey=" + apikey;

			if (this.request == null)
			{
				throw new NotSupportedException();
			}
			else
			{
				request.AutomaticDecompression = DecompressionMethods.GZip;
				using (this.response)
				{
					if (response == null)
					{
						throw new WebException();
					}
					if (stream == null)
					{
						throw new ProtocolViolationException();
					}
					using (StreamReader streamReader = new StreamReader(stream))
					{
						Result = streamReader.ReadToEnd();
					}
				}
			}

		}

		public void CheckLocation(string loc)
		{
			bool containsDigit = loc.Any(char.IsDigit);
			if (containsDigit)
			{
				throw new NotAllowedValueException("Invalid location. Location cannot contain numbers");
			}
		}
    }
}
