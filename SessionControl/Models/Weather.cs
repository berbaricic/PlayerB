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
		private HttpWebRequest request;
		private HttpWebResponse response;

		public Weather()
		{

		}

		public Weather(IHttpWebRequestFactory factory, HttpWebRequest request, HttpWebResponse response)
		{
			this.factory = factory;
			this.request = request;
			this.response = response;
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

			this.request = this.factory.Create(currentWeatherUrl);

			if (this.request == null)
			{
				throw new NotSupportedException();
			}
			else
			{
				this.request.AutomaticDecompression = DecompressionMethods.GZip;
				using (this.response = (HttpWebResponse)this.request.GetResponse())
				{
					if (this.response == null)
					{
						throw new WebException();
					}				
					using (StreamReader streamReader = new StreamReader(this.response.GetResponseStream()))
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
