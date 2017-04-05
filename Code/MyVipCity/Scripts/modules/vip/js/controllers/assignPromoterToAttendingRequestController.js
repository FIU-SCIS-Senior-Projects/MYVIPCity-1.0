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
						text: 'The selected promoter has been assigned to this request, and a notification email has been sent to the promoter with the details. You will receive an email once the promoter either accepts or decline the request.',
						confirmButtonText: "Ok",
						showCancelButton: false
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