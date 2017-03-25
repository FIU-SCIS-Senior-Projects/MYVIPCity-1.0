define(['vip/js/vip'], function (vip) {
	'use strict';

	vip.factory('vipNavigationService', ['$location', function ($location) {

		var changeLocation = function(url) {
			$location.path(url);
		};

		return {
			goToClub: function(friendlyId) {
				var url = '/view-business/' + friendlyId;
				changeLocation(url);
			}
		};
	}]);
});