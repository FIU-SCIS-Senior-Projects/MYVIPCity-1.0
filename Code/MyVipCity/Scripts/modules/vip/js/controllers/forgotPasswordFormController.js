define(['vip/js/vip', 'jquery', 'sweet-alert'], function (vip, jQuery, swal) {
	'use strict';

	vip.controller('vip.forgotPasswordFormController', ['$scope', '$http', 'vipServerErrorProcessorService', '$window', function ($scope, $http, vipServerErrorProcessorService, $window) {
		$scope.loading = false;
		// default state of forgotPasswordModel
		$scope.forgotPasswordModel = {
		};

		$scope.submitForgotPassword = function (e) {
			// prevent default submit behavior
			e.preventDefault();
			$scope.loading = true;
			// post login form
			$http.post('/Account/ForgotPassword', jQuery.param($scope.forgotPasswordModel), {
				headers: {
					'Content-Type': 'application/x-www-form-urlencoded'
				}
			}).then(function (response) {
				// let the user know the registration was successful
				swal('Awesome!', response.data, 'success');
			},
				function (error) {
					// login failed, show the error
					$scope.serverError = vipServerErrorProcessorService(error.data);
				})['finally'](function () {
					$scope.loading = false;
				});
		};
	}]);
});