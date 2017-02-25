define(['vip/js/vip'], function (vip) {
	'use strict';

	vip.controller('vip.viewBusinessController', ['$scope', '$routeParams', '$http', 'vipConfig', function ($scope, $routeParams, $http, vipConfig) {
		$scope.renderingMode = vip.renderingModes.read;
		$scope.model = {};

		if (vipConfig && vipConfig.Roles && vipConfig.Roles.indexOf("Admin") > -1) {
			$scope.showEditButton = true;
			$scope.showReadModeButton = false;
			$scope.showSaveButton = false;

			$scope.editClick = function () {
				$scope.renderingMode = vip.renderingModes.edit;
				$scope.showReadModeButton = true;
				$scope.showEditButton = false;
				$scope.showSaveButton = true;
			};

			$scope.readModeClick = function () {
				$scope.renderingMode = vip.renderingModes.read;
				$scope.showReadModeButton = false;
				$scope.showEditButton = true;
				$scope.showSaveButton = false;
			};
		}

		var friendlyId = $routeParams.friendlyId;
		$http.get('/api/Business/ByFriendlyId/' + friendlyId).then(function (response) {
			$scope.model = response.data;
		}, function (error) {
		});
	}]);
});