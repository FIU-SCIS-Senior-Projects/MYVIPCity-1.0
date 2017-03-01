define(['vip/js/vip', 'sweet-alert'], function (vip, swal) {
	'use strict';

	vip.controller('vip.viewBusinessController', ['$scope', '$routeParams', '$http', 'vipConfig', '$route', 'vipNavigationService', function ($scope, $routeParams, $http, vipConfig, $route, vipNavigationService) {
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

			$scope.save = function() {
				$http.put('api/Business', $scope.model)
				.then(function (response) {
					// get the response
					var business = response.data;
					// let the user know save was successful, and prompt if want to display the view mode for this club
					swal({
						type: 'success',
						title: 'Success!',
						text: 'The club has been updated successfully',
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
				}, function (error) {
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