define(['vip/js/vip'], function (vip) {
	'use strict';

	vip.factory('vipLocationService', ['$location', function ($location) {

		var changeLocation = function(url) {
			$location.path(url);
		};

		return {
			goToClub: function(friendlyId) {
				var url = '/viewClub/' + friendlyId;
				changeLocation(url);
			}
		};
	}]);
});