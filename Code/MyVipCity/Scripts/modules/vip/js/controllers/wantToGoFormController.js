define(['vip/js/vip', 'sweet-alert'], function (vip) {
	'use strict';

	vip.controller('vip.wantToGoFormController', ['$scope', '$http', function ($scope, $http) {
		var listeners = [];

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

		$scope.submitRequest = function () {
			$http.post('api/AttendingRequest', $scope.attendingRequest).then(function (response) {
				var x = 1;
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