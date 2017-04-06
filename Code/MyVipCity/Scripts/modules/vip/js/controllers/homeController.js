define(['vip/js/vip', 'angular'], function (vip, angular) {
	'use strict';

	vip.controller('vip.homeController', ['$scope', '$http', '$cookies', '$timeout', 'vipBusinessService', function ($scope, $http, $cookies, $timeout, vipBusinessService) {
		$scope.clubs = [];

		var location = $cookies.getObject('ipLocation');

		var processBusinesses = function(businesses) {
			angular.forEach(businesses, function (b) {
				// sort the pictures by Index and take the first one
				b.Pictures.sort(function (a, b) {
					return a.Index - b.Index;
				});
				b.firstPictureUrl = 'api/Pictures/' + b.Pictures[0].BinaryDataId;
			});

			return businesses;
		};

		// executes a search
		var search = function () {
			var searchCriteria = {};

			if (location) {
				searchCriteria.longitude = location.Longitude;
				searchCriteria.latitude = location.Latitude;
			}
			if ($scope.textCriteria) {
				searchCriteria.criteria = $scope.textCriteria;
			}

			vipBusinessService.search(searchCriteria).then(function (businesses) {

				$scope.businesses = processBusinesses(businesses);
			});
		};

		var timeoutPromise = $timeout(function () {
			search();
		}, 2000);

		// check if there is geolocation functionality available in the browser
		if (navigator.geolocation) {
			// ask for the position
			navigator.geolocation.getCurrentPosition(function (position) {
				// use more accurate location
				location = angular.extend(location || {}, {
					Longitude: position.coords.longitude,
					Latitude: position.coords.latitude
				});

				$timeout.cancel(timeoutPromise);

				search();
			});
		}
	}]);
});