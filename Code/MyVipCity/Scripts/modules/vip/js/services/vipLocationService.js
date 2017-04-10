define(['vip/js/vip'], function (vip) {
	'use strict';

	vip.factory('vipLocationService', ['vipApiKeys', '$http', function (vipApiKeys, $http) {
		var coordinatesMap = {};
		return {
			locateByCoordinates: function (latitude, longitude) {
				var key = latitude + ',' + longitude;
				var locationInfo = coordinatesMap[key];
				if (locationInfo)
					return locationInfo;

				return $http.get('https://maps.googleapis.com/maps/api/geocode/json?latlng=' + key + '&key=' + vipApiKeys.googleGeocoding).then(function (response) {
					return response.data;
				});
			}
		};
	}]);
});