define(['vip/js/vip', 'moment', 'sweet-alert'], function (vip, moment, swal) {
	'use strict';

	vip.controller('vip.wantToGoFormController', ['$scope', '$http', function ($scope, $http) {
		var listeners = [];

		this.minDate = new Date();
		this.maxDate = moment(this.minDate).add(60, 'd').toDate();
		// TODO: filter dates so that it is not possible to select a day where the business is closed

		$scope.attendingRequest = {};

		$scope.partyCountChanged = function () {
			delete $scope.attendingRequest.MaleCount;
			delete $scope.attendingRequest.FemaleCount;
		};

		$scope.maleCountChanged = function () {
			if ($scope.attendingRequest.MaleCount > $scope.attendingRequest.PartyCount || !$scope.attendingRequest.PartyCount)
				$scope.attendingRequest.PartyCount = $scope.attendingRequest.MaleCount;
			$scope.attendingRequest.FemaleCount = $scope.attendingRequest.PartyCount - $scope.attendingRequest.MaleCount;
		};

		$scope.femaleCountChanged = function () {
			if ($scope.attendingRequest.FemaleCount > $scope.attendingRequest.PartyCount || !$scope.attendingRequest.PartyCount)
				$scope.attendingRequest.PartyCount = $scope.attendingRequest.FemaleCount;
			$scope.attendingRequest.MaleCount = $scope.attendingRequest.PartyCount - $scope.attendingRequest.FemaleCount;
		};

		var showErrorPopup = function (title, errorMsg) {
			swal(title || 'Oops', errorMsg || 'An error has occurred', 'error');
		};

		$scope.submitRequest = function () {
			$http.post('api/AttendingRequest', $scope.attendingRequest).then(function (response) {
				var result = response.data;
				if (!result || !result.Result) {
					showErrorPopup();
				}
				else {
					swal({
						type: 'success',
						title: 'Success',
						text: 'Your request has been submitted. You will receive an email as soon as we process your request.',
						confirmButtonText: "Ok",
						showCancelButton: false
					}).then(function () {
						$scope.$applyAsync(function () {
							$scope.attendingRequest = {};
						});
					}, angular.noop);
				}
			});
		};

		listeners.push($scope.$watch('model', function (business) {
			// only store the business Id
			$scope.attendingRequest.Business = { Id: business.Id };
		}));

		listeners.push($scope.$on('$destroy', function () {
			// unregister listeners
			for (var i = 0; i < listeners.length; i++)
				listeners[i]();
		}));
	}]);
});