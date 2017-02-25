define(['vip/js/vip', 'sweet-alert'], function (vip, swal) {
	'use strict';

	vip.controller('vip.addBusinessController', ['$scope', 'vipFactoryService', '$q', '$http', 'vipLocationService', function ($scope, vipFactoryService, $q, $http, vipLocationService) {
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

		$scope.resetForm = function() {
			setNewBusinessModel();
		};

		$scope.save = function () {
			$http.post('api/Business', $scope.model)
				.then(function (response) {
					var business = response.data;
					// let the user know the registration was successful
					swal({
						type: 'success',
						title: 'Club created successfully!',
						text: 'Do you want to view the club?',
						showCancelButton: true,
						confirmButtonText: "Yes",
						cancelButtonText: "No"
					}).then(function () {
						vipLocationService.goToClub(business.FriendlyId);
					}, setNewBusinessModel);
				}, function (error) {

				});
		};

		$scope.toggleRenderingMode = function () {
			$scope.renderingMode = $scope.renderingMode === vip.renderingModes.edit ? vip.renderingModes.read : vip.renderingModes.edit;
		};
	}]);
});