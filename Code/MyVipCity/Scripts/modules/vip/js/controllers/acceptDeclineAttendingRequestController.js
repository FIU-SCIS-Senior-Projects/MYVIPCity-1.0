define(['vip/js/vip', 'sweet-alert'], function (vip, swal) {
	'use strict';

	vip.controller('vip.acceptDeclineAttendingRequestController', ['$scope', '$http', '$window', function ($scope, $http, $window) {

		var showErrorPopup = function (title, errorMsg) {
			swal(title || 'Oops', errorMsg || 'An error has occurred', 'error');
		};

		$scope.acceptAttendingRequest = function () {
			$http.post('api/AttendingRequest/Accept/' + $scope.requestId).then(function (response) {
				var result = response.data;
				if (!result) {
					showErrorPopup();
				}
				else {
					swal({
						type: 'success',
						title: 'Success',
						text: 'Thank you! An email has ben sent to the user notifying that the request has been accepted.',
						confirmButtonText: "Ok",
						showCancelButton: false
					}).then(function () {
						$window.location = $window.location.origin;
					}, angular.noop);
				}
			}, function() {
				showErrorPopup();
			});
		};

		$scope.declineAttendingRequest = function () {
			$http.post('api/AttendingRequest/DeclineByPromoter/' + $scope.requestId).then(function (response) {
				var result = response.data;
				if (!result) {
					showErrorPopup();
				}
				else {
					swal({
						type: 'success',
						title: 'Success',
						text: 'The request has been declined',
						confirmButtonText: "Ok",
						showCancelButton: false
					}).then(function () {
						$window.location = $window.location.origin;
					}, angular.noop);
				}
			}, function () {
				showErrorPopup();
			});
		};
	}]);
});