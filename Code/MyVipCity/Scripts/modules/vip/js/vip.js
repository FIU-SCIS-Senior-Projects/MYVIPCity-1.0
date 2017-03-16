define(['angular', 'angular-resource', 'angular-messages', 'angular-animate', 'angular-route', 'angular-sanitize', 'angular-material', 'ng-on'], function (angular) {
	'use strict';

	var vipModule = angular.module('vip', ['ng', 'ngResource', 'ngMessages', 'ngAnimate', 'ngRoute', 'ngSanitize', 'ngMaterial', 'argshook.ngOn']);

	vipModule.renderingModes = {
		read: 1,
		edit: 2
	};

	return vipModule;
});