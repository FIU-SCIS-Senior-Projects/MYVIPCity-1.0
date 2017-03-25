﻿define(['vip/js/vip'], function (vip) {
	'use strict';

	vip.controller('vip.mainController', ['$scope', 'vipPageLoaderService', 'vipUserService', function ($scope, vipPageLoaderService, vipUserService) {
		var listeners = [];

		listeners.push($scope.$on('$routeChangeStart', function (e) {
			vipPageLoaderService.showPageLoader();
		}));

		listeners.push($scope.$on('$routeChangeSuccess', function (e) {
			vipPageLoaderService.hidePageLoader();
		}));

		listeners.push($scope.$on('$destroy', function () {
			// unregister listeners
			for (var i = 0; i < listeners.length; i++)
				listeners[i]();
		}));

		$scope.userIsAdmin = vipUserService.isAdmin();
	}]);
});