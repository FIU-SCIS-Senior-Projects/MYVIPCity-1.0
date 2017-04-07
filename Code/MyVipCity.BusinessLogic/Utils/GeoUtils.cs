using System.Device.Location;

namespace MyVipCity.BusinessLogic.Utils {

	public static class GeoUtils {

		/// <summary>
		/// Returns the distance in meters between two geographical points.
		/// </summary>
		/// <param name="lat1">Latitude of the first point.</param>
		/// <param name="long1">Longitude of the first point.</param>
		/// <param name="lat2">Latitude of the second point.</param>
		/// <param name="long2">Longitude of the second point.</param>
		/// <returns></returns>
		public static double DistanceBetweenCoordinates(double lat1, double long1, double lat2, double long2) {
			var coord1 = new GeoCoordinate(lat1, long1);
			var coord2 = new GeoCoordinate(lat2, long2);
			var distanceInMeters = coord1.GetDistanceTo(coord2);
			return distanceInMeters;
		}
	}
}
