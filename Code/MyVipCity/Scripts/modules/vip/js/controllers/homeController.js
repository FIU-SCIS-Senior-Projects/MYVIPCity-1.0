define(['vip/js/vip', 'angular'], function (vip, angular) {
	'use strict';

	vip.controller('vip.homeController', ['$scope', '$http', '$cookies', '$timeout', 'vipBusinessService', function ($scope, $http, $cookies, $timeout, vipBusinessService) {
		var top = 10;

		$scope.businesses = [];
		$scope.loading = false;

		var location = $cookies.getObject('ipLocation');

		var processBusinesses = function (businesses) {
			vipBusinessService.addFirstPictureUrl(businesses);
			return businesses;
		};

		var timeoutPromise = null;

		// executes a search
		$scope.search = function (loadMore) {
			// indicate that data is being loaded
			$scope.loading = true;
			// if there is a timeout promise, cancel it
			if (timeoutPromise)
				$timeout.cancel(timeoutPromise);
			// build the search criteria
			var searchCriteria = {
				top: top
			};

			// if we know the location of the user, include it in the search
			if (location) {
				searchCriteria.longitude = location.Longitude;
				searchCriteria.latitude = location.Latitude;
			}
			// include text search criteri
			if ($scope.searchText) {
				searchCriteria.criteria = $scope.searchText;
			}
			// check if we are loading more
			if (loadMore)
				searchCriteria.skip = $scope.businesses.length;

			// excute the search
			vipBusinessService.search(searchCriteria).then(function (businesses) {
				// process the results
				var searchResult = processBusinesses(businesses);
				// check if we are loading more data, and therefore it needs to be appended to the existing list
				if (loadMore) {
					// only add items not already in the list
					var map = {};
					angular.forEach($scope.businesses, function (b) {
						map[b.Id] = true;
					});
					angular.forEach(searchResult, function (result) {
						if (!map[result.Id])
							$scope.businesses.push(result);
					});
				}
				else {
					$scope.businesses = searchResult;
				}
			})['finally'](function () {
				// indicate loading has finished
				$scope.loading = false;
			});
		};

		timeoutPromise = $timeout(function () {
			$scope.search();
		}, 1000);

		$scope.criteriaChanged = function () {
			if (timeoutPromise)
				$timeout.cancel(timeoutPromise);
			var timeoutTime = $scope.searchText ? 5000 : 700;
			timeoutPromise = $timeout(function () {
				$scope.search();
			}, timeoutTime);
		};

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

				$scope.search();
			});
		}
	}]);
});