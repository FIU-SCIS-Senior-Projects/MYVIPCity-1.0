define(['angular', 'angular-resource', 'angular-messages', 'angular-animate', 'angular-route', 'angular-sanitize'], function (angular) {
	'use strict';

	var vipModule = angular.module('vip', ['ng', 'ngResource', 'ngMessages', 'ngAnimate', 'ngRoute', 'ngSanitize']);

	vipModule.renderingModes = {
		read: 1,
		edit: 2
	};

	return vipModule;
});