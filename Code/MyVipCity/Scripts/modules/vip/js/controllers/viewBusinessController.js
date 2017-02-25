define(['vip/js/vip'], function (vip) {
	'use strict';

	vip.controller('vip.viewBusinessController', ['$scope', '$routeParams', 'vipFactoryService', '$q', '$http', 'vipLocationService', function ($scope, $routeParams, vipFactoryService, $q, $http, vipLocationService) {
		$scope.renderingMode = vip.renderingModes.read;
		$scope.model = {};
		var friendlyId = $routeParams.friendlyId;
		$http.get('/api/Business/ByFriendlyId/' + friendlyId).then(function(response) {
			$scope.model = response.data;
		}, function(error) {
			
		});
	}]);
});