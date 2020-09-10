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
		private IHttpWebRequestFactory factory;

		public Weather()
		{

		}

		public Weather(IHttpWebRequestFactory factory)
		{
			this.factory = factory;
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

			 var request = this.factory.Create(currentWeatherUrl);

			if (request == null)
			{
				throw new NotSupportedException();
			}
			else
			{
				request.AutomaticDecompression = DecompressionMethods.GZip;
				using (var response = (HttpWebResponse)request.GetResponse())
				{
					if (response == null)
					{
						throw new WebException();
					}				
					using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
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
