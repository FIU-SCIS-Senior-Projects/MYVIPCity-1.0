define(['vip/js/vip', 'sweet-alert'], function (vip, swal) {
	'use strict';

	vip.controller('vip.assignPromoterToAttendingRequestController', ['$scope', '$http', '$window', function ($scope, $http, $window) {

		var showErrorPopup = function (title, errorMsg) {
			swal(title || 'Oops', errorMsg || 'An error has occurred', 'error');
		};

		$scope.assignPromoter = function () {
			var url = 'api/AttendingRequest/AssignPromoter/' + $scope.requestId + '/' + $scope.promoter.Id;
			$http.post(url).then(function (response) {
				var result = response.data;
				if (!result || !result.Result) {
					showErrorPopup();
				}
				else {
					swal({
						type: 'success',
						title: 'Success',
						text: 'A notification email has been sent to the selected promoter with the details of the request.',
						confirmButtonText: "Ok",
						showCancelButton: true
					}).then(function () {
						$window.location = $window.location.origin;
					}, angular.noop);
				}
			});
		};

		$scope.declineAttendingRequest = function () {

		};
	}]);
});