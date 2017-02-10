﻿define(['vip/js/vip', 'jquery'], function (vip, jQuery) {
	'use strict';

	vip.controller('vip.registerFormController', ['$scope', '$http', 'vipServerErrorProcessorService', '$window', function ($scope, $http, vipServerErrorProcessorService, $window) {

		// default state of register model
		$scope.registerModel = {
		};

		$scope.submitRegister = function (e) {
			// prevent default submit behavior
			e.preventDefault();

			// post login form
			$http.post('Account/Register', jQuery.param($scope.registerModel), {
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