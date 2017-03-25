define(['vip/js/vip', 'sweet-alert'], function (vip, swal) {
	'use strict';

	vip.controller('vip.viewBusinessController', ['$scope', '$routeParams', '$http', 'vipConfig', '$route', 'vipUserService', function ($scope, $routeParams, $http, vipConfig, $route, vipUserService) {
		$scope.renderingMode = vip.renderingModes.read;
		$scope.model = {};

		if (vipUserService.isAdmin()) {
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

			$scope.save = function () {
				$http.put('api/Business', $scope.model)
				.then(function () {
					// let the user know save was successful
					swal({
						type: 'success',
						title: 'Success!',
						text: 'The business has been updated successfully',
						confirmButtonText: "Ok"
					}).then(function () {
						$scope.$apply(function () {
							// navigate to the club
							$route.reload();
						});
					}, function () {
						// setNewBusinessModel();
						$route.reload();
					});
				}, function () {
					swal('Oops!', 'Something went wrong!', 'error');
				});
			};
		}

		var friendlyId = $routeParams.friendlyId;
		$http.get('/api/Business/ByFriendlyId/' + friendlyId).then(function (response) {
			$scope.model = response.data;
		}, function (error) {
		});
	}]);
});