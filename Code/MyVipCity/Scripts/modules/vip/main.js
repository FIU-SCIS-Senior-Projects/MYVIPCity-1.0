define([
	'require',
	'angular',
	'vip/js/vip',
	// controllers
	'vip/js/controllers/forgotPasswordFormController',
	'vip/js/controllers/loginFormController',
	'vip/js/controllers/registerFormController',
	// directives
	'vip/js/directives/vip-match',
	'vip/js/directives/vip-mvc-antiforgery-token',
	// services
	'vip/js/services/vipServerErrorProcessorService'
], function (require, angular, vip) {
	return vip;
});