define(['vip/js/vip', 'jquery'], function (vip, jQuery) {
	'use strict';

	vip.controller('vip.loginFormController', ['$scope', '$http', 'vipServerErrorProcessorService', '$window', function ($scope, $http, vipServerErrorProcessorService, $window) {

		// default state of login model
		$scope.loginModel = {
			RememberMe: false
		};

		$scope.submitLogin = function (e) {
			// prevent default submit behavior
			e.preventDefault();

			// post login form
			$http.post('Account/Login', jQuery.param($scope.loginModel), {
				headers: {
					'Content-Type': 'application/x-www-form-urlencoded'
				}
			}).then(function () {
				// if success login, then reload the page
				$window.location.reload();
			},
				function (error) {
					// login failed, show the error
					$scope.serverError = vipServerErrorProcessorService(error.data);
				});
		};
	}]);
});