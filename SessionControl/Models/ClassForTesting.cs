using SessionControl.Exception;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SessionControl.Models
{
    public class ClassForTesting
    {
        public void MethodTest(string location, string apikey)
        {
			try
			{
				string result;
				bool validLocation = CheckLocation(location);

				var currentWeatherUrl = "http://api.openweathermap.org/data/2.5/weather?q=" +
					location + "&apikey=" + apikey;
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(currentWeatherUrl);
				request.AutomaticDecompression = DecompressionMethods.GZip;

				using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
				using (Stream stream = response.GetResponseStream())
				using (StreamReader reader = new StreamReader(stream))
				{
					result = reader.ReadToEnd();
				}

			}
			catch (ArgumentNullException)
			{
			}
			catch (NotAllowedValueException)
			{
			}
			catch (WebException)
			{
			}
			
        }

		public bool CheckLocation(string loc)
		{
			bool containsDigit = loc.Any(char.IsDigit);
			if (containsDigit)
			{
				throw new NotAllowedValueException("Invalid location. Location cannot contain numbers");
			}
			return true;
		}
    }
}
