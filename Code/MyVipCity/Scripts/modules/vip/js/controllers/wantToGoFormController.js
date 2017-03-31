define(['vip/js/vip', 'sweet-alert'], function (vip) {
	'use strict';

	vip.controller('vip.wantToGoFormController', ['$scope', '$http', function ($scope, $http) {
		$scope.attendingRequest = {};

		$scope.partyCountChanged = function() {
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
	}]);
});