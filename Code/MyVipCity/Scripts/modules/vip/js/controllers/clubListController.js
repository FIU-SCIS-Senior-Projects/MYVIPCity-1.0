define(['vip/js/vip', 'sweet-alert'], function (vip) {
	'use strict';

	vip.controller('vip.clubListController', ['$scope', '$http', function ($scope, $http) {
		$scope.clubs = [];

		$http.get('api/Business').then(function(response) {
			$scope.clubs = response.data;
		});
	}]);
});