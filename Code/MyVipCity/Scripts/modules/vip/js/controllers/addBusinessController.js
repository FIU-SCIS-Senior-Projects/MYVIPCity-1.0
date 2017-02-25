define(['vip/js/vip', 'sweet-alert'], function (vip, swal) {
	'use strict';

	vip.controller('vip.addBusinessController', ['$scope', 'vipFactoryService', '$q', '$http', 'vipNavigationService', '$route', function ($scope, vipFactoryService, $q, $http, vipNavigationService, $route) {
		// TODO Remove this button
		$scope.showToggleButton = true;
		$scope.showResetButton = true;
		$scope.showSaveButton = true;
		$scope.renderingMode = vip.renderingModes.edit;
		$scope.model = {};

		var setNewBusinessModel = function () {
			$q.when(vipFactoryService('business')).then(function (newBusiness) {
				$scope.model = newBusiness;
			});
		};

		setNewBusinessModel();

		$scope.resetForm = function () {
			setNewBusinessModel();
			// $route.reload();
		};

		$scope.save = function () {
			$http.post('api/Business', $scope.model)
				.then(function (response) {
					// get the response
					var business = response.data;
					// let the user know save was successful, and prompt if want to display the view mode for this club
					swal({
						type: 'success',
						title: 'Club created successfully!',
						text: 'Do you want to view the club?',
						showCancelButton: true,
						confirmButtonText: "Yes",
						cancelButtonText: "No"
					}).then(function () {
						$scope.$apply(function() {
							// navigate to the club
							vipNavigationService.goToClub(business.FriendlyId);
						});
					}, function () {
						// setNewBusinessModel();
						$route.reload();
					});
				}, function (error) {
					swal('Oops!', 'Something went wrong!', 'error');
				});
		};

		$scope.toggleRenderingMode = function () {
			$scope.renderingMode = $scope.renderingMode === vip.renderingModes.edit ? vip.renderingModes.read : vip.renderingModes.edit;
		};
	}]);
});