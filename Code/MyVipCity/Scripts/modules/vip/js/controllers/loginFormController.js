define(['vip/js/vip', 'jquery'], function (vip, jQuery) {
	'use strict';

	vip.controller('vip.loginFormController', ['$scope', '$http', 'vipServerErrorProcessorService', '$window', function ($scope, $http, vipServerErrorProcessorService, $window) {
		$scope.loading = false;
		// default state of login model
		$scope.loginModel = {
			RememberMe: false
		};

		$scope.submitLogin = function (e) {
			// prevent default submit behavior
			e.preventDefault();

			$scope.loading = true;
			// post login form
			$http.post('/Account/Login', jQuery.param($scope.loginModel), {
				headers: {
					'Content-Type': 'application/x-www-form-urlencoded'
				}
			}).then(function () {
				var avoidRoutes = ["ResetPasswordConfirmation", "ConfirmEmail"];
				for (var i = 0; i < avoidRoutes.length; i++) {
					if ($window.location.pathname.toLowerCase().indexOf(avoidRoutes[i].toLowerCase()) > -1) {
						$window.location = $window.location.origin;
						return;
					}
				}
				// if successful login, then reload the page
				$window.location.reload();
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