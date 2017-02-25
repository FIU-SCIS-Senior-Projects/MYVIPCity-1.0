define(['vip/js/vip'], function (vip) {
	'use strict';

	vip.controller('vip.viewBusinessController', ['$scope', '$routeParams', function ($scope, $routeParams) {
		$scope.renderingMode = vip.renderingModes.read;
		$scope.model = {};
		var friendlyId = $routeParams.friendlyId;
		$http.get('/api/Business/ByFriendlyId/' + friendlyId).then(function (response) {
			$scope.model = response.data;
		}, function (error) {
		});
	}]);
});