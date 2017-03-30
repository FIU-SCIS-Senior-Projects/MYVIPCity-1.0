using System.Threading.Tasks;

namespace MyVipCity.IpGeolocation {

	public interface IIpGeolocator {

		Task<IpLocation> LocateIpAddressAsync(string ipAddress);

		IpLocation LocateIpAddress(string ipAddress);
	}
}
