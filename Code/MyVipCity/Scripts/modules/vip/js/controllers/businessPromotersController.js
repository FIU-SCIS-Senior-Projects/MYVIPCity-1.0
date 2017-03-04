define(['vip/js/vip', 'angular', 'sweet-alert'], function (vip, angular, swal) {
	'use strict';

	vip.controller('vip.businessPromotersController', ['$scope', '$http', function ($scope, $http) {
		$scope.newPromoters = [];

		$scope.sendInvitation = function() {
			angular.forEach($scope.newPromoters, function(newPromoter) {
				newPromoter.Id = 0;
				newPromoter.ClubFriendlyId = $scope.model.FriendlyId;
			});

			$http.post('api/Business/SendPromoterInvitation', $scope.newPromoters).then(function (response) {
				$scope.newPromoters = [];
				swal('Invitation sent!', 'An invitation email has been sent to each new promoter', 'success');
			}, function(error) {
				
			});
		};
	}]);
});