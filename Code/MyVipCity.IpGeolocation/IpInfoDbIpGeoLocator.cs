using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace MyVipCity.IpGeolocation {

	public class IpInfoDbIpGeoLocator: IIpGeolocator {

		private readonly string apiKey;

		public IpInfoDbIpGeoLocator(string apiKey) {
			this.apiKey = apiKey;
		}

		public IpLocation LocateIpAddress(string ipAddress) {
			var result = GetIpLocation(ipAddress).Result;
			return result;
		}

		public async Task<IpLocation> LocateIpAddressAsync(string ipAddress) {
			return await GetIpLocation(ipAddress);
		}

		private async Task<IpLocation> GetIpLocation(string ipAddress) {
			var jsonString = await GetJsonLocation(ipAddress);
			var jObj = JObject.Parse(jsonString);
			var result = new IpLocation {
				City = jObj["cityName"].Value<string>(),
				Country = jObj["countryName"].Value<string>(),
				CountryCode = jObj["countryCode"].Value<string>(),
				IpAddress = jObj["ipAddress"].Value<string>(),
				Region = jObj["regionName"].Value<string>(),
				TimeZone = jObj["timeZone"].Value<string>(),
				ZipCode = jObj["zipCode"].Value<string>(),
				Latitude = jObj["latitude"].Value<decimal>(),
				Longitude = jObj["longitude"].Value<decimal>()
			};
			return result;
		}

		private async Task<string> GetJsonLocation(string ipAddress) {
			var url = $"http://api.ipinfodb.com/v3/ip-city/?key={apiKey}&ip={ipAddress}&format=json";
			var httpClient = new HttpClient();
			var jsonString = await httpClient.GetStringAsync(url);
			return jsonString;
		}
	}
}
