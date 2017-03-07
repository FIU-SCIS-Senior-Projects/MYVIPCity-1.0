define(['vip/js/vip'], function (vip) {
	'use strict';

	vip.controller('vip.promoterProfileController', ['$scope', '$routeParams', '$http', '$location', function ($scope, $routeParams, $http, $location) {
		// set rendering mode to read mode
		$scope.renderingMode = vip.renderingModes.read;
		// get the id of the profile from the route parameters
		var promoterProfileId = $routeParams.promoterProfileId;
		// set empty model
		$scope.model = {};
		// make request to get the profile info
		$http.get('api/PromoterProfile/' + promoterProfileId).then(function (response) {
			// if no response data then redirect to home
			if (!response.data) {
				$location.path('/');
			}
			else {
				// update the model
				$scope.model = response.data;
				$scope.model.AverageRating = 5;
			}
		});

		$scope.toggleRenderingMode = function () {
			if ($scope.renderingMode === vip.renderingModes.read)
				$scope.renderingMode = vip.renderingModes.edit;
			else
				$scope.renderingMode = vip.renderingModes.read;
		};
	}]);
});